import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const authRoutes: Routes = [
    { path: 'error', loadChildren: () => import('./error/error.module').then(m => m.ErrorModule) },
    { path: 'access', loadChildren: () => import('./access/access.module').then(m => m.AccessModule) },
    { path: 'login', loadChildren: () => import('./login/login.module').then(m => m.LoginModule) },
    { path: 'logout', loadChildren: () => import('./logout/logout.module').then(m => m.LogoutModule) },
    { path: 'forgot-password', loadChildren: () => import('./forgot-password/forgot-password.module').then(m => m.ForgotPasswordModule) },
    { path: 'register', loadChildren: () => import('./register/register.module').then(m => m.RegisterModule) },
    { path: 'profile', loadChildren: () => import('./profile/profile.module').then(m => m.ProfileModule) },
    { path: '**', redirectTo: '/notfound' }
];

@NgModule({
    imports: [RouterModule.forChild(authRoutes)],
    exports: [RouterModule]
})
export class AuthRoutingModule { }
