import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ViagemFormularioComponent } from './viagem-formulario.component';
import { RouterModule, Routes } from '@angular/router';
import { FormularioModule } from '../../../../componentes/formulario/formulario.module';

const formularioRoutes: Routes = [
  { path: '', component: ViagemFormularioComponent }
];


@NgModule({
  declarations: [ViagemFormularioComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(formularioRoutes),
    FormularioModule
  ]
})
export class ViagemFormularioModule { }
