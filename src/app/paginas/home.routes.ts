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
            path: 'veiculo/adicionar',
            loadChildren: () => import('./cadastros/veiculo/veiculo-formulario/veiculo-formulario.module').then(m => m.VeiculoFormularioModule),
            data: { breadcrumb: 'Adicionar Veículo', icon: 'plus', oculta: true }
          },
          {
            path: 'veiculo/editar/:id',
            loadChildren: () => import('./cadastros/veiculo/veiculo-formulario/veiculo-formulario.module').then(m => m.VeiculoFormularioModule),
            data: { breadcrumb: 'Editar Veículo', icon: 'edit', oculta: true }
          },
          {
            path: 'veiculo',
            loadChildren: () => import('./cadastros/veiculo/veiculo.module').then(m => m.VeiculoModule),
            data: { breadcrumb: 'Veículo', icon: 'car' }  // Icone de carro para Veículo
          },          
        ]
      },      
    ]
  },
];
