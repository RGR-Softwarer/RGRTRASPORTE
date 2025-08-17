import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { LocalidadeFormularioComponent } from './localidade-formulario.component';
import { FormularioComponent } from '../../../../componentes/formulario/formulario.component';

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormularioComponent,
    LocalidadeFormularioComponent
  ],
  exports: [
    LocalidadeFormularioComponent
  ]
})
export class LocalidadeFormularioModule { }