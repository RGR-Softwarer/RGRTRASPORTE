import { Component, OnDestroy, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { BreadcrumbService } from '../../services/breadcrumb/breadcrumb.service';
import { Subject, takeUntil, filter, map } from 'rxjs';
import { ItemBreadcrumb } from '../../dominio/interface/ItemBreadcrumb';

@Component({
  selector: 'app-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  styleUrl: './breadcrumb.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BreadcrumbComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  
  breadcrumbs$ = this.breadcrumbService.breadcrumbs$;
  urlAtual: string = '';
  isLoading: boolean = false;

  constructor(
    private breadcrumbService: BreadcrumbService, 
    private router: Router
  ) {}

  ngOnInit(): void {
    this.inicializarObservadores();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private inicializarObservadores(): void {
    // Observar mudanças de rota para atualizar URL atual
    this.router.events
      .pipe(
        filter(event => event instanceof NavigationEnd),
        map((event: NavigationEnd) => {
          // Limpar query parameters da URL
          return event.url.split('?')[0].split('#')[0];
        }),
        takeUntil(this.destroy$)
      )
      .subscribe((url: string) => {
        this.urlAtual = url;
        this.isLoading = false;
      });
    
    // Definir URL inicial (também limpa)
    this.urlAtual = this.router.url.split('?')[0].split('#')[0];
  }

  // Navegar para um breadcrumb específico
  navegarPara(breadcrumb: ItemBreadcrumb): void {
    if (this.podeNavegar(breadcrumb)) {
      this.isLoading = true;
      this.breadcrumbService.navegarPara(breadcrumb.url);
    }
  }

  // Verificar se é possível navegar para o breadcrumb
  podeNavegar(breadcrumb: ItemBreadcrumb): boolean {
    return !!(
      breadcrumb.url && 
      breadcrumb.url.trim() !== '' && 
      !breadcrumb.isActive && 
      !breadcrumb.disabled
    );
  }

  // Obter classe CSS para o breadcrumb
  obterClasseBreadcrumb(breadcrumb: ItemBreadcrumb): string {
    const classes: string[] = [];
    
    if (breadcrumb.isActive) {
      classes.push('breadcrumb-active');
    }
    
    if (breadcrumb.isHome) {
      classes.push('breadcrumb-home');
    }
    
    if (breadcrumb.disabled) {
      classes.push('breadcrumb-disabled');
    }
    
    if (this.podeNavegar(breadcrumb)) {
      classes.push('breadcrumb-clickable');
    }

    return classes.join(' ');
  }

  // Obter tooltip para o breadcrumb
  obterTooltip(breadcrumb: ItemBreadcrumb): string {
    if (breadcrumb.tooltip) {
      return breadcrumb.tooltip;
    }
    
    if (breadcrumb.isActive) {
      return `Página atual: ${breadcrumb.label}`;
    }
    
    if (this.podeNavegar(breadcrumb)) {
      return `Navegar para ${breadcrumb.label}`;
    }
    
    return breadcrumb.label;
  }

  // Identificar se o breadcrumb está na URL atual
  isAtivo(breadcrumb: ItemBreadcrumb): boolean {
    return this.urlAtual === breadcrumb.url || breadcrumb.isActive === true;
  }

  // TrackBy function para melhor performance
  trackByBreadcrumb(index: number, breadcrumb: ItemBreadcrumb): string {
    return `${breadcrumb.url}-${breadcrumb.label}`;
  }
}