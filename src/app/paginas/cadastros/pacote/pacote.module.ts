import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PacoteComponent } from './pacote.component';
import { RouterModule, Routes } from '@angular/router';
import { GridModule } from '../../../componentes/grid/grid.module';

const pacoteRoutes: Routes = [
  { path: '', component: PacoteComponent}
];

@NgModule({
  declarations: [PacoteComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(pacoteRoutes),
    GridModule,
  ]
})
export class CadastroPacoteModule { }
