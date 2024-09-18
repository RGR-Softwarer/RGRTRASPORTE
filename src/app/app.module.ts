import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NzNotificationModule } from 'ng-zorro-antd/notification';
import { ToastService } from './services/utils/notificacao/toast.service';
import * as AllIcons from '@ant-design/icons-angular/icons';

import { NZ_ICONS } from 'ng-zorro-antd/icon';
import { NZ_I18N, pt_BR } from 'ng-zorro-antd/i18n';
import { IconDefinition } from '@ant-design/icons-angular';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { HomeModule } from './paginas/home.module';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';


const antDesignIcons = AllIcons as {
    [key: string]: IconDefinition;
  };

  const icons: IconDefinition[] = Object.keys(antDesignIcons).map(key => antDesignIcons[key])

@NgModule({ declarations: [
        AppComponent,
    ],
    bootstrap: [AppComponent], imports: [BrowserModule,
        CommonModule,
        RouterModule.forRoot(routes),
        BrowserAnimationsModule,
        NzNotificationModule,
        NzLayoutModule,
        HomeModule,
        NzBreadCrumbModule], providers: [ToastService, { provide: NZ_I18N, useValue: pt_BR }, { provide: NZ_ICONS, useValue: icons }, provideHttpClient(withInterceptorsFromDi())] })
export class AppModule { }
