import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

// Módulos de componentes reutilizáveis
import { BreadcrumbModule } from '../componentes/breadcrumb/breadcrumb.module';
import { FormularioModule } from '../componentes/formulario/formulario.module';
import { GridModule } from '../componentes/grid/grid.module';

// Módulos de UI de terceiros (ex: NG-ZORRO)
// Adicionar aqui outros módulos do NG-ZORRO que são amplamente utilizados
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzInputModule } from 'ng-zorro-antd/input';


const COMMON_MODULES = [
  CommonModule,
  FormsModule,
  ReactiveFormsModule,
  RouterModule,
  BreadcrumbModule,
  FormularioModule,
  GridModule,
  NzButtonModule,
  NzIconModule,
  NzTableModule,
  NzInputModule
];

@NgModule({
  imports: [
    ...COMMON_MODULES
  ],
  exports: [
    ...COMMON_MODULES
  ]
})
export class SharedModule { } 