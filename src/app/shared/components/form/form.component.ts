import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.scss']
})
export class FormComponent {
  @Input() formGroup: any;
  @Input() submitLabel: string = 'Salvar';
  @Input() loading: boolean = false;
} 