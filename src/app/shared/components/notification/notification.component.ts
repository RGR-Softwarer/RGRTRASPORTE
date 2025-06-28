import { Component, OnInit, OnDestroy } from '@angular/core';
import { NotificationService, Notification } from '../../services/notification.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-notification',
  template: `
    <div class="notification-container">
      <div
        *ngFor="let notification of notifications"
        class="notification notification-{{ notification.type }}"
        [@slideIn]
      >
        <div class="notification-content">
          <div class="notification-title">{{ notification.title }}</div>
          <div class="notification-message" *ngIf="notification.message">{{ notification.message }}</div>
        </div>
        <button class="notification-close" (click)="removeNotification(notification.id)">×</button>
      </div>
    </div>
  `,
  styleUrls: ['./notification.component.scss']
})
export class NotificationComponent implements OnInit, OnDestroy {
  notifications: Notification[] = [];
  private subscription: Subscription = new Subscription();

  constructor(private notificationService: NotificationService) {}

  ngOnInit() {
    this.subscription = this.notificationService.getNotifications().subscribe(notification => {
      this.notifications.push(notification);
      
      if (notification.duration) {
        setTimeout(() => {
          this.removeNotification(notification.id);
        }, notification.duration);
      }
    });
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  removeNotification(id: string) {
    this.notifications = this.notifications.filter(n => n.id !== id);
  }
} 