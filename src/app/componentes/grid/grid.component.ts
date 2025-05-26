import { Component, OnInit, Input } from '@angular/core';
import { FormControl, FormGroup, NonNullableFormBuilder } from '@angular/forms';
import { ConfiguracaoGrid, RolagemTabela } from '../../dominio/interface/grid/configuracao-grid';
import { NzTableSize, NzTablePaginationType, NzTableLayout, NzTablePaginationPosition, NzTableSortOrder } from 'ng-zorro-antd/table';
import { Router } from '@angular/router';
import { ApiService } from '../../services/http/api.service';
import { ToastService } from '../../services/utils/notificacao/toast.service';
import { firstValueFrom } from 'rxjs';
import { Veiculo } from '../../dominio/entidade/veiculo';
import { FormCampoConstrutor, FormCamposMetadata } from '../../services/decorator/formulario-decorator';
import { Action } from '../../dominio/interface/grid/action-grid';
import { NzCheckBoxOptionInterface } from 'ng-zorro-antd/checkbox';
import { FiltroGrid, ParametrosBusca } from '../../dominio/interface/grid/filtros-grid';
import { ResponseGrid } from '../../dominio/interface/grid/response-grid';
import { Subject, debounceTime } from 'rxjs';
import { ApiResponse } from '../../dominio/interface/grid/api-response';

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
  @Input() entidade: any;
  @Input() identificador: string = '';

  gridColumns: FormCamposMetadata[] = []; 
  listaDados: any[] = [];
  dadosExibidos: readonly any[] = [];
  todosMarcados = false;
  indeterminado = false;
  colunaFixa = false;
  rolagemX: string | null = null;
  rolagemY: string | null = null;
  valorConfiguracao: ConfiguracaoGrid = {
    comBorda: true,
    carregando: false,
    paginacao: true,
    alteradorTamanho: true,
    titulo: false,
    cabecalho: true,
    rodape: false,
    expansivel: false,
    caixaSelecao: false,
    cabecalhoFixo: false,
    semResultado: false,
    elipse: false,
    simples: false,
    mostrarOpcoes: false,
    tamanho: 'small',
    tipoPaginacao: 'default',
    rolagemTabela: 'unset',
    layoutTabela: 'auto',
    posicao: 'bottom',
    tituloTabela: '',
    rodapeTabela: '',
    adicionar: true,
    action: true
  };
  dadosEntrada: any[] = [];
  searchValue = '';
  visible = false;
  filteredData: any[] = [];
  searchInputs: { [key: string]: string } = {};
  
  // Paginação
  paginaAtual = 1;
  tamanhoPagina = 10;
  totalRegistros = 0;
  carregando = false;

  // Controle de expansão dos filtros
  painelFiltrosExpandido = true;

  // Controle de filtragem local
  private filtroLocal$ = new Subject<void>();
  dadosFiltradosLocalmente: any[] = [];
  ultimaConsulta: ParametrosBusca | null = null;

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

  isColumnSelectorVisible = false;
  columnSelections: NzCheckBoxOptionInterface[] = [];
  private originalColumns: FormCamposMetadata[] = [];
  allChecked = false;

  // Adicione estas propriedades à classe
  sortField: string = '';
  sortOrder: NzTableSortOrder = null;

  constructor(
    private readonly formBuilder: NonNullableFormBuilder, 
    private readonly router: Router, 
    private readonly apiService: ApiService, 
    private readonly toastService: ToastService
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

    // Configura debounce para filtragem local
    this.filtroLocal$
      .pipe(debounceTime(300))
      .subscribe(() => this.aplicarFiltroLocal());
  }

  ngOnInit(): void {
    this.inicializarGrid();
  }

  private async inicializarGrid(): Promise<void> {
    // Usa as configurações padrão se nenhuma configuração for passada
    if (!this.formularioConfiguracao) {
      this.formularioConfiguracao = this.formularioConfiguracaoPadrao;
    }

    if (this.adicionarUrl !== '') {
      this.formularioConfiguracao.controls.adicionar.setValue(true);
    }

    this.formularioConfiguracao.valueChanges.subscribe(valor => {
      this.valorConfiguracao = { ...this.valorConfiguracao, ...valor };
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

    // Inicializa os inputs de busca para cada coluna
    this.gridColumns.forEach(column => {
      this.searchInputs[column.key] = '';
    });

    // Carrega dados da API
    this.dadosEntrada = await this.obterTodos();
    this.listaDados = this.dadosEntrada;
    this.filteredData = [...this.listaDados];

    // Gera colunas baseadas nos metadados da entidade
    const objetoConstructor = this.entidade?.constructor as FormCampoConstrutor;
    if (objetoConstructor) {
      // Mantém apenas colunas que devem ser visíveis por padrão
      this.originalColumns = (objetoConstructor.formFields ?? [])
        .filter(field => field.visible)
        .map(field => ({
          ...field,
          sortable: true,
          filterable: true,
          visible: true
        }));

      // Carrega configurações salvas
      if (this.identificador) {
        const savedConfig = this.loadGridConfig();
        if (savedConfig) {
          this.originalColumns = this.originalColumns.map(col => ({
            ...col,
            visible: savedConfig[col.key] ?? col.visible
          }));
        }
      }

      this.gridColumns = [...this.originalColumns];
      this.initializeColumnSelections();
    }

    await this.buscarDados();
  }

  private initializeColumnSelections(): void {
    this.columnSelections = this.originalColumns.map(col => ({
      label: col.label,
      value: col.key,
      checked: col.visible
    }));
    this.updateAllChecked();
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
      const response = await firstValueFrom(
        this.apiService.get<any[]>(this.buscarTodosUrl)
      );
      if (response.sucesso) {
        return response.dados;
      } else {
        this.toastService.exibirMensagemErro('Erro', 'Erro ao obter dados');
        return [];
      }
    } catch (error: unknown) {
      console.error('Erro ao obter dados:', error);
      this.toastService.exibirMensagemErro('Erro', 'Erro ao obter dados');
      return [];
    }
  }
  
  showColumnSelector(): void {
    this.isColumnSelectorVisible = true;
  }

  handleColumnSelectorCancel(): void {
    this.isColumnSelectorVisible = false;
  }

  handleColumnSelectorOk(): void {
    this.updateColumnsVisibility();
    this.isColumnSelectorVisible = false;
  }

  private updateColumnsVisibility(): void {
    const selectedColumns = new Set(
      this.columnSelections
        .filter(option => option.checked)
        .map(option => option.value)
    );

    this.gridColumns = this.originalColumns.map(col => ({
      ...col,
      visible: selectedColumns.has(col.key)
    }));

    // Salva configurações
    if (this.identificador) {
      const config = this.gridColumns.reduce((acc, col) => ({
        ...acc,
        [col.key]: col.visible
      }), {});
      this.saveGridConfig(config);
    }
  }

  private loadGridConfig(): Record<string, boolean> | null {
    try {
      const key = `grid-config-${this.identificador}`;
      const saved = localStorage.getItem(key);
      return saved ? JSON.parse(saved) : null;
    } catch {
      return null;
    }
  }

  private saveGridConfig(config: Record<string, boolean>): void {
    try {
      const key = `grid-config-${this.identificador}`;
      localStorage.setItem(key, JSON.stringify(config));
    } catch {
      console.warn('Não foi possível salvar a configuração do grid');
    }
  }

  // Método para exportar dados para Excel
  exportToExcel(): void {
    if (!this.listaDados.length) return;

    const visibleColumns = this.gridColumns.filter(col => col.visible);
    const data = this.listaDados.map(row => {
      const newRow: Record<string, any> = {};
      visibleColumns.forEach(col => {
        newRow[col.label] = row[col.key];
      });
      return newRow;
    });

    // TODO: Implementar exportação para Excel
    console.log('Dados para exportar:', data);
  }

  updateAllChecked(): void {
    if (this.columnSelections.length === 0) {
      this.allChecked = false;
      this.indeterminado = false;
      return;
    }

    this.allChecked = this.columnSelections.every(item => item.checked);
    this.indeterminado = !this.allChecked && this.columnSelections.some(item => item.checked);
  }

  onItemCheckedChange(): void {
    this.updateAllChecked();
  }

  onAllChecked(checked: boolean): void {
    this.columnSelections = this.columnSelections.map(item => ({
      ...item,
      checked
    }));
    this.updateAllChecked();
  }

  // Método para buscar dados com filtros
  async buscarDados(forcarConsulta: boolean = false): Promise<void> {
    this.carregando = true;

    try {
      const parametros: ParametrosBusca = {
        filtros: this.obterFiltrosAtivos(),
        paginaAtual: this.paginaAtual,
        tamanhoPagina: this.tamanhoPagina,
        campoOrdenacao: this.sortField || this.gridColumns[0]?.key || '',
        descendente: this.sortOrder === 'descend'
      };

      // Se não forçar consulta e os parâmetros são iguais aos últimos, aplica filtro local
      if (!forcarConsulta && this.ultimaConsulta && this.parametrosIguais(parametros, this.ultimaConsulta)) {
        this.aplicarFiltroLocal();
        return;
      }

      const response = await firstValueFrom(
        this.apiService.post<ResponseGrid<any>>(this.buscarTodosUrl + '/filtrar', parametros)
      );

      if (response.sucesso && response.dados) {
        this.listaDados = response.dados.items ?? [];
        this.dadosFiltradosLocalmente = [...this.listaDados];
        this.totalRegistros = response.dados.total;
        this.ultimaConsulta = parametros;
      } else {
        this.toastService.exibirMensagemErro('Erro', response.mensagem || 'Erro ao buscar dados');
      }
    } catch (error) {
      console.error('Erro ao buscar dados:', error);
      this.toastService.exibirMensagemErro('Erro', 'Erro ao buscar dados');
    } finally {
      this.carregando = false;
    }
  }

  // Compara se os parâmetros de busca são iguais
  private parametrosIguais(params1: ParametrosBusca, params2: ParametrosBusca): boolean {
    return params1.paginaAtual === params2.paginaAtual &&
           params1.tamanhoPagina === params2.tamanhoPagina &&
           params1.campoOrdenacao === params2.campoOrdenacao &&
           params1.descendente === params2.descendente &&
           JSON.stringify(params1.filtros) === JSON.stringify(params2.filtros);
  }

  // Aplica filtro local
  private aplicarFiltroLocal(): void {
    const filtrosAtivos = this.obterFiltrosAtivos();
    
    if (filtrosAtivos.length === 0) {
      this.dadosFiltradosLocalmente = [...this.listaDados];
    } else {
      this.dadosFiltradosLocalmente = this.listaDados.filter(item => {
        return filtrosAtivos.every(filtro => {
          const valorItem = String(item[filtro.campo] || '').toLowerCase();
          return valorItem.includes(filtro.valor.toLowerCase());
        });
      });
    }
  }

  // Método para resetar filtros
  reset(): void {
    this.searchInputs = {};
    this.gridColumns.forEach(column => {
      this.searchInputs[column.key] = '';
    });
    this.paginaAtual = 1;
    this.sortField = '';
    this.sortOrder = null;
    this.buscarDados(true);
  }

  // Método para limpar filtro específico
  clearFilter(key: string): void {
    this.searchInputs[key] = '';
    this.onFilterChange();
  }

  // Handler para mudança de página
  onPageIndexChange(page: number): void {
    this.paginaAtual = page;
    this.buscarDados(true);
  }

  // Handler para mudança de tamanho da página
  onPageSizeChange(size: number): void {
    this.tamanhoPagina = size;
    this.paginaAtual = 1;
    this.buscarDados(true);
  }

  // Handler para mudança nos filtros
  onFilterChange(): void {
    this.paginaAtual = 1; // Reset para primeira página ao filtrar
    this.filtroLocal$.next();
  }

  // Obtém filtros ativos
  private obterFiltrosAtivos(): FiltroGrid[] {
    return Object.entries(this.searchInputs)
      .filter(([_, value]) => value && value.trim() !== '')
      .map(([campo, valor]) => ({
        campo,
        valor: valor.trim()
      }));
  }

  // Adicione este método para lidar com a ordenação
  onSort(sort: { key: string; value: NzTableSortOrder }): void {
    this.sortField = sort.key;
    this.sortOrder = sort.value;
    this.buscarDados(true);
  }
}
