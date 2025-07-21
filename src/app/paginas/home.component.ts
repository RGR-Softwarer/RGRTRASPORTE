import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { Router, Route, NavigationEnd, RouterOutlet, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Subject, takeUntil, filter } from 'rxjs';
import { AppContextService } from '../services/context/app.context';
import { AppContext } from '../dominio/entidade/app.context';
import { NotificationService, ConfirmService } from '../shared/services';
import { BreadcrumbComponent } from '../componentes/breadcrumb/breadcrumb.component';

interface MenuItem {
  path: string;
  breadcrumb: string;
  icon?: string;
  submenu: MenuItem[];
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  standalone: true,
  imports: [CommonModule, RouterModule, RouterOutlet, BreadcrumbComponent]
})
export class HomeComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  private openSubmenus: Set<string> = new Set();
  
  menuAberto: boolean = false;
  anoAtual: number = new Date().getFullYear();
  menus: MenuItem[] = [
    {
      path: 'inicio',
      breadcrumb: 'Início',
      icon: 'home',
      submenu: []
    },
    {
      path: 'frota',
      breadcrumb: 'Frota',
      icon: 'car',
      submenu: [
        {
          path: 'frota/veiculo',
          breadcrumb: 'Veículos',
          icon: 'car',
          submenu: []
        }
      ]
    },
    {
      path: 'cadastros',
      breadcrumb: 'Cadastros',
      icon: 'database',
      submenu: []
    },
    {
      path: 'relatorios',
      breadcrumb: 'Relatórios',
      icon: 'chart-bar',
      submenu: []
    }
  ];
  rotaAtual: string = '';
  loading: boolean = false;
  usuarioLogado: AppContext | null = null;
  userMenuAberto: boolean = false;
  temaEscuro: boolean = false;

  constructor(
    private router: Router,
    private appContextService: AppContextService,
    private notificationService: NotificationService,
    private confirmService: ConfirmService
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
   * Alterna o estado do menu mobile
   */
  toggleMenu(): void {
    this.menuAberto = !this.menuAberto;
  }

  /**
   * Alterna o estado de um submenu
   */
  toggleSubmenu(item: MenuItem): void {
    if (this.openSubmenus.has(item.path)) {
      this.openSubmenus.delete(item.path);
    } else {
      this.openSubmenus.add(item.path);
    }
  }

  /**
   * Verifica se um submenu está aberto
   */
  isSubmenuOpen(item: MenuItem): boolean {
    return this.openSubmenus.has(item.path);
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
   * Mapeia ícones ng-zorro para Font Awesome
   */
  getIconClass(icon?: string): string {
    if (!icon) return 'fas fa-circle';
    
    // Mapeamento de ícones ng-zorro para Font Awesome
    const iconMap: { [key: string]: string } = {
      'menu': 'fas fa-bars',
      'file': 'fas fa-file',
      'user': 'fas fa-user',
      'dashboard': 'fas fa-tachometer-alt',
      'car': 'fas fa-car',
      'truck': 'fas fa-truck',
      'users': 'fas fa-users',
      'settings': 'fas fa-cog',
      'logout': 'fas fa-sign-out-alt',
      'profile': 'fas fa-user-circle',
      'home': 'fas fa-home',
      'search': 'fas fa-search',
      'plus': 'fas fa-plus',
      'edit': 'fas fa-edit',
      'delete': 'fas fa-trash',
      'eye': 'fas fa-eye',
      'download': 'fas fa-download',
      'upload': 'fas fa-upload',
      'calendar': 'fas fa-calendar',
      'clock': 'fas fa-clock',
      'location': 'fas fa-map-marker-alt',
      'phone': 'fas fa-phone',
      'envelope': 'fas fa-envelope',
      'lock': 'fas fa-lock',
      'unlock': 'fas fa-unlock',
      'check': 'fas fa-check',
      'times': 'fas fa-times',
      'warning': 'fas fa-exclamation-triangle',
      'info': 'fas fa-info-circle',
      'question': 'fas fa-question-circle',
      'star': 'fas fa-star',
      'heart': 'fas fa-heart',
      'bookmark': 'fas fa-bookmark',
      'share': 'fas fa-share',
      'print': 'fas fa-print',
      'save': 'fas fa-save',
      'refresh': 'fas fa-sync',
      'filter': 'fas fa-filter',
      'sort': 'fas fa-sort',
      'list': 'fas fa-list',
      'grid': 'fas fa-th',
      'table': 'fas fa-table',
      'chart': 'fas fa-chart-bar',
      'pie-chart': 'fas fa-chart-pie',
      'line-chart': 'fas fa-chart-line',
      'area-chart': 'fas fa-chart-area',
      'database': 'fas fa-database',
      'server': 'fas fa-server',
      'cloud': 'fas fa-cloud',
      'wifi': 'fas fa-wifi',
      'bluetooth': 'fas fa-bluetooth',
      'mobile': 'fas fa-mobile-alt',
      'desktop': 'fas fa-desktop',
      'laptop': 'fas fa-laptop',
      'tablet': 'fas fa-tablet-alt',
      'keyboard': 'fas fa-keyboard',
      'mouse': 'fas fa-mouse',
      'headphones': 'fas fa-headphones',
      'speaker': 'fas fa-volume-up',
      'microphone': 'fas fa-microphone',
      'camera': 'fas fa-camera',
      'video': 'fas fa-video',
      'image': 'fas fa-image',
      'folder': 'fas fa-folder',
      'file-alt': 'fas fa-file-alt',
      'file-pdf': 'fas fa-file-pdf',
      'file-word': 'fas fa-file-word',
      'file-excel': 'fas fa-file-excel',
      'file-powerpoint': 'fas fa-file-powerpoint',
      'file-archive': 'fas fa-file-archive',
      'file-code': 'fas fa-file-code',
      'file-image': 'fas fa-file-image',
      'file-video': 'fas fa-file-video',
      'file-audio': 'fas fa-file-audio'
    };
    
    return iconMap[icon] || `fas fa-${icon}`;
  }

  /**
   * Listener para fechar o dropdown quando clicar fora
   */
  @HostListener('document:click', ['$event'])
  onDocumentClick(event: Event): void {
    const target = event.target as HTMLElement;
    if (!target.closest('.user-profile')) {
      this.userMenuAberto = false;
    }
  }

  /**
   * Alterna o estado do menu do usuário
   */
  toggleUserMenu(): void {
    console.log('toggleUserMenu chamado, userMenuAberto atual:', this.userMenuAberto);
    this.userMenuAberto = !this.userMenuAberto;
    console.log('toggleUserMenu novo valor:', this.userMenuAberto);
    // Impedir propagação do evento
    event?.stopPropagation();
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
    this.confirmService.confirmAction('Sair', 'Tem certeza que deseja sair do sistema?')
      .subscribe(confirmed => {
        if (confirmed) {
    this.appContextService.logout();
    this.notificationService.success('Logout', 'Você foi desconectado com sucesso');
    this.router.navigate(['/auth/login']);
        }
      });
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
