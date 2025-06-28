import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

export interface Notification {
  id: string;
  type: 'success' | 'error' | 'warning' | 'info';
  title: string;
  message: string;
  duration?: number;
}

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private notifications$ = new Subject<Notification>();

  constructor() {}

  getNotifications() {
    return this.notifications$.asObservable();
  }

  success(title: string, message: string = '', duration: number = 3000) {
    this.showNotification({
      id: this.generateId(),
      type: 'success',
      title,
      message,
      duration
    });
  }

  error(title: string, message: string = '', duration: number = 5000) {
    this.showNotification({
      id: this.generateId(),
      type: 'error',
      title,
      message,
      duration
    });
  }

  warning(title: string, message: string = '', duration: number = 4000) {
    this.showNotification({
      id: this.generateId(),
      type: 'warning',
      title,
      message,
      duration
    });
  }

  info(title: string, message: string = '', duration: number = 3000) {
    this.showNotification({
      id: this.generateId(),
      type: 'info',
      title,
      message,
      duration
    });
  }

  private showNotification(notification: Notification) {
    this.notifications$.next(notification);
  }

  private generateId(): string {
    return Math.random().toString(36).substr(2, 9);
  }
} 