import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { BreadcrumbComponent } from './breadcrumb.component';

@NgModule({
  declarations: [
    BreadcrumbComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    NzBreadCrumbModule,
    NzIconModule
  ],
  exports: [
    BreadcrumbComponent
  ]
})
export class BreadcrumbModule { } 