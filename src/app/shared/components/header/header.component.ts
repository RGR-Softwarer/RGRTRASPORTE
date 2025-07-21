import { Component, Input, Output, EventEmitter, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

export interface UserInfo {
  username?: string;
  email?: string;
  avatar?: string;
}

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <header class="header">
      <div class="header-content">
        <div class="header-left">
          <button class="menu-toggle" (click)="toggleMenu.emit()">
            <i class="fas fa-bars"></i>
          </button>
          <div class="breadcrumb-container">
            <ng-content select="[breadcrumb]"></ng-content>
          </div>
        </div>
        
        <div class="header-right">
          <div class="user-profile">
            <div class="dropdown-container">
              <button class="user-button" (click)="toggleUserMenu()">
                <div class="user-avatar">
                  <img *ngIf="user?.avatar" [src]="user.avatar" [alt]="user?.username || 'Usuário'">
                  <i *ngIf="!user?.avatar" class="fas fa-user"></i>
                </div>
                <span class="username" *ngIf="!collapsed">{{ user?.username || 'Usuário' }}</span>
                <i class="dropdown-arrow fas fa-chevron-down" 
                   [class.rotated]="userMenuOpen"
                   *ngIf="!collapsed"></i>
              </button>
              
              <div class="dropdown-menu" [class.open]="userMenuOpen">
                <div class="dropdown-header">
                  <div class="user-info">
                    <div class="user-avatar-large">
                      <img *ngIf="user?.avatar" [src]="user.avatar" [alt]="user?.username || 'Usuário'">
                      <i *ngIf="!user?.avatar" class="fas fa-user"></i>
                    </div>
                    <div class="user-details">
                      <div class="user-name">{{ user?.username || 'Usuário' }}</div>
                      <div class="user-email" *ngIf="user?.email">{{ user.email }}</div>
                    </div>
                  </div>
                </div>
                
                <div class="dropdown-divider"></div>
                
                <ul class="menu-list">
                  <li class="menu-item" (click)="onProfileClick(); userMenuOpen = false;">
                    <i class="menu-icon fas fa-user-circle"></i>
                    <span class="menu-text">Meu Perfil</span>
                  </li>
                  
                  <li class="menu-item" (click)="onThemeToggle(); userMenuOpen = false;">
                    <i class="menu-icon" [class]="darkTheme ? 'fas fa-sun' : 'fas fa-moon'"></i>
                    <span class="menu-text">{{ darkTheme ? 'Tema claro' : 'Tema escuro' }}</span>
                  </li>
                  
                  <div class="dropdown-divider"></div>
                  
                  <li class="menu-item" (click)="onLogoutClick(); userMenuOpen = false;">
                    <i class="menu-icon fas fa-sign-out-alt"></i>
                    <span class="menu-text">Sair</span>
                  </li>
                </ul>
              </div>
            </div>
          </div>
        </div>
      </div>
    </header>
  `,
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
  @Input() collapsed: boolean = false;
  @Input() user?: UserInfo;
  @Input() darkTheme: boolean = false;
  @Output() toggleMenu = new EventEmitter<void>();
  @Output() profileClick = new EventEmitter<void>();
  @Output() themeToggle = new EventEmitter<void>();
  @Output() logoutClick = new EventEmitter<void>();
  
  userMenuOpen: boolean = false;

  toggleUserMenu(): void {
    this.userMenuOpen = !this.userMenuOpen;
  }

  onProfileClick(): void {
    this.profileClick.emit();
  }

  onThemeToggle(): void {
    this.themeToggle.emit();
  }

  onLogoutClick(): void {
    this.logoutClick.emit();
  }

  // Fechar dropdown quando clicar fora
  @HostListener('document:click', ['$event'])
  onDocumentClick(event: Event): void {
    const target = event.target as HTMLElement;
    if (!target.closest('.dropdown-container')) {
      this.userMenuOpen = false;
    }
  }
} 