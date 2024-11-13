import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { FormularioModule } from '../../../../../componentes/formulario/formulario.module';
import { ModeloVeicularFormularioComponent } from './modelo-veicular-formulario.component';

const formularioRoutes: Routes = [
  { path: '', component: ModeloVeicularFormularioComponent }
];


@NgModule({
  declarations: [ModeloVeicularFormularioComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(formularioRoutes),
    FormularioModule
  ]
})
export class ModeloVeicularFormularioModule { }
