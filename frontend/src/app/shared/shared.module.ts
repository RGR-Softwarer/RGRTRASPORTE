import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

// Módulos de componentes reutilizáveis
import { BreadcrumbModule } from '../componentes/breadcrumb/breadcrumb.module';


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
  declarations: [],
  exports: [
    ...COMMON_MODULES
  ]
})
export class SharedModule { } 