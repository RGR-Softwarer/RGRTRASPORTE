import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-button',
  standalone: true,
  imports: [CommonModule],
  template: `
    <button
      [type]="type"
      [class]="'btn ' + btnClass + (block ? ' btn-block' : '')"
      [disabled]="disabled"
      [title]="title"
    >
      <ng-content></ng-content>
    </button>
  `,
  styleUrls: ['./button.component.scss']
})
export class ButtonComponent {
  @Input() type: 'button' | 'submit' | 'reset' = 'button';
  @Input() variant: 'primary' | 'secondary' | 'success' | 'danger' | 'warning' | 'info' | 'light' | 'dark' = 'primary';
  @Input() btnClass: string = 'btn-primary'; // Ex: btn-primary, btn-secondary
  @Input() block: boolean = false;
  @Input() disabled: boolean = false;
  @Input() title: string = '';

  ngOnInit() {
    if (this.variant && !this.btnClass.includes('btn-')) {
      this.btnClass = `btn-${this.variant}`;
    }
  }
} 