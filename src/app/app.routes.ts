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
                loadComponent: () => import('./paginas/frota/veiculo/veiculo.component').then(c => c.VeiculoComponent) 
            },
            { 
                path: 'frota/veiculo/adicionar', 
                loadComponent: () => import('./paginas/frota/veiculo/veiculo-formulario/veiculo-formulario.component').then(c => c.VeiculoFormularioComponent) 
            },
            { 
                path: 'frota/veiculo/editar/:id', 
                loadComponent: () => import('./paginas/frota/veiculo/veiculo-formulario/veiculo-formulario.component').then(c => c.VeiculoFormularioComponent) 
            },
            { 
                path: 'cadastros', 
                loadComponent: () => import('./paginas/cadastros/cadastros.component').then(c => c.CadastrosComponent) 
            },
            { 
                path: 'cadastros/motoristas', 
                loadComponent: () => import('./paginas/cadastros/motoristas/motorista.component').then(c => c.MotoristaComponent) 
            },
            { 
                path: 'cadastros/clientes', 
                loadComponent: () => import('./paginas/cadastros/clientes/cliente.component').then(c => c.ClienteComponent) 
            },
            { 
                path: 'relatorios', 
                loadComponent: () => import('./paginas/relatorios/relatorios.component').then(c => c.RelatoriosComponent) 
            },
            { 
                path: 'relatorios/gerencial/dashboard', 
                loadComponent: () => import('./paginas/relatorios/gerencial/dashboard-gerencial.component').then(c => c.DashboardGerencialComponent) 
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