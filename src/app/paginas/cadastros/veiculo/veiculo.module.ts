import { NgModule } from '@angular/core';
import { VeiculoComponent } from './veiculo.component';
import { RouterModule, Routes } from '@angular/router';

const veiculoRoutes: Routes = [
  { path: '', component: VeiculoComponent }
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(veiculoRoutes),
    VeiculoComponent
  ],
  exports: [VeiculoComponent]
})
export class VeiculoModule { }
