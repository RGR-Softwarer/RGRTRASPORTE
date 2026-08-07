import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

export interface SidebarMenuItem {
  path: string;
  breadcrumb: string;
  icon?: string;
  submenu: SidebarMenuItem[];
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <aside class="sidebar" [class.collapsed]="collapsed">
      <div class="sidebar-header">
        <div class="logo" *ngIf="!collapsed">
          <img src="assets/layout/images/logo.png" alt="Logo" class="logo-img">
          <span class="logo-text">RGR SISTEMAS</span>
        </div>
        <div class="logo-collapsed" *ngIf="collapsed">
          <img src="assets/layout/images/logo.png" alt="Logo" class="logo-img-small">
        </div>
      </div>
      
      <nav class="sidebar-nav">
        <ul class="nav-menu">
          <ng-container *ngFor="let item of menuItems">
            <!-- Item com submenu -->
            <li *ngIf="item.submenu.length > 0" class="nav-item has-submenu">
              <div class="nav-link" 
                   [class.active]="isActive(item.path)"
                   (click)="toggleSubmenu(item)">
                <i class="nav-icon" [class]="getIconClass(item.icon)"></i>
                <span class="nav-text" *ngIf="!collapsed">{{ item.breadcrumb }}</span>
                <i class="submenu-arrow fas fa-chevron-down" 
                   *ngIf="!collapsed"
                   [class.rotated]="isSubmenuOpen(item)"></i>
              </div>
              <ul class="submenu" [class.open]="isSubmenuOpen(item)">
                <li *ngFor="let subItem of item.submenu" class="submenu-item">
                  <a class="submenu-link" 
                     [routerLink]="subItem.path"
                     routerLinkActive="active"
                     [class.active]="isActive(subItem.path)">
                    <i class="submenu-icon" [class]="getIconClass(subItem.icon)"></i>
                    <span class="submenu-text" *ngIf="!collapsed">{{ subItem.breadcrumb }}</span>
                  </a>
                </li>
              </ul>
            </li>
            
            <!-- Item simples -->
            <li *ngIf="item.submenu.length === 0" class="nav-item">
              <a class="nav-link" 
                 [routerLink]="item.path"
                 routerLinkActive="active"
                 [class.active]="isActive(item.path)">
                <i class="nav-icon" [class]="getIconClass(item.icon)"></i>
                <span class="nav-text" *ngIf="!collapsed">{{ item.breadcrumb }}</span>
              </a>
            </li>
          </ng-container>
        </ul>
      </nav>
      
      <div class="sidebar-footer" *ngIf="!collapsed">
        <button class="collapse-btn" (click)="toggleCollapse()">
          <i class="fas fa-chevron-left"></i>
        </button>
      </div>
    </aside>
  `,
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent {
  @Input() collapsed: boolean = false;
  @Input() menuItems: SidebarMenuItem[] = [];
  @Output() collapsedChange = new EventEmitter<boolean>();
  
  private openSubmenus: Set<string> = new Set();

  toggleCollapse(): void {
    this.collapsed = !this.collapsed;
    this.collapsedChange.emit(this.collapsed);
  }

  toggleSubmenu(item: SidebarMenuItem): void {
    if (this.openSubmenus.has(item.path)) {
      this.openSubmenus.delete(item.path);
    } else {
      this.openSubmenus.add(item.path);
    }
  }

  isSubmenuOpen(item: SidebarMenuItem): boolean {
    return this.openSubmenus.has(item.path);
  }

  isActive(path: string): boolean {
    return window.location.pathname.includes(path);
  }

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
} 