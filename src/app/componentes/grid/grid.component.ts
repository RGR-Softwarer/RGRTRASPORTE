import { Component, OnInit, Input } from '@angular/core';
import { FormControl, FormGroup, NonNullableFormBuilder } from '@angular/forms';
import { NzTableLayout, NzTablePaginationPosition, NzTablePaginationType, NzTableSize } from 'ng-zorro-antd/table';
import { ConfiguracaoGrid, RolagemTabela } from '../../dominio/interface/configuracao-grid';

interface ItemData {
  nome: string;
  idade: number | string;
  endereco: string;
  marcado: boolean;
  expandido: boolean;
  descricao: string;
  desativado?: boolean;
}

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrl: './grid.component.scss'
})
export class GridComponent implements OnInit {
  @Input() formularioConfiguracao: FormGroup<{ [K in keyof ConfiguracaoGrid]: FormControl<ConfiguracaoGrid[K]> }>;

  //formularioConfiguracao: FormGroup<{ [K in keyof ConfiguracaoGrid]: FormControl<ConfiguracaoGrid[K]> }>;
  listaDados: readonly ItemData[] = [];
  dadosExibidos: readonly ItemData[] = [];
  todosMarcados = false;
  indeterminado = false;
  colunaFixa = false;
  rolagemX: string | null = null;
  rolagemY: string | null = null;
  valorConfiguracao: ConfiguracaoGrid;
  listOfSwitch = [
    { nome: 'Com Borda', formControlName: 'comBorda' },
    { nome: 'Carregando', formControlName: 'carregando' },
    { nome: 'Paginação', formControlName: 'paginacao' },
    { nome: 'Alterador de Tamanho', formControlName: 'alteradorTamanho' },
    { nome: 'Título', formControlName: 'titulo' },
    { nome: 'Cabeçalho da Coluna', formControlName: 'cabecalho' },
    { nome: 'Rodapé', formControlName: 'rodape' },
    { nome: 'Expansível', formControlName: 'expansivel' },
    { nome: 'Caixa de Seleção', formControlName: 'caixaSelecao' },
    { nome: 'Cabeçalho Fixo', formControlName: 'cabecalhoFixo' },
    { nome: 'Sem Resultado', formControlName: 'semResultado' },
    { nome: 'Elipse', formControlName: 'elipse' },
    { nome: 'Paginação Simples', formControlName: 'simples' }
  ];
  listOfRadio = [
    {
      nome: 'Tamanho',
      formControlName: 'tamanho',
      listOfOption: [
        { valor: 'default', rotulo: 'Padrão' },
        { valor: 'middle', rotulo: 'Médio' },
        { valor: 'small', rotulo: 'Pequeno' }
      ]
    },
    {
      nome: 'Rolagem da Tabela',
      formControlName: 'rolagemTabela',
      listOfOption: [
        { valor: 'unset', rotulo: 'Não definido' },
        { valor: 'scroll', rotulo: 'Rolagem' },
        { valor: 'fixed', rotulo: 'Fixo' }
      ]
    },
    {
      nome: 'Layout da Tabela',
      formControlName: 'layoutTabela',
      listOfOption: [
        { valor: 'auto', rotulo: 'Automático' },
        { valor: 'fixed', rotulo: 'Fixo' }
      ]
    },
    {
      nome: 'Posição da Paginação',
      formControlName: 'posicao',
      listOfOption: [
        { valor: 'top', rotulo: 'Topo' },
        { valor: 'bottom', rotulo: 'Inferior' },
        { valor: 'both', rotulo: 'Ambos' }
      ]
    },
    {
      nome: 'Tipo de Paginação',
      formControlName: 'tipoPaginacao',
      listOfOption: [
        { valor: 'default', rotulo: 'Padrão' },
        { valor: 'small', rotulo: 'Pequeno' }
      ]
    }
  ];

  mudancaDadosPaginaAtual($event: readonly ItemData[]): void {
    this.dadosExibidos = $event;
    this.atualizarStatus();
  }

  atualizarStatus(): void {
    const dadosValidos = this.dadosExibidos.filter(valor => !valor.desativado);
    const todosMarcados = dadosValidos.length > 0 && dadosValidos.every(valor => valor.marcado === true);
    const todosDesmarcados = dadosValidos.every(valor => !valor.marcado);
    this.todosMarcados = todosMarcados;
    this.indeterminado = !todosMarcados && !todosDesmarcados;
  }

  marcarTodos(valor: boolean): void {
    this.dadosExibidos.forEach(dado => {
      if (!dado.desativado) {
        dado.marcado = valor;
      }
    });
    this.atualizarStatus();
  }

  gerarDados(): readonly ItemData[] {
    const dados = [];
    for (let i = 1; i <= 100; i++) {
      dados.push({
        nome: 'John Brown',
        idade: `${i}2`,
        endereco: `Nova York No. ${i} Lake Park`,
        descricao: `Meu nome é John Brown, tenho ${i}2 anos, morando em Nova York No. ${i} Lake Park.`,
        marcado: false,
        expandido: false
      });
    }
    return dados;
  }

  constructor(private formBuilder: NonNullableFormBuilder) {
    this.formularioConfiguracao = this.formBuilder.group({
      comBorda: [false],
      carregando: [false],
      paginacao: [true],
      alteradorTamanho: [false],
      titulo: [true],
      cabecalho: [true],
      rodape: [true],
      expansivel: [true],
      caixaSelecao: [true],
      cabecalhoFixo: [false],
      semResultado: [false],
      elipse: [false],
      simples: [false],
      mostrarOpcoes: [false],
      tamanho: 'small' as NzTableSize,
      tipoPaginacao: 'default' as NzTablePaginationType,
      rolagemTabela: 'unset' as RolagemTabela,
      layoutTabela: 'auto' as NzTableLayout,
      posicao: 'bottom' as NzTablePaginationPosition
    });

    this.valorConfiguracao = this.formularioConfiguracao.value as ConfiguracaoGrid;
  }

  ngOnInit(): void {
    this.formularioConfiguracao.valueChanges.subscribe(valor => {
      this.valorConfiguracao = valor as ConfiguracaoGrid;
    });
    this.formularioConfiguracao.controls.rolagemTabela.valueChanges.subscribe(rolagem => {
      this.colunaFixa = rolagem === 'fixed';
      this.rolagemX = rolagem === 'scroll' || rolagem === 'fixed' ? '100vw' : null;
    });
    this.formularioConfiguracao.controls.cabecalhoFixo.valueChanges.subscribe(fixo => {
      this.rolagemY = fixo ? '240px' : null;
    });
    this.formularioConfiguracao.controls.semResultado.valueChanges.subscribe(vazio => {
      if (vazio) {
        this.listaDados = [];
      } else {
        this.listaDados = this.gerarDados();
      }
    });
    this.listaDados = this.gerarDados();
  }
}
