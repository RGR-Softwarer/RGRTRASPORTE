import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginRoutingModule } from './login-routing.module';
import { LoginComponent } from './login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CheckboxModule } from 'primeng/checkbox';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';

@NgModule({
    imports: [
        CommonModule,
        LoginRoutingModule,
        FormsModule,
        CheckboxModule,
        ButtonModule,
        ReactiveFormsModule,
        CardModule,
    ],
    declarations: [LoginComponent]
})
export class LoginModule { }
