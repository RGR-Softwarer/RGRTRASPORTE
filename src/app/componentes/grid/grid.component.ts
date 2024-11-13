import { Component, OnInit, Input } from '@angular/core';
import { FormControl, FormGroup, NonNullableFormBuilder } from '@angular/forms';
import { ConfiguracaoGrid, RolagemTabela } from '../../dominio/interface/grid/configuracao-grid';
import { NzTableSize, NzTablePaginationType, NzTableLayout, NzTablePaginationPosition } from 'ng-zorro-antd/table';
import { Router } from '@angular/router';
import { ApiService } from '../../services/http/api.service';
import { ToastService } from '../../services/utils/notificacao/toast.service';
import { firstValueFrom } from 'rxjs';
import { Veiculo } from '../../dominio/entidade/veiculo';
import { FormCampoConstrutor, FormCamposMetadata } from '../../services/decorator/formulario-decorator';
import { Action } from '../../dominio/interface/grid/action-grid';

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.scss']
})
export class GridComponent implements OnInit {
  @Input() formularioConfiguracao!: FormGroup<{ [K in keyof ConfiguracaoGrid]: FormControl<ConfiguracaoGrid[K]> }>;
  @Input() buscarTodosUrl: string = '';
  @Input() adicionarUrl: string = '';
  @Input() acoes?: Action[] = [{ label: 'Editar', acao: this.editar.bind(this) }];

  gridColumns: FormCamposMetadata[] = []; 
  listaDados: any[] = [];
  dadosExibidos: readonly any[] = [];
  todosMarcados = false;
  indeterminado = false;
  colunaFixa = false;
  rolagemX: string | null = null;
  rolagemY: string | null = null;
  valorConfiguracao!: ConfiguracaoGrid;
  dadosEntrada: any[] = [];

  formularioConfiguracaoPadrao: FormGroup<{ [K in keyof ConfiguracaoGrid]: FormControl<ConfiguracaoGrid[K]> }>;

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

  constructor(
    private formBuilder: NonNullableFormBuilder, 
    private router: Router, 
    private apiService: ApiService, 
    private toastService: ToastService
  ) {
    // Inicializa as configurações padrão da grid
    this.formularioConfiguracaoPadrao = this.formBuilder.group({
      comBorda: [true],
      carregando: [false],
      paginacao: [true],
      alteradorTamanho: [false],
      titulo: [false],
      cabecalho: [true],
      rodape: [false],
      expansivel: [false],
      caixaSelecao: [false],
      cabecalhoFixo: [false],
      semResultado: [false],
      elipse: [false],
      simples: [false],
      mostrarOpcoes: [false],
      tamanho: 'small' as NzTableSize,
      tipoPaginacao: 'default' as NzTablePaginationType,
      rolagemTabela: 'unset' as RolagemTabela,
      layoutTabela: 'auto' as NzTableLayout,
      posicao: 'bottom' as NzTablePaginationPosition,
      tituloTabela: 'Título da Tabela',
      rodapeTabela: 'Rodapé da Tabela',
      adicionar: [false],
      action: [true]
    });
  }

  async ngOnInit(): Promise<void> {
    // Usa as configurações padrão se nenhuma configuração for passada
    if (!this.formularioConfiguracao) {
      this.formularioConfiguracao = this.formularioConfiguracaoPadrao;
    }

    if (this.adicionarUrl !== '') {
      this.formularioConfiguracao.controls.adicionar.setValue(true);
    }

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
      this.listaDados = vazio ? [] : this.dadosEntrada;
    });

    this.valorConfiguracao = this.formularioConfiguracao.value as ConfiguracaoGrid;

    // Carrega dados da API
    this.dadosEntrada = await this.obterTodos();
    this.listaDados = this.dadosEntrada;

    // Gera colunas baseadas nos metadados da entidade
    const objetoConstructor = new Veiculo().constructor as FormCampoConstrutor;
    this.gridColumns = objetoConstructor.formFields ?? [];
  }

  mudancaDadosPaginaAtual($event: readonly any[]): void {
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

  editar(item: any): void {
    this.router.navigate([this.adicionarUrl+ 'editar'], { 
      state: { 
        isEditMode: true, 
        objeto: JSON.stringify(item) // Converte objeto para string JSON para evitar problemas de serialização
      }
    });
  }  

  adicionar(): void {
    this.router.navigate([this.adicionarUrl+'adicionar'], { 
      queryParams: { 
        isEditMode: false // Define que não está no modo de edição
      }
    });
  }

  async obterTodos(): Promise<any[]> {
    try {
      const data = await firstValueFrom(this.apiService.get(this.buscarTodosUrl));
      if (data.sucesso) {
        return data.dados;
      } else {
        this.toastService.exibirMensagemErro('Erro', 'Erro ao obter dados');
        return [];
      }
    } catch (error) {
      this.toastService.exibirMensagemErro('Erro', 'Erro ao obter dados');
      return [];
    }
  }
  
}
