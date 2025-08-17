import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

interface Cliente {
  id: number;
  nome: string;
  cnpj: string;
  cpf: string;
  tipo: 'PJ' | 'PF';
  email: string;
  telefone: string;
  endereco: string;
  cidade: string;
  estado: string;
  cep: string;
  status: string;
  dataCadastro: Date;
  observacoes?: string;
}

@Component({
  selector: 'app-cliente',
  templateUrl: './cliente.component.html',
  styleUrls: ['./cliente.component.scss'],
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule]
})
export class ClienteComponent implements OnInit {
  
  clientes: Cliente[] = [
    {
      id: 1,
      nome: 'Empresa ABC Ltda',
      cnpj: '12.345.678/0001-90',
      cpf: '',
      tipo: 'PJ',
      email: 'contato@empresaabc.com.br',
      telefone: '(11) 3333-3333',
      endereco: 'Rua das Flores, 123',
      cidade: 'São Paulo',
      estado: 'SP',
      cep: '01234-567',
      status: 'Ativo',
      dataCadastro: new Date('2023-01-10'),
      observacoes: 'Cliente preferencial'
    },
    {
      id: 2,
      nome: 'João da Silva',
      cnpj: '',
      cpf: '123.456.789-00',
      tipo: 'PF',
      email: 'joao.silva@email.com',
      telefone: '(11) 99999-9999',
      endereco: 'Av. Paulista, 1000',
      cidade: 'São Paulo',
      estado: 'SP',
      cep: '01310-100',
      status: 'Ativo',
      dataCadastro: new Date('2023-02-15')
    },
    {
      id: 3,
      nome: 'Transportadora XYZ',
      cnpj: '98.765.432/0001-10',
      cpf: '',
      tipo: 'PJ',
      email: 'fiscal@transportadoraxyz.com.br',
      telefone: '(11) 4444-4444',
      endereco: 'Rua do Comércio, 500',
      cidade: 'Campinas',
      estado: 'SP',
      cep: '13000-000',
      status: 'Inativo',
      dataCadastro: new Date('2023-03-20'),
      observacoes: 'Suspenso por inadimplência'
    }
  ];

  clientesFiltrados: Cliente[] = [];
  termoBusca: string = '';
  filtroStatus: string = '';
  filtroTipo: string = '';

  constructor() { }

  ngOnInit(): void {
    this.aplicarFiltros();
  }

  aplicarFiltros(): void {
    this.clientesFiltrados = this.clientes.filter(cliente => {
      const matchTermo = !this.termoBusca || 
        cliente.nome.toLowerCase().includes(this.termoBusca.toLowerCase()) ||
        cliente.cnpj.includes(this.termoBusca) ||
        cliente.cpf.includes(this.termoBusca) ||
        cliente.email.toLowerCase().includes(this.termoBusca.toLowerCase());
      
      const matchStatus = !this.filtroStatus || cliente.status === this.filtroStatus;
      const matchTipo = !this.filtroTipo || cliente.tipo === this.filtroTipo;
      
      return matchTermo && matchStatus && matchTipo;
    });
  }

  limparFiltros(): void {
    this.termoBusca = '';
    this.filtroStatus = '';
    this.filtroTipo = '';
    this.aplicarFiltros();
  }

  getStatusClass(status: string): string {
    return status === 'Ativo' ? 'status-ativo' : 'status-inativo';
  }

  getTipoClass(tipo: string): string {
    return tipo === 'PJ' ? 'tipo-pj' : 'tipo-pf';
  }

  formatarData(data: Date): string {
    return data.toLocaleDateString('pt-BR');
  }

  formatarCNPJ(cnpj: string): string {
    if (!cnpj) return '';
    return cnpj.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, '$1.$2.$3/$4-$5');
  }

  formatarCPF(cpf: string): string {
    if (!cpf) return '';
    return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
  }

  formatarCEP(cep: string): string {
    return cep.replace(/(\d{5})(\d{3})/, '$1-$2');
  }

  getDocumento(cliente: Cliente): string {
    return cliente.tipo === 'PJ' ? cliente.cnpj : cliente.cpf;
  }

  formatarDocumento(cliente: Cliente): string {
    return cliente.tipo === 'PJ' ? 
      this.formatarCNPJ(cliente.cnpj) : 
      this.formatarCPF(cliente.cpf);
  }
}