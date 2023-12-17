import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';


import { AppComponent } from './app.component';

import { routes } from './app.routes';
import { HttpClientModule } from '@angular/common/http';
import { CheckboxModule } from 'primeng/checkbox';


@NgModule({
    imports: [
        BrowserModule,
        CommonModule,
        RouterModule.forRoot(routes),
        FormsModule,
        HttpClientModule,
        CheckboxModule
    ],
    declarations: [
        AppComponent,       
    ],    
    bootstrap: [AppComponent],
})
export class AppModule { }
