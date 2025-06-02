import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, Route, NavigationEnd } from '@angular/router';
import { Subject, takeUntil, filter } from 'rxjs';

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

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.inicializarComponente();
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
}
