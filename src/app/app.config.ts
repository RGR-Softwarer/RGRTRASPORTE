import { ApplicationConfig, LOCALE_ID } from '@angular/core';
import { provideRouter } from '@angular/router';
import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';
import { provideAnimations } from '@angular/platform-browser/animations';

import { routes } from './app.routes';

// Registrar dados de localização para português brasileiro
registerLocaleData(localePt);

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideAnimations(),
    { provide: LOCALE_ID, useValue: 'pt-BR' }
  ]
};
