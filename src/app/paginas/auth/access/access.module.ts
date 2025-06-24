import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { AccessRoutingModule } from './access-routing.module';
import { AccessComponent } from './access.component';

@NgModule({
    imports: [
        CommonModule,
        RouterModule,
        AccessRoutingModule
    ],
    declarations: [AccessComponent]
})
export class AccessModule { }
