import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { Router, Route, NavigationEnd } from '@angular/router';
import { Subject, takeUntil, filter } from 'rxjs';
import { AppContextService } from '../services/context/app.context';
import { AppContext } from '../dominio/entidade/app.context';
import { ToastService } from '../services/utils/notificacao/toast.service';

interface MenuItem {
  path: string;
  breadcrumb: string;
  icon?: string;
  submenu: MenuItem[];
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  
  menuAberto: boolean = false;
  anoAtual: number = new Date().getFullYear();
  menus: MenuItem[] = [];
  rotaAtual: string = '';
  loading: boolean = false;
  usuarioLogado: AppContext | null = null;
  userMenuAberto: boolean = false;
  temaEscuro: boolean = false;

  constructor(
    private router: Router,
    private appContextService: AppContextService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.inicializarComponente();
    this.carregarUsuarioLogado();
    this.carregarTema();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private inicializarComponente(): void {
    this.generateMenuFromRoutes();
    this.observarMudancasRota();
    this.rotaAtual = this.router.url;
  }

  private carregarUsuarioLogado(): void {
    this.usuarioLogado = this.appContextService.obterUsuarioLogado();
  }

  private observarMudancasRota(): void {
    this.router.events
      .pipe(
        filter(event => event instanceof NavigationEnd),
        takeUntil(this.destroy$)
      )
      .subscribe((event: NavigationEnd) => {
        this.rotaAtual = event.url;
        this.fecharMenuMobile();
      });
  }

  /**
   * Gera os menus a partir das rotas configuradas
   */
  private generateMenuFromRoutes(): void {
    const homeRoute = this.router.config.find((route: Route) => route.component === HomeComponent);
    const childRoutes = homeRoute?.children || [];

    this.menus = childRoutes
      .filter(this.isValidMenuItem)
      .map(this.mapRouteToMenuItem.bind(this));
  }

  private isValidMenuItem(route: Route): boolean {
    return !!(route.data?.['breadcrumb'] && !route.data?.['oculta']);
  }

  private mapRouteToMenuItem(route: Route): MenuItem {
    return {
      path: route.path || '',
      breadcrumb: route.data?.['breadcrumb'] || '',
      icon: route.data?.['icon'],
      submenu: this.getSubRoutes(route)
    };
  }

  /**
   * Obtém sub-rotas para criar submenus
   */
  private getSubRoutes(route: Route): MenuItem[] {
    if (!route.children?.length) {
      return [];
    }

    return route.children
      .filter(this.isValidMenuItem)
      .map(subRoute => ({
        path: `${route.path}/${subRoute.path}`,
        breadcrumb: subRoute.data?.['breadcrumb'] || '',
        icon: subRoute.data?.['icon'],
        submenu: []
      }));
  }

  /**
   * Alterna o estado do menu mobile
   */
  toggleMenu(): void {
    this.menuAberto = !this.menuAberto;
  }

  /**
   * Fecha o menu mobile
   */
  fecharMenuMobile(): void {
    this.menuAberto = false;
  }

  /**
   * Navega para uma rota específica
   */
  navegarPara(path: string): void {
    if (path && path !== this.rotaAtual) {
      this.loading = true;
      this.router.navigate([path]).finally(() => {
        this.loading = false;
      });
    }
  }

  /**
   * Verifica se uma rota está ativa
   */
  isRotaAtiva(path: string): boolean {
    return this.rotaAtual.includes(path);
  }

  /**
   * Obtém o ícone padrão se não especificado
   */
  getIcone(item: MenuItem): string {
    return item.icon || 'menu';
  }

  /**
   * Listener para fechar o dropdown quando clicar fora
   */
  @HostListener('document:click', ['$event'])
  onDocumentClick(event: Event): void {
    const target = event.target as HTMLElement;
    if (!target.closest('.dropdown-container')) {
      this.userMenuAberto = false;
    }
  }

  /**
   * Alterna o estado do menu do usuário
   */
  toggleUserMenu(): void {
    this.userMenuAberto = !this.userMenuAberto;
  }

  /**
   * Abre a página de perfil do usuário
   */
  abrirPerfil(): void {
    this.router.navigate(['/auth/profile']);
  }

  /**
   * Realiza o logout do usuário
   */
  fazerLogout(): void {
    this.appContextService.logout();
    this.toastService.exibirMensagemSucesso('Logout', 'Você foi desconectado com sucesso');
    this.router.navigate(['/auth/login']);
  }

  carregarTema(): void {
    const tema = localStorage.getItem('temaEscuro');
    this.temaEscuro = tema === 'true';
    this.aplicarTema();
  }

  alternarTema(): void {
    this.temaEscuro = !this.temaEscuro;
    localStorage.setItem('temaEscuro', this.temaEscuro.toString());
    this.aplicarTema();
  }

  aplicarTema(): void {
    if (this.temaEscuro) {
      document.body.classList.add('dark-theme');
    } else {
      document.body.classList.remove('dark-theme');
    }
  }
}
