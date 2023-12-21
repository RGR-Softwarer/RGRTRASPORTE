import { Routes } from '@angular/router';
import { AuthGuard } from './services/login/autenticacao.guard';

export const routes: Routes = [
    { path: 'auth', loadChildren: () => import('./componentes/auth/autenticacao.module').then(m => m.AuthModule) },
    { path: '', loadChildren: () => import('./componentes/dashboard/dashboard/dashboard.module').then(m => m.DashboardModule), canActivate: [AuthGuard] },
];