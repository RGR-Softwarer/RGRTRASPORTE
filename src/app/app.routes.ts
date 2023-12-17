import { Routes } from '@angular/router';

export const routes: Routes = [
    { path: 'auth', loadChildren: () => import('./Componentes/auth/auth.module').then(m => m.AuthModule) },
];
