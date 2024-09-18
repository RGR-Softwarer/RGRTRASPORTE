import { Routes } from '@angular/router';
import { HomeComponent } from './home.component';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    children: [
      { 
        path: '', 
        redirectTo: 'inicio', 
        pathMatch: 'full' 
      },
      { 
        path: 'inicio', 
        loadChildren: () => import('./dashboard/dashboard.module').then(m => m.DashboardModule), 
        data: { breadcrumb: 'Início', icon: 'home' }  // Icone de casa
      },
      {
        path: 'frota',
        data: { breadcrumb: 'Frota', icon: 'car' },  // Icone de carro para Frota
        children: [
          { 
            path: '', 
            redirectTo: '/inicio',  
            pathMatch: 'full' 
          },
          {
            path: 'veiculo',
            loadChildren: () => import('./cadastros/veiculo/veiculo.module').then(m => m.CadastroVeiculoModule),
            data: { breadcrumb: 'Veículo', icon: 'car' }  // Icone de carro para Veículo
          },
          {
            path: 'veiculo/adicionar',
            loadChildren: () => import('./cadastros/veiculo/veiculo-formulario/veiculo-formulario.module').then(m => m.VeiculoFormularioModule),
            data: { breadcrumb: 'Adicionar', oculta: true }
          }
        ]
      },
      { 
        path: 'cadastro/pacote', 
        loadChildren: () => import('./cadastros/pacote/pacote.module').then(m => m.CadastroPacoteModule), 
        data: { breadcrumb: 'Pacote', icon: 'book' }  // Icone de pacote
      },
      { 
        path: 'cadastro/pacote/adicionar', 
        loadChildren: () => import('./cadastros/pacote/pacote-formulario/pacote-formulario.module').then(m => m.PacoteFormularioModule), 
        data: { breadcrumb: 'Adicionar', oculta: true } 
      },
      { 
        path: 'cadastro/viagem', 
        loadChildren: () => import('./cadastros/viagem/viagem.module').then(m => m.CadastroViagemModule), 
        data: { breadcrumb: 'Viagem', icon: 'compass' }  // Icone de bússola para Viagem
      },
      { 
        path: 'cadastro/viagem/adicionar', 
        loadChildren: () => import('./cadastros/viagem/viagem-formulario/viagem-formulario.module').then(m => m.ViagemFormularioModule), 
        data: { breadcrumb: 'Adicionar', oculta: true } 
      }
    ]
  },
];
