import { NgModule } from '@angular/core';
import { VeiculoComponent } from './veiculo.component';
import { SharedModule } from '../../../shared/shared.module';
import { RouterModule, Routes } from '@angular/router';

const veiculoRoutes: Routes = [
  { path: '', component: VeiculoComponent }
];

@NgModule({
  declarations: [VeiculoComponent],
  imports: [
    RouterModule.forChild(veiculoRoutes),
    SharedModule
  ],
  exports: [VeiculoComponent]
})
export class VeiculoModule { }
