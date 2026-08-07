import { Routes } from '@angular/router';

export const AUTH_ROUTES: Routes = [
    { 
        path: 'error', 
        loadChildren: () => import('./error/error.routes').then(r => r.ERROR_ROUTES) 
    },
    { 
        path: 'access', 
        loadChildren: () => import('./access/access.routes').then(r => r.ACCESS_ROUTES) 
    },
    { 
        path: 'login', 
        loadChildren: () => import('./login/login.routes').then(r => r.LOGIN_ROUTES) 
    },
    { 
        path: 'logout', 
        loadComponent: () => import('./logout/logout.component').then(c => c.LogoutComponent) 
    },
    { 
        path: 'forgot-password', 
        loadComponent: () => import('./forgot-password/forgot-password.component').then(c => c.ForgotPasswordComponent) 
    },
    { 
        path: 'register', 
        loadChildren: () => import('./register/register.routes').then(r => r.REGISTER_ROUTES) 
    },
    { 
        path: 'profile', 
        loadComponent: () => import('./profile/profile.component').then(c => c.ProfileComponent) 
    },
    { path: '**', redirectTo: '/notfound' }
]; 