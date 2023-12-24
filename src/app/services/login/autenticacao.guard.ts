import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AppContextService } from '../context/app.context';

@Injectable({
  providedIn: 'root'
})
export class AutenticacaoGuard implements CanActivate {
  constructor(private appContextService: AppContextService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (this.appContextService.obterUsuarioLogado()) {
      return true;
    } else {
      return this.router.createUrlTree(['/auth/login']);
    }
  }
}
