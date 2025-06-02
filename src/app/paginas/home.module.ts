import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HomeRoutingModule } from './homeRoutingModule';
import { DashboardModule } from './dashboard/dashboard.module';
import { HomeComponent } from './home.component';
import { BreadcrumbModule } from '../componentes/breadcrumb/breadcrumb.module';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzMenuModule } from 'ng-zorro-antd/menu';

@NgModule({
    imports: [
        CommonModule,
        RouterModule,
        HomeRoutingModule,
        FormsModule,
        ReactiveFormsModule,
        DashboardModule,
        BreadcrumbModule,
        NzLayoutModule,
        NzIconModule,
        NzMenuModule
    ],
    declarations: [HomeComponent]
})
export class HomeModule { }
