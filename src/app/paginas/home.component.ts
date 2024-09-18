import { Component, OnInit } from '@angular/core';
import { Router, Route } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  menuAberto = false;
  anoAtual: number = new Date().getFullYear();
  menus: any[] = [];

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.generateMenuFromRoutes();
  }

  // Gera os menus a partir das rotas
  generateMenuFromRoutes() {
    const childRoutes = this.router.config.find((route: Route) => route.component === HomeComponent)?.children || [];

    this.menus = childRoutes
      .filter((route: Route) => route.data && route.data['breadcrumb'] && !route.data['oculta']) // Exclui rotas com hideFromMenu
      .map((route: Route) => {
        return {
          path: route.path,
          breadcrumb: route.data?.['breadcrumb'],
          icon: route.data?.['icon'],  // Captura o ícone das rotas
          submenu: this.getSubRoutes(route) // Obtém sub-rotas
        };
      });
  }

  // Função para pegar sub-rotas (submenus)
  getSubRoutes(route: Route): any[] {
    if (route.children && route.children.length > 0) {
      return route.children
        .filter((subRoute: Route) => subRoute.data && subRoute.data['breadcrumb'] && !subRoute.data['oculta']) // Exclui sub-rotas com hideFromMenu
        .map((subRoute: Route) => ({
          path: `${route.path}/${subRoute.path}`, // Caminho completo da sub-rota
          breadcrumb: subRoute.data?.['breadcrumb'],
          icon: subRoute.data?.['icon'] // Captura o ícone das sub-rotas
        }));
    }
    return [];
  }
}
