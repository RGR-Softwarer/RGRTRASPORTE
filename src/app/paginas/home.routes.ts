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
          },
          {
            path: 'veiculo/editar',
            loadChildren: () => import('./cadastros/veiculo/veiculo-formulario/veiculo-formulario.module').then(m => m.VeiculoFormularioModule),
            data: { breadcrumb: 'Editar', oculta: true }
          },
          {
            path: 'modeloveicular',
            loadChildren: () => import('./cadastros/veiculo/modelo-veicular/modelo-veicular.module').then(m => m.CadastroModeloVeicularModule),
            data: { breadcrumb: 'Modelo Veícular', icon: 'car' }  // Icone de carro para Veículo
          },
          {
            path: 'modeloveicular/adicionar',
            loadChildren: () => import('./cadastros/veiculo/modelo-veicular/modelo-veicular-formulario/modelo-veicular-formulario.module').then(m => m.ModeloVeicularFormularioModule),
            data: { breadcrumb: 'Adicionar', oculta: true }
          },
          {
            path: 'modeloveicular/editar',
            loadChildren: () => import('./cadastros/veiculo/modelo-veicular/modelo-veicular-formulario/modelo-veicular-formulario.module').then(m => m.ModeloVeicularFormularioModule),
            data: { breadcrumb: 'Editar', oculta: true }
          }
        ]
      },      
    ]
  },
];
