import { NgModule } from '@angular/core';
import { VeiculoComponent } from './veiculo.component';
import { SharedModule } from '../../../shared/shared.module';
import { GridComponent } from '../../../componentes/grid/grid.component';
import { RouterModule, Routes } from '@angular/router';

const veiculoRoutes: Routes = [
  { path: '', component: VeiculoComponent }
];

@NgModule({
  declarations: [VeiculoComponent],
  imports: [
    RouterModule.forChild(veiculoRoutes),
    SharedModule,
    GridComponent
  ],
  exports: [VeiculoComponent]
})
export class VeiculoModule { }
