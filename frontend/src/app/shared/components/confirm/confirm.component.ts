import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ConfirmService, ConfirmConfig } from '../../services/confirm.service';
import { Subscription } from 'rxjs';
import { trigger, state, style, transition, animate } from '@angular/animations';

@Component({
  selector: 'app-confirm',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="confirm-overlay" *ngIf="visible" (click)="onOverlayClick()">
      <div class="confirm-modal" [@slideIn] (click)="$event.stopPropagation()">
        <div class="confirm-header">
          <div class="confirm-icon" *ngIf="config && config.icon">
            <i class="fas fa-{{ config.icon }} icon-{{ config.okType ?? 'primary' }}"></i>
          </div>
          <h3 class="confirm-title">{{ config?.title ?? 'Confirmar' }}</h3>
          <button 
            class="confirm-close" 
            *ngIf="config?.closable !== false"
            (click)="onCancel()"
            aria-label="Fechar">
            <i class="fas fa-times"></i>
          </button>
        </div>
        
        <div class="confirm-content">
          <p class="confirm-message">{{ config?.content }}</p>
        </div>
        
        <div class="confirm-footer">
          <button 
            class="btn btn-{{ config?.cancelType || 'default' }}"
            *ngIf="config?.cancelText"
            (click)="onCancel()">
            {{ config?.cancelText }}
          </button>
          <button 
            class="btn btn-{{ config?.okType || 'primary' }}"
            (click)="onConfirm()">
            {{ config?.okText || 'OK' }}
          </button>
        </div>
      </div>
    </div>
  `,
  styleUrls: ['./confirm.component.scss'],
  animations: [
    trigger('slideIn', [
      state('void', style({
        opacity: 0,
        transform: 'scale(0.8) translateY(-20px)'
      })),
      state('*', style({
        opacity: 1,
        transform: 'scale(1) translateY(0)'
      })),
      transition('void => *', [
        animate('200ms ease-out')
      ]),
      transition('* => void', [
        animate('150ms ease-in')
      ])
    ])
  ]
})
export class ConfirmComponent implements OnInit, OnDestroy {
  visible = false;
  config: ConfirmConfig | null = null;
  private subscription: Subscription = new Subscription();

  constructor(private confirmService: ConfirmService) {}

  ngOnInit() {
    this.subscription = this.confirmService.getConfirmStream().subscribe(config => {
      this.config = config;
      this.visible = true;
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  onConfirm() {
    this.confirmService.respond(true);
    this.visible = false;
  }

  onCancel() {
    this.confirmService.respond(false);
    this.visible = false;
  }

  onOverlayClick() {
    if (this.config?.maskClosable !== false) {
      this.onCancel();
    }
  }
} 