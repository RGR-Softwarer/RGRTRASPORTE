import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

// Módulos de componentes reutilizáveis
import { BreadcrumbModule } from '../componentes/breadcrumb/breadcrumb.module';
import { CardComponent } from './components/card/card.component';
import { TableComponent } from './components/table/table.component';
import { FormComponent } from './components/form/form.component';
import { NotificationComponent } from './components/notification/notification.component';

const COMMON_MODULES = [
  CommonModule,
  FormsModule,
  ReactiveFormsModule,
  RouterModule,
  BreadcrumbModule
];

@NgModule({
  imports: [
    ...COMMON_MODULES
  ],
  declarations: [
    CardComponent,
    TableComponent,
    FormComponent,
    NotificationComponent
  ],
  exports: [
    ...COMMON_MODULES,
    CardComponent,
    TableComponent,
    FormComponent,
    NotificationComponent
  ]
})
export class SharedModule { } 