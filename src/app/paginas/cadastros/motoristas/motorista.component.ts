import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

interface Motorista {
  id: number;
  nome: string;
  cpf: string;
  cnh: string;
  categoriaCnh: string;
  dataVencimentoCnh: Date;
  telefone: string;
  email: string;
  status: string;
  dataCadastro: Date;
}

@Component({
  selector: 'app-motorista',
  templateUrl: './motorista.component.html',
  styleUrls: ['./motorista.component.scss'],
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule]
})
export class MotoristaComponent implements OnInit {
  
  motoristas: Motorista[] = [
    {
      id: 1,
      nome: 'João Silva',
      cpf: '123.456.789-00',
      cnh: '12345678901',
      categoriaCnh: 'E',
      dataVencimentoCnh: new Date('2025-12-31'),
      telefone: '(11) 99999-9999',
      email: 'joao.silva@email.com',
      status: 'Ativo',
      dataCadastro: new Date('2023-01-15')
    },
    {
      id: 2,
      nome: 'Maria Santos',
      cpf: '987.654.321-00',
      cnh: '98765432109',
      categoriaCnh: 'D',
      dataVencimentoCnh: new Date('2024-08-15'),
      telefone: '(11) 88888-8888',
      email: 'maria.santos@email.com',
      status: 'Ativo',
      dataCadastro: new Date('2023-02-20')
    },
    {
      id: 3,
      nome: 'Pedro Oliveira',
      cpf: '456.789.123-00',
      cnh: '45678912345',
      categoriaCnh: 'C',
      dataVencimentoCnh: new Date('2025-03-10'),
      telefone: '(11) 77777-7777',
      email: 'pedro.oliveira@email.com',
      status: 'Inativo',
      dataCadastro: new Date('2023-03-05')
    }
  ];

  motoristasFiltrados: Motorista[] = [];
  termoBusca: string = '';
  filtroStatus: string = '';

  constructor() { }

  ngOnInit(): void {
    this.aplicarFiltros();
  }

  aplicarFiltros(): void {
    this.motoristasFiltrados = this.motoristas.filter(motorista => {
      const matchTermo = !this.termoBusca || 
        motorista.nome.toLowerCase().includes(this.termoBusca.toLowerCase()) ||
        motorista.cpf.includes(this.termoBusca) ||
        motorista.cnh.includes(this.termoBusca);
      
      const matchStatus = !this.filtroStatus || motorista.status === this.filtroStatus;
      
      return matchTermo && matchStatus;
    });
  }

  limparFiltros(): void {
    this.termoBusca = '';
    this.filtroStatus = '';
    this.aplicarFiltros();
  }

  getStatusClass(status: string): string {
    return status === 'Ativo' ? 'status-ativo' : 'status-inativo';
  }

  formatarData(data: Date): string {
    return data.toLocaleDateString('pt-BR');
  }

  formatarCPF(cpf: string): string {
    return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
  }

  verificarVencimentoCNH(dataVencimento: Date): string {
    const hoje = new Date();
    const vencimento = new Date(dataVencimento);
    const diasParaVencimento = Math.ceil((vencimento.getTime() - hoje.getTime()) / (1000 * 60 * 60 * 24));
    
    if (diasParaVencimento < 0) {
      return 'vencido';
    } else if (diasParaVencimento <= 30) {
      return 'proximo-vencimento';
    } else {
      return 'valido';
    }
  }

  getVencimentoClass(dataVencimento: Date): string {
    const status = this.verificarVencimentoCNH(dataVencimento);
    return `vencimento-${status}`;
  }
}