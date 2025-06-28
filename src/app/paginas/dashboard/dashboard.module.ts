import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { DashboardComponent } from './dashboard.component';
import { SharedModule } from '../../shared/shared.module';

const dashboardRoutes: Routes = [
    { path: '', component: DashboardComponent }
];

@NgModule({
  declarations: [
    DashboardComponent,
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(dashboardRoutes),
    ReactiveFormsModule,
    SharedModule
  ]
})
export class DashboardModule { }
