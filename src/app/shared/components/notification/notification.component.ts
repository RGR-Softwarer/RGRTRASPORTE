import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotificationService, Notification } from '../../services/notification.service';
import { Subscription } from 'rxjs';
import { trigger, state, style, transition, animate } from '@angular/animations';

@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [CommonModule],
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
  styleUrls: ['./notification.component.scss'],
  animations: [
    trigger('slideIn', [
      state('void', style({
        transform: 'translateX(100%)',
        opacity: 0
      })),
      state('*', style({
        transform: 'translateX(0)',
        opacity: 1
      })),
      transition('void => *', [
        animate('300ms ease-out')
      ]),
      transition('* => void', [
        animate('200ms ease-in')
      ])
    ])
  ]
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