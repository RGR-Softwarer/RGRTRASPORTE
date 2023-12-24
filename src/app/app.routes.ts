import { Routes } from '@angular/router';
import { AutenticacaoGuard } from './services/login/autenticacao.guard';

export const routes: Routes = [
    { path: 'auth', loadChildren: () => import('./paginas/auth/autenticacao.module').then(m => m.AuthModule) },
    { path: '', loadChildren: () => import('./paginas/home.module').then(m => m.HomeModule), canActivate: [AutenticacaoGuard] },
    { path: '**', redirectTo: '' },

];