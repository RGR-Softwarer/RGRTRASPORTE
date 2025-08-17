import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

interface Indicador {
  titulo: string;
  valor: string | number;
  unidade: string;
  variacao: number;
  icone: string;
  cor: string;
  descricao: string;
}

interface GraficoDados {
  labels: string[];
  datasets: {
    label: string;
    data: number[];
    backgroundColor: string[];
    borderColor: string[];
    borderWidth: number;
  }[];
}

@Component({
  selector: 'app-dashboard-gerencial',
  templateUrl: './dashboard-gerencial.component.html',
  styleUrls: ['./dashboard-gerencial.component.scss'],
  standalone: true,
  imports: [CommonModule]
})
export class DashboardGerencialComponent implements OnInit {
  
  indicadores: Indicador[] = [
    {
      titulo: 'Total de Veículos',
      valor: 45,
      unidade: 'unidades',
      variacao: 12.5,
      icone: 'car',
      cor: 'primary',
      descricao: 'Frota ativa'
    },
    {
      titulo: 'Motoristas Ativos',
      valor: 28,
      unidade: 'pessoas',
      variacao: 8.3,
      icone: 'user-tie',
      cor: 'success',
      descricao: 'Em atividade'
    },
    {
      titulo: 'Clientes Ativos',
      valor: 156,
      unidade: 'clientes',
      variacao: 15.2,
      icone: 'users',
      cor: 'info',
      descricao: 'Cadastros ativos'
    },
    {
      titulo: 'Faturamento Mensal',
      valor: 'R$ 125.450',
      unidade: '',
      variacao: 22.1,
      icone: 'dollar-sign',
      cor: 'warning',
      descricao: 'Último mês'
    },
    {
      titulo: 'Viagens Realizadas',
      valor: 342,
      unidade: 'viagens',
      variacao: -5.8,
      icone: 'route',
      cor: 'danger',
      descricao: 'Este mês'
    },
    {
      titulo: 'Manutenções Pendentes',
      valor: 8,
      unidade: 'veículos',
      variacao: -25.0,
      icone: 'wrench',
      cor: 'secondary',
      descricao: 'Aguardando'
    }
  ];

  dadosVeiculosPorStatus: GraficoDados = {
    labels: ['Ativo', 'Manutenção', 'Inativo'],
    datasets: [{
      label: 'Veículos',
      data: [35, 8, 2],
      backgroundColor: ['#2ecc71', '#f39c12', '#e74c3c'],
      borderColor: ['#27ae60', '#e67e22', '#c0392b'],
      borderWidth: 1
    }]
  };

  dadosFaturamentoMensal: GraficoDados = {
    labels: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun'],
    datasets: [{
      label: 'Faturamento (R$)',
      data: [98000, 105000, 112000, 118000, 125450, 132000],
      backgroundColor: ['rgba(52, 152, 219, 0.2)'],
      borderColor: ['#3498db'],
      borderWidth: 2
    }]
  };

  dadosViagensPorMes: GraficoDados = {
    labels: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun'],
    datasets: [{
      label: 'Viagens',
      data: [280, 295, 310, 325, 342, 358],
      backgroundColor: ['rgba(46, 204, 113, 0.2)'],
      borderColor: ['#2ecc71'],
      borderWidth: 2
    }]
  };

  constructor() { }

  ngOnInit(): void {
  }

  getIconClass(icon: string): string {
    return `fas fa-${icon}`;
  }

  getVariacaoClass(variacao: number): string {
    return variacao >= 0 ? 'variacao-positiva' : 'variacao-negativa';
  }

  getVariacaoIcon(variacao: number): string {
    return variacao >= 0 ? 'fas fa-arrow-up' : 'fas fa-arrow-down';
  }

  formatarVariacao(variacao: number): string {
    return `${variacao >= 0 ? '+' : ''}${variacao.toFixed(1)}%`;
  }
}