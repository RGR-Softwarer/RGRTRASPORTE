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
            // Veículos
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
            
            // Passageiros
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
            
            // Motoristas
            { 
                path: 'frota/motorista', 
                loadComponent: () => import('./paginas/frota/motorista/motorista.component').then(c => c.MotoristaComponent) 
            },
            { 
                path: 'frota/motorista/adicionar', 
                loadComponent: () => import('./paginas/frota/motorista/motorista-formulario/motorista-formulario.component').then(c => c.MotoristaFormularioComponent) 
            },
            { 
                path: 'frota/motorista/editar/:id', 
                loadComponent: () => import('./paginas/frota/motorista/motorista-formulario/motorista-formulario.component').then(c => c.MotoristaFormularioComponent) 
            },
            
            // Viagens
            { 
                path: 'frota/viagem', 
                loadComponent: () => import('./paginas/frota/viagem/viagem.component').then(c => c.ViagemComponent) 
            },
            { 
                path: 'frota/viagem/adicionar', 
                loadComponent: () => import('./paginas/frota/viagem/viagem-formulario/viagem-formulario.component').then(c => c.ViagemFormularioComponent) 
            },
            { 
                path: 'frota/viagem/editar/:id', 
                loadComponent: () => import('./paginas/frota/viagem/viagem-formulario/viagem-formulario.component').then(c => c.ViagemFormularioComponent) 
            },
            
            // Localidades
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
            
            // Modelos Veiculares
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
            
            // Gatilhos de Viagem
            { 
                path: 'frota/gatilho-viagem', 
                loadComponent: () => import('./paginas/frota/gatilho-viagem/gatilho-viagem.component').then(c => c.GatilhoViagemComponent) 
            },
            { 
                path: 'frota/gatilho-viagem/adicionar', 
                loadComponent: () => import('./paginas/frota/gatilho-viagem/gatilho-viagem-formulario/gatilho-viagem-formulario.component').then(c => c.GatilhoViagemFormularioComponent) 
            },
            { 
                path: 'frota/gatilho-viagem/editar/:id', 
                loadComponent: () => import('./paginas/frota/gatilho-viagem/gatilho-viagem-formulario/gatilho-viagem-formulario.component').then(c => c.GatilhoViagemFormularioComponent) 
            },
            
            // Dashboard
            { 
                path: 'inicio', 
                loadComponent: () => import('./paginas/dashboard/dashboard.component').then(c => c.DashboardComponent) 
            },
            { path: '', redirectTo: 'inicio', pathMatch: 'full' }
        ]
    },
    { path: '**', redirectTo: '' }
];
