import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ViagemComponent } from './viagem.component';
import { RouterModule, Routes } from '@angular/router';
import { GridModule } from '../../../componentes/grid/grid.module';

const veiculoRoutes: Routes = [
  { path: '', component: ViagemComponent }
];

@NgModule({
  declarations: [ViagemComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(veiculoRoutes),
    GridModule,
  ]
})
export class CadastroViagemModule { }