import { Routes } from '@angular/router';
import { HomeComponent } from './home.component';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent, 
    children: [
      { path: '', redirectTo: 'inicio', pathMatch: 'full' },
      { path: 'inicio', loadChildren: () => import('./dashboard/dashboard.module').then(m => m.DashboardModule), data: { breadcrumb: 'Início' } },
      { path: 'cadastro/veiculo', loadChildren: () => import('./cadastros/cadastro-veiculo/cadastro-veiculo.module').then(m => m.CadastroVeiculoModule), data: { breadcrumb: 'Veiculo' } },
    ]
  },
];
