import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AutenticacaoRotaModule } from './autenticacao-rota.module';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { PasswordModule } from 'primeng/password';

@NgModule({
    imports: [
        CommonModule,
        AutenticacaoRotaModule,
        FormsModule,
        InputTextModule,
        ButtonModule,
        CheckboxModule,
        PasswordModule
    ]
})
export class AuthModule { }
