import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HomeRoutingModule } from './homeRoutingModule';
import { DashboardModule } from './dashboard/dashboard.module';
import { HomeComponent } from './home.component';
import { BreadcrumbModule } from '../componentes/breadcrumb/breadcrumb.module';

@NgModule({
    imports: [
        CommonModule,
        RouterModule,
        HomeRoutingModule,
        FormsModule,
        ReactiveFormsModule,
        DashboardModule,
        BreadcrumbModule,
        HomeComponent
    ],
    declarations: []
})
export class HomeModule { }
