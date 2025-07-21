import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface TableColumn {
  key: string;
  label: string;
  type?: 'text' | 'status' | 'actions';
  render?: (value: any, row: any) => string;
}

export interface TableAction {
  label: string;
  icon: string;
  action: (row: any) => void;
  class?: string;
}

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss'],
  standalone: true,
  imports: [CommonModule]
})
export class TableComponent {
  @Input() columns: TableColumn[] = [];
  @Input() data: any[] = [];
  @Input() loading: boolean = false;
  @Input() bordered: boolean = false;
  @Input() size: 'small' | 'middle' | 'default' = 'default';
  @Input() actions: TableAction[] = [];

  @Output() actionClick = new EventEmitter<{ action: string, row: any }>();

  getStatusClass(status: boolean): string {
    return status ? 'status-active' : 'status-inactive';
  }

  getStatusText(status: boolean): string {
    return status ? 'Ativo' : 'Inativo';
  }

  onActionClick(action: TableAction, row: any): void {
    this.actionClick.emit({ action: action.label, row });
    action.action(row);
  }

  trackByFn(index: number, item: any): any {
    return item.id || index;
  }
} 