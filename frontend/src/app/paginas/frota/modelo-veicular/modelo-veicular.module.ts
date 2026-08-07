import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';

import { ModeloVeicularComponent } from './modelo-veicular.component';
import { ModeloVeicularFormularioComponent } from './modelo-veicular-formulario/modelo-veicular-formulario.component';
import { GridComponent } from '../../../componentes/grid/grid.component';
import { FormularioComponent } from '../../../componentes/formulario/formulario.component';

const routes: Routes = [
  {
    path: '',
    component: ModeloVeicularComponent
  },
  {
    path: 'adicionar',
    component: ModeloVeicularFormularioComponent
  },
  {
    path: 'editar/:id',
    component: ModeloVeicularFormularioComponent
  }
];

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes),
    GridComponent,
    FormularioComponent,
    ModeloVeicularComponent,
    ModeloVeicularFormularioComponent
  ],
  exports: [
    ModeloVeicularComponent,
    ModeloVeicularFormularioComponent
  ]
})
export class ModeloVeicularModule { }