import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import { DashboardComponent } from './dashboard.component';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';

const dashboardRoutes: Routes = [
    { path: '', component: DashboardComponent }
];

@NgModule({
  declarations: [
    DashboardComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(dashboardRoutes),
    NzBreadCrumbModule,
  ]
})
export class DashboardModule { }
