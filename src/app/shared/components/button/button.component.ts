import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-button',
  template: `
    <button
      [type]="type"
      [class]="'btn ' + btnClass + (block ? ' btn-block' : '')"
      [disabled]="disabled"
    >
      <ng-content></ng-content>
    </button>
  `,
  styleUrls: ['./button.component.scss']
})
export class ButtonComponent {
  @Input() type: 'button' | 'submit' | 'reset' = 'button';
  @Input() btnClass: string = 'btn-primary'; // Ex: btn-primary, btn-secondary
  @Input() block: boolean = false;
  @Input() disabled: boolean = false;
} 