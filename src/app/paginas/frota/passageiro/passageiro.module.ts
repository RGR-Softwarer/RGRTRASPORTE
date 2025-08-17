import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';

import { PassageiroComponent } from './passageiro.component';
import { PassageiroFormularioComponent } from './passageiro-formulario/passageiro-formulario.component';
import { GridComponent } from '../../../componentes/grid/grid.component';
import { FormularioComponent } from '../../../componentes/formulario/formulario.component';

const routes: Routes = [
  {
    path: '',
    component: PassageiroComponent
  },
  {
    path: 'adicionar',
    component: PassageiroFormularioComponent
  },
  {
    path: 'editar/:id',
    component: PassageiroFormularioComponent
  }
];

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes),
    GridComponent,
    FormularioComponent,
    PassageiroComponent,
    PassageiroFormularioComponent
  ],
  exports: [
    PassageiroComponent,
    PassageiroFormularioComponent
  ]
})
export class PassageiroModule { }