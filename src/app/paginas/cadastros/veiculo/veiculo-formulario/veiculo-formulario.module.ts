import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VeiculoFormularioComponent } from './veiculo-formulario.component';
import { RouterModule, Routes } from '@angular/router';
import { FormularioModule } from '../../../../componentes/formulario/formulario.module';

const formularioRoutes: Routes = [
  { path: '', component: VeiculoFormularioComponent }
];


@NgModule({
  declarations: [VeiculoFormularioComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(formularioRoutes),
    FormularioModule
  ]
})
export class VeiculoFormularioModule { }
