import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AppContextService } from '../context/app.context';

export const AuthGuard = () => {
  const appContextService = inject(AppContextService);
  const router = inject(Router);

  return (): boolean => {
    if (appContextService.usuarioLogado()) {
      return true;
    } else {
      router.navigate(['auth/login']);
      return false;
    }
  };
};
