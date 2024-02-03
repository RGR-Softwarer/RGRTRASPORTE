import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CadastroVeiculoComponent } from './veiculo.component';
import { RouterModule, Routes } from '@angular/router';
import { GridModule } from '../../../componentes/grid/grid.module';

const veiculoRoutes: Routes = [
  { path: '', component: CadastroVeiculoComponent }
];

@NgModule({
  declarations: [CadastroVeiculoComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(veiculoRoutes),
    GridModule,
  ]
})
export class CadastroVeiculoModule { }
