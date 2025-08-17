import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

interface RelatorioItem {
  path: string;
  breadcrumb: string;
  icon: string;
  description: string;
  color: string;
  category: string;
}

@Component({
  selector: 'app-relatorios',
  templateUrl: './relatorios.component.html',
  styleUrls: ['./relatorios.component.scss'],
  standalone: true,
  imports: [CommonModule, RouterModule]
})
export class RelatoriosComponent implements OnInit {
  
  relatorios: RelatorioItem[] = [
    // Relatórios de Frota
    {
      path: 'frota/veiculos',
      breadcrumb: 'Relatório de Veículos',
      icon: 'car',
      description: 'Relatório completo da frota de veículos',
      color: 'primary',
      category: 'Frota'
    },
    {
      path: 'frota/manutencoes',
      breadcrumb: 'Relatório de Manutenções',
      icon: 'wrench',
      description: 'Histórico de manutenções por veículo',
      color: 'warning',
      category: 'Frota'
    },
    {
      path: 'frota/combustivel',
      breadcrumb: 'Relatório de Combustível',
      icon: 'gas-pump',
      description: 'Consumo de combustível por veículo',
      color: 'danger',
      category: 'Frota'
    },
    {
      path: 'frota/motoristas',
      breadcrumb: 'Relatório de Motoristas',
      icon: 'user-tie',
      description: 'Relatório de motoristas e suas atividades',
      color: 'info',
      category: 'Frota'
    },
    
    // Relatórios Financeiros
    {
      path: 'financeiro/custos',
      breadcrumb: 'Relatório de Custos',
      icon: 'dollar-sign',
      description: 'Análise de custos operacionais',
      color: 'success',
      category: 'Financeiro'
    },
    {
      path: 'financeiro/faturamento',
      breadcrumb: 'Relatório de Faturamento',
      icon: 'chart-line',
      description: 'Análise de faturamento e receitas',
      color: 'primary',
      category: 'Financeiro'
    },
    
    // Relatórios Operacionais
    {
      path: 'operacional/viagens',
      breadcrumb: 'Relatório de Viagens',
      icon: 'route',
      description: 'Relatório de viagens realizadas',
      color: 'info',
      category: 'Operacional'
    },
    {
      path: 'operacional/clientes',
      breadcrumb: 'Relatório de Clientes',
      icon: 'users',
      description: 'Análise de clientes e serviços',
      color: 'success',
      category: 'Operacional'
    },
    
    // Relatórios Gerenciais
    {
      path: 'gerencial/dashboard',
      breadcrumb: 'Dashboard Gerencial',
      icon: 'tachometer-alt',
      description: 'Visão geral dos indicadores principais',
      color: 'secondary',
      category: 'Gerencial'
    },
    {
      path: 'gerencial/performance',
      breadcrumb: 'Relatório de Performance',
      icon: 'chart-bar',
      description: 'Indicadores de performance da empresa',
      color: 'warning',
      category: 'Gerencial'
    }
  ];

  categorias: string[] = [];

  constructor() { }

  ngOnInit(): void {
    this.categorias = [...new Set(this.relatorios.map(r => r.category))];
  }

  getIconClass(icon: string): string {
    return `fas fa-${icon}`;
  }

  getRelatoriosPorCategoria(categoria: string): RelatorioItem[] {
    return this.relatorios.filter(r => r.category === categoria);
  }
}