import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { GridModule } from '../../../../componentes/grid/grid.module';
import { CadastroModeloVeicularComponent } from './modelo-veicular.component';

const modeloveicularRoutes: Routes = [
  { path: '', component: CadastroModeloVeicularComponent }
];

@NgModule({
  declarations: [CadastroModeloVeicularComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(modeloveicularRoutes),
    GridModule,
  ]
})
export class CadastroModeloVeicularModule { }
