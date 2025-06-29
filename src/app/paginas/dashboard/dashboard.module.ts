import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { DashboardComponent } from './dashboard.component';
import { SharedModule } from '../../shared/shared.module';
import { InputFieldComponent } from '../../shared/components/form/input-field/input-field.component';
import { SelectFieldComponent } from '../../shared/components/form/select-field/select-field.component';
import { ButtonComponent } from '../../shared/components/button/button.component';

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
    SharedModule,
    InputFieldComponent,
    SelectFieldComponent,
    ButtonComponent
  ]
})
export class DashboardModule { }
