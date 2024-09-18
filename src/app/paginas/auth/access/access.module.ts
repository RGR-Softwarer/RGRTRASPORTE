import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AccessRoutingModule } from './access-routing.module';
import { AccessComponent } from './access.component';

@NgModule({
    imports: [
        CommonModule,
        AccessRoutingModule
    ],
    declarations: [AccessComponent]
})
export class AccessModule { }
