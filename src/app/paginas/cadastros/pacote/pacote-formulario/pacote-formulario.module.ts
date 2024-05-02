import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PacoteFormularioComponent } from './pacote-formulario.component';
import { RouterModule, Routes } from '@angular/router';
import { FormularioModule } from '../../../../componentes/formulario/formulario.module';

const formularioRoutes: Routes = [
  { path: '', component: PacoteFormularioComponent }
];


@NgModule({
  declarations: [PacoteFormularioComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(formularioRoutes),
    FormularioModule
  ]
})
export class PacoteFormularioModule { }
