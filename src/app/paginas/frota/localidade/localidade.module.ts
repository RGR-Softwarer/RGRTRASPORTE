import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';

import { LocalidadeComponent } from './localidade.component';
import { LocalidadeFormularioComponent } from './localidade-formulario/localidade-formulario.component';
import { GridComponent } from '../../../componentes/grid/grid.component';
import { FormularioComponent } from '../../../componentes/formulario/formulario.component';

const routes: Routes = [
  {
    path: '',
    component: LocalidadeComponent
  },
  {
    path: 'adicionar',
    component: LocalidadeFormularioComponent
  },
  {
    path: 'editar/:id',
    component: LocalidadeFormularioComponent
  }
];

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes),
    GridComponent,
    FormularioComponent,
    LocalidadeComponent,
    LocalidadeFormularioComponent
  ],
  exports: [
    LocalidadeComponent,
    LocalidadeFormularioComponent
  ]
})
export class LocalidadeModule { }