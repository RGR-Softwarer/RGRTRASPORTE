import { ToastModule } from 'primeng/toast';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { routes } from './app.routes';
import { HttpClientModule } from '@angular/common/http';
import { MessageService } from 'primeng/api';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MenuModule } from './componentes/dashboard/menu/menu.module';

@NgModule({
    imports: [
        BrowserModule,
        CommonModule,
        RouterModule.forRoot(routes),
        HttpClientModule,
        ToastModule,
        BrowserAnimationsModule,
        MenuModule
    ],
    declarations: [
        AppComponent,
    ],    
    bootstrap: [AppComponent],
    providers: [MessageService],
})
export class AppModule { }
