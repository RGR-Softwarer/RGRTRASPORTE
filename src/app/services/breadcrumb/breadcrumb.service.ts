import { Injectable } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter, map, distinctUntilChanged } from 'rxjs/operators';
import { BehaviorSubject, Subject } from 'rxjs';
import { ItemBreadcrumb } from '../../dominio/interface/ItemBreadcrumb';

@Injectable({
  providedIn: 'root'
})
export class BreadcrumbService {
  private breadcrumbs = new BehaviorSubject<ItemBreadcrumb[]>([]);
  private routeDataCache = new Map<string, any>();
  
  breadcrumbs$ = this.breadcrumbs.asObservable().pipe(
    distinctUntilChanged((prev, curr) => JSON.stringify(prev) === JSON.stringify(curr))
  );

  constructor(private router: Router, private activatedRoute: ActivatedRoute) {
    this.inicializarObservacaoRotas();
  }

  private inicializarObservacaoRotas(): void {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      map((event: NavigationEnd) => event.url)
    ).subscribe(url => {
      this.atualizarBreadcrumbs(url);
    });

    // Inicializar com a rota atual
    this.atualizarBreadcrumbs(this.router.url);
  }

  private atualizarBreadcrumbs(url: string): void {
    try {
      // Limpar query parameters e fragments da URL
      const urlLimpa = url.split('?')[0].split('#')[0];
      const breadcrumbs = this.construirBreadcrumbsOtimizado(urlLimpa);
      this.breadcrumbs.next(breadcrumbs);
    } catch (error) {
      console.error('Erro ao construir breadcrumbs:', error);
      this.breadcrumbs.next([]);
    }
  }

  private construirBreadcrumbsOtimizado(url: string): ItemBreadcrumb[] {
    const breadcrumbs: ItemBreadcrumb[] = [];
    const segmentosUrl = this.extrairSegmentosUrl(url);
    
    // Adicionar sempre o item Home
    breadcrumbs.push({
      label: 'Início',
      url: '/inicio',
      icon: 'home',
      isHome: true
    });

    let urlAcumulada = '';
    
    for (let i = 0; i < segmentosUrl.length; i++) {
      const segmento = segmentosUrl[i];
      urlAcumulada += `/${segmento}`;
      
      const dadosRota = this.obterDadosRota(urlAcumulada);
      
      // Pular se a rota está marcada como oculta
      if (dadosRota?.oculta) {
        continue;
      }

      // Verificação adicional: ocultar rotas de formulário automaticamente
      if (this.isRotaFormulario(segmento, urlAcumulada)) {
        continue;
      }

      const breadcrumb = this.criarItemBreadcrumb(segmento, urlAcumulada, dadosRota, i === segmentosUrl.length - 1);
      
      // Evitar duplicatas
      if (!this.breadcrumbJaExiste(breadcrumbs, breadcrumb)) {
        breadcrumbs.push(breadcrumb);
      }
    }

    return this.filtrarBreadcrumbsValidos(breadcrumbs);
  }

  private extrairSegmentosUrl(url: string): string[] {
    // Remover query parameters e fragments da URL
    const urlLimpa = url.split('?')[0].split('#')[0];
    return urlLimpa.split('/').filter(segmento => segmento && segmento.trim() !== '');
  }

  private obterDadosRota(url: string): any {
    // Cache dos dados da rota para performance
    if (this.routeDataCache.has(url)) {
      return this.routeDataCache.get(url);
    }

    let dadosRota = null;

    // Verificação de segurança para activatedRoute.root
    if (this.activatedRoute && this.activatedRoute.root) {
      dadosRota = this.buscarDadosRotaRecursivo(this.activatedRoute.root, url);
    }
    
    // Se não encontrou pelos dados da rota ativa, tentar buscar na configuração de rotas
    if (!dadosRota) {
      const dadosConfig = this.buscarDadosNaConfiguracaoRotas(url);
      this.routeDataCache.set(url, dadosConfig);
      return dadosConfig;
    }
    
    this.routeDataCache.set(url, dadosRota);
    return dadosRota;
  }

  private buscarDadosRotaRecursivo(rota: ActivatedRoute, urlProcurada: string): any {
    // Verificação de segurança para rota e snapshot
    if (!rota || !rota.snapshot) {
      return null;
    }

    // Verificar rota atual
    const urlRota = this.construirUrlRota(rota);
    if (urlRota === urlProcurada) {
      return rota.snapshot.data;
    }

    // Buscar em filhos (com verificação de segurança)
    if (rota.children && rota.children.length > 0) {
      for (const filho of rota.children) {
        const resultado = this.buscarDadosRotaRecursivo(filho, urlProcurada);
        if (resultado) {
          return resultado;
        }
      }
    }

    return null;
  }

  private construirUrlRota(rota: ActivatedRoute): string {
    const segmentos: string[] = [];
    let rotaAtual: ActivatedRoute | null = rota;

    while (rotaAtual) {
      // Verificação de segurança para snapshot e url
      if (rotaAtual.snapshot && rotaAtual.snapshot.url && rotaAtual.snapshot.url.length > 0) {
        segmentos.unshift(...rotaAtual.snapshot.url.map(s => s.path));
      }
      rotaAtual = rotaAtual.parent;
    }

    return '/' + segmentos.join('/');
  }

  private buscarDadosNaConfiguracaoRotas(url: string): any {
    // Mapear configurações conhecidas para casos onde a rota ativa não tem os dados
    const configuracaoRotas: { [key: string]: any } = {
      '/frota/veiculo/adicionar': { breadcrumb: 'Adicionar Veículo', icon: 'plus', oculta: true },
      '/frota/veiculo/editar': { breadcrumb: 'Editar Veículo', icon: 'edit', oculta: true },
      '/frota/modeloveicular/adicionar': { breadcrumb: 'Adicionar Modelo', icon: 'plus', oculta: true },
      '/frota/modeloveicular/editar': { breadcrumb: 'Editar Modelo', icon: 'edit', oculta: true },
      '/frota': { breadcrumb: 'Frota', icon: 'car' },
      '/frota/veiculo': { breadcrumb: 'Veículo', icon: 'car' },
      '/frota/modeloveicular': { breadcrumb: 'Modelo Veicular', icon: 'car' },
      '/inicio': { breadcrumb: 'Início', icon: 'home' }
    };

    return configuracaoRotas[url] || null;
  }

  private criarItemBreadcrumb(segmento: string, url: string, dadosRota: any, isUltimo: boolean): ItemBreadcrumb {
    const label = this.obterLabelBreadcrumb(segmento, dadosRota);
    const icon = dadosRota?.icon || this.obterIconePadrao(segmento);

    return {
      label,
      url: isUltimo ? '' : url, // URL vazia para o último item (não clicável)
      icon,
      isActive: isUltimo,
      isHome: false
    };
  }

  private obterLabelBreadcrumb(segmento: string, dadosRota: any): string {
    if (dadosRota?.breadcrumb) {
      return dadosRota.breadcrumb;
    }

    // Mapeamento de segmentos para labels mais amigáveis
    const mapeamentoLabels: { [key: string]: string } = {
      'frota': 'Frota',
      'veiculo': 'Veículo',
      'modeloveicular': 'Modelo Veicular',
      'adicionar': 'Adicionar',
      'editar': 'Editar',
      'inicio': 'Início',
      'dashboard': 'Dashboard'
    };

    return mapeamentoLabels[segmento.toLowerCase()] || this.formatarLabel(segmento);
  }

  private obterIconePadrao(segmento: string): string {
    const mapeamentoIcones: { [key: string]: string } = {
      'frota': 'car',
      'veiculo': 'car',
      'modeloveicular': 'car',
      'adicionar': 'plus',
      'editar': 'edit',
      'dashboard': 'dashboard'
    };

    return mapeamentoIcones[segmento.toLowerCase()] || 'folder';
  }

  private formatarLabel(texto: string): string {
    return texto
      .replace(/([A-Z])/g, ' $1') // Adicionar espaço antes de maiúsculas
      .replace(/^./, str => str.toUpperCase()) // Primeira letra maiúscula
      .trim();
  }

  private breadcrumbJaExiste(breadcrumbs: ItemBreadcrumb[], novoBreadcrumb: ItemBreadcrumb): boolean {
    return breadcrumbs.some(b => 
      b.label === novoBreadcrumb.label && 
      b.url === novoBreadcrumb.url
    );
  }

  private filtrarBreadcrumbsValidos(breadcrumbs: ItemBreadcrumb[]): ItemBreadcrumb[] {
    return breadcrumbs.filter(b => b.label && b.label.trim() !== '');
  }

  // Métodos utilitários públicos
  navegarPara(url: string): void {
    if (url && url.trim() !== '') {
      this.router.navigate([url]);
    }
  }

  limparCache(): void {
    this.routeDataCache.clear();
  }

  adicionarBreadcrumbCustomizado(item: ItemBreadcrumb): void {
    const breadcrumbsAtuais = this.breadcrumbs.value;
    this.breadcrumbs.next([...breadcrumbsAtuais, item]);
  }

  private isRotaFormulario(segmento: string, url: string): boolean {
    const rotasFormulario = ['adicionar', 'editar'];
    return rotasFormulario.includes(segmento.toLowerCase());
  }
}