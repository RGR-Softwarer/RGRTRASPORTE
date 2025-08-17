import { Routes } from '@angular/router';
// import { AutenticacaoGuard } from './services/login/autenticacao.guard';

export const routes: Routes = [
    { 
        path: 'auth', 
        loadChildren: () => import('./paginas/auth/auth.routes').then(r => r.AUTH_ROUTES) 
    },
    { 
        path: '', 
        loadComponent: () => import('./paginas/home/home.component').then(c => c.HomeComponent), 
        // canActivate: [AutenticacaoGuard],
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
                path: 'frota/passageiro', 
                loadComponent: () => import('./paginas/frota/passageiro/passageiro.component').then(c => c.PassageiroComponent) 
            },
            { 
                path: 'frota/passageiro/adicionar', 
                loadComponent: () => import('./paginas/frota/passageiro/passageiro-formulario/passageiro-formulario.component').then(c => c.PassageiroFormularioComponent) 
            },
            { 
                path: 'frota/passageiro/editar/:id', 
                loadComponent: () => import('./paginas/frota/passageiro/passageiro-formulario/passageiro-formulario.component').then(c => c.PassageiroFormularioComponent) 
            },
            { 
                path: 'frota/viagem', 
                loadComponent: () => import('./paginas/frota/viagem/viagem.component').then(c => c.ViagemComponent) 
            },
            { 
                path: 'frota/localidade', 
                loadComponent: () => import('./paginas/frota/localidade/localidade.component').then(c => c.LocalidadeComponent) 
            },
            { 
                path: 'frota/localidade/adicionar', 
                loadComponent: () => import('./paginas/frota/localidade/localidade-formulario/localidade-formulario.component').then(c => c.LocalidadeFormularioComponent) 
            },
            { 
                path: 'frota/localidade/editar/:id', 
                loadComponent: () => import('./paginas/frota/localidade/localidade-formulario/localidade-formulario.component').then(c => c.LocalidadeFormularioComponent) 
            },
            { 
                path: 'frota/modelo-veicular', 
                loadComponent: () => import('./paginas/frota/modelo-veicular/modelo-veicular.component').then(c => c.ModeloVeicularComponent) 
            },
            { 
                path: 'frota/modelo-veicular/adicionar', 
                loadComponent: () => import('./paginas/frota/modelo-veicular/modelo-veicular-formulario/modelo-veicular-formulario.component').then(c => c.ModeloVeicularFormularioComponent) 
            },
            { 
                path: 'frota/modelo-veicular/editar/:id', 
                loadComponent: () => import('./paginas/frota/modelo-veicular/modelo-veicular-formulario/modelo-veicular-formulario.component').then(c => c.ModeloVeicularFormularioComponent) 
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