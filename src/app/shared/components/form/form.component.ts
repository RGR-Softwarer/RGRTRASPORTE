import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class FormComponent {
  @Input() formGroup: any;
  @Input() submitLabel: string = 'Salvar';
  @Input() loading: boolean = false;
} 