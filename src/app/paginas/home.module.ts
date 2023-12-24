import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HomeRoutingModule } from './homeRoutingModule';
import { DashboardModule } from './dashboard/dashboard.module';
import { HomeComponent } from './home.component';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { BreadcrumbComponent } from '../componentes/breadcrumb/breadcrumb.component';

@NgModule({
    imports: [
        CommonModule,
        HomeRoutingModule,
        FormsModule,
        DashboardModule,
        NzLayoutModule,
        NzBreadCrumbModule,
        NzIconModule,
        NzMenuModule,
    ],
    declarations: [HomeComponent, BreadcrumbComponent]
})
export class HomeModule { }
