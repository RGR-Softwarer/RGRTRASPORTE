import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CadastroVeiculoComponent } from './cadastro-veiculo.component';
import { RouterModule, Routes } from '@angular/router';
import { GridModule } from '../../../componentes/grid/grid.module';

const dashboardRoutes: Routes = [
  { path: '', component: CadastroVeiculoComponent }
];

@NgModule({
  declarations: [CadastroVeiculoComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(dashboardRoutes),
    GridModule,
  ]
})
export class CadastroVeiculoModule { }
