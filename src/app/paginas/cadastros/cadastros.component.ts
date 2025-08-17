import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

interface CadastroItem {
  path: string;
  breadcrumb: string;
  icon: string;
  description: string;
  color: string;
}

@Component({
  selector: 'app-cadastros',
  templateUrl: './cadastros.component.html',
  styleUrls: ['./cadastros.component.scss'],
  standalone: true,
  imports: [CommonModule, RouterModule]
})
export class CadastrosComponent implements OnInit {
  
  cadastros: CadastroItem[] = [
    {
      path: 'motoristas',
      breadcrumb: 'Motoristas',
      icon: 'user-tie',
      description: 'Gerenciar motoristas da frota',
      color: 'primary'
    },
    {
      path: 'clientes',
      breadcrumb: 'Clientes',
      icon: 'users',
      description: 'Cadastro de clientes',
      color: 'success'
    },
    {
      path: 'fornecedores',
      breadcrumb: 'Fornecedores',
      icon: 'building',
      description: 'Cadastro de fornecedores',
      color: 'info'
    },
    {
      path: 'manutencoes',
      breadcrumb: 'Manutenções',
      icon: 'wrench',
      description: 'Tipos de manutenção',
      color: 'warning'
    },
    {
      path: 'combustiveis',
      breadcrumb: 'Combustíveis',
      icon: 'gas-pump',
      description: 'Tipos de combustível',
      color: 'danger'
    },
    {
      path: 'configuracoes',
      breadcrumb: 'Configurações',
      icon: 'cog',
      description: 'Configurações do sistema',
      color: 'secondary'
    }
  ];

  constructor() { }

  ngOnInit(): void {
  }

  getIconClass(icon: string): string {
    return `fas fa-${icon}`;
  }
}