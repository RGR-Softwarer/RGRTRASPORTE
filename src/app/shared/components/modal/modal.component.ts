import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss']
})
export class ModalComponent {
  @Input() show: boolean = false;
  @Input() title: string = '';
  @Input() closeButton: boolean = true;
  @Output() closed = new EventEmitter<void>();

  close() {
    this.closed.emit();
  }
} 