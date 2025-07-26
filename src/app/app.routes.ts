import { Routes } from '@angular/router';
import { AutenticacaoGuard } from './services/login/autenticacao.guard';

export const routes: Routes = [
    { 
        path: 'auth', 
        loadChildren: () => import('./paginas/auth/auth.routes').then(r => r.AUTH_ROUTES) 
    },
    { 
        path: '', 
        loadComponent: () => import('./paginas/home/home.component').then(c => c.HomeComponent), 
        canActivate: [AutenticacaoGuard],
        children: [
            { 
                path: 'frota/veiculo', 
                loadComponent: () => import('./paginas/cadastros/veiculo/veiculo.component').then(c => c.VeiculoComponent) 
            },
            { 
                path: 'frota/veiculo/adicionar', 
                loadComponent: () => import('./paginas/cadastros/veiculo/veiculo-formulario/veiculo-formulario.component').then(c => c.VeiculoFormularioComponent) 
            },
            { 
                path: 'frota/veiculo/editar/:id', 
                loadComponent: () => import('./paginas/cadastros/veiculo/veiculo-formulario/veiculo-formulario.component').then(c => c.VeiculoFormularioComponent) 
            },
            { 
                path: 'inicio', 
                loadComponent: () => import('./paginas/dashboard/dashboard.component').then(c => c.DashboardComponent) 
            },
            { path: '', redirectTo: 'inicio', pathMatch: 'full' }
        ]
    },
    { path: '**', redirectTo: '' }
];