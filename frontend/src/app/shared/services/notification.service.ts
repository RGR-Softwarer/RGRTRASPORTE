import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';

export interface Notification {
  id: string;
  type: 'success' | 'error' | 'warning' | 'info' | 'loading';
  title: string;
  message: string;
  duration?: number;
  closable?: boolean;
  icon?: string;
  action?: {
    text: string;
    callback: () => void;
  };
}

export interface MessageConfig {
  duration?: number;
  closable?: boolean;
  icon?: string;
}

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private notifications$ = new Subject<Notification>();
  private messages$ = new Subject<Notification>();

  constructor() {}

  // Métodos para notificações (toast)
  getNotifications(): Observable<Notification> {
    return this.notifications$.asObservable();
  }

  success(title: string, message: string = '', config: MessageConfig = {}) {
    this.showNotification({
      id: this.generateId(),
      type: 'success',
      title,
      message,
      duration: config.duration ?? 3000,
      closable: config.closable ?? true,
      icon: config.icon ?? 'check-circle'
    });
  }

  error(title: string, message: string = '', config: MessageConfig = {}) {
    this.showNotification({
      id: this.generateId(),
      type: 'error',
      title,
      message,
      duration: config.duration ?? 5000,
      closable: config.closable ?? true,
      icon: config.icon ?? 'exclamation-circle'
    });
  }

  warning(title: string, message: string = '', config: MessageConfig = {}) {
    this.showNotification({
      id: this.generateId(),
      type: 'warning',
      title,
      message,
      duration: config.duration ?? 4000,
      closable: config.closable ?? true,
      icon: config.icon ?? 'exclamation-triangle'
    });
  }

  info(title: string, message: string = '', config: MessageConfig = {}) {
    this.showNotification({
      id: this.generateId(),
      type: 'info',
      title,
      message,
      duration: config.duration ?? 3000,
      closable: config.closable ?? true,
      icon: config.icon ?? 'info-circle'
    });
  }

  loading(title: string, message: string = '', config: MessageConfig = {}) {
    return this.showNotification({
      id: this.generateId(),
      type: 'loading',
      title,
      message,
      duration: 0, // Não fecha automaticamente
      closable: config.closable ?? false,
      icon: config.icon ?? 'spinner'
    });
  }

  // Métodos para mensagens (message)
  getMessages(): Observable<Notification> {
    return this.messages$.asObservable();
  }

  message(content: string, config: MessageConfig = {}) {
    this.showMessage({
      id: this.generateId(),
      type: 'info',
      title: '',
      message: content,
      duration: config.duration ?? 3000,
      closable: config.closable ?? true,
      icon: config.icon ?? 'info-circle'
    });
  }

  // Métodos de conveniência para mensagens
  successMessage(content: string, config: MessageConfig = {}) {
    this.showMessage({
      id: this.generateId(),
      type: 'success',
      title: '',
      message: content,
      duration: config.duration ?? 3000,
      closable: config.closable ?? true,
      icon: config.icon ?? 'check-circle'
    });
  }

  errorMessage(content: string, config: MessageConfig = {}) {
    this.showMessage({
      id: this.generateId(),
      type: 'error',
      title: '',
      message: content,
      duration: config.duration ?? 5000,
      closable: config.closable ?? true,
      icon: config.icon ?? 'exclamation-circle'
    });
  }

  warningMessage(content: string, config: MessageConfig = {}) {
    this.showMessage({
      id: this.generateId(),
      type: 'warning',
      title: '',
      message: content,
      duration: config.duration ?? 4000,
      closable: config.closable ?? true,
      icon: config.icon ?? 'exclamation-triangle'
    });
  }

  infoMessage(content: string, config: MessageConfig = {}) {
    this.showMessage({
      id: this.generateId(),
      type: 'info',
      title: '',
      message: content,
      duration: config.duration ?? 3000,
      closable: config.closable ?? true,
      icon: config.icon ?? 'info-circle'
    });
  }

  loadingMessage(content: string, config: MessageConfig = {}) {
    return this.showMessage({
      id: this.generateId(),
      type: 'loading',
      title: '',
      message: content,
      duration: 0,
      closable: config.closable ?? false,
      icon: config.icon ?? 'spinner'
    });
  }

  private showNotification(notification: Notification) {
    this.notifications$.next(notification);
  }

  private showMessage(message: Notification) {
    this.messages$.next(message);
  }

  private generateId(): string {
    return Math.random().toString(36).substr(2, 9);
  }

  // Método para remover notificação específica
  remove(id: string) {
    // Implementar lógica de remoção se necessário
  }

  // Método para limpar todas as notificações
  clear() {
    // Implementar lógica de limpeza se necessário
  }
} 