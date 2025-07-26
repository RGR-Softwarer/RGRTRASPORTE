import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VeiculoFormularioComponent } from './veiculo-formulario.component';
import { RouterModule, Routes } from '@angular/router';

const formularioRoutes: Routes = [
  { path: '', component: VeiculoFormularioComponent }
];

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild(formularioRoutes),
    VeiculoFormularioComponent
  ]
})
export class VeiculoFormularioModule { }
