import { Component, OnInit, Input } from '@angular/core';
import { FormControl, FormGroup, NonNullableFormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject, debounceTime, firstValueFrom } from 'rxjs';
import { NzTableSize, NzTablePaginationType, NzTableLayout, NzTablePaginationPosition, NzTableSortOrder } from 'ng-zorro-antd/table';
import { NzCheckBoxOptionInterface } from 'ng-zorro-antd/checkbox';
import { ApiService } from '../../services/http/api.service';
import { ToastService } from '../../services/utils/notificacao/toast.service';
import { LoggingService } from '../../services/utils/log/logging.service';
import { ConfiguracaoGrid, RolagemTabela } from '../../dominio/interface/grid/configuracao-grid';
import { Action } from '../../dominio/interface/grid/action-grid';
import { FormCampoConstrutor, FormCamposMetadata, FiltroConstrutor, FiltroMetadata } from '../../services/decorator/formulario-decorator';
import { FiltroGrid, ParametrosBusca } from '../../dominio/interface/grid/filtros-grid';
import { ResponseGrid } from '../../dominio/interface/grid/response-grid';
import { ApiResponse } from '../../dominio/interface/grid/api-response';
import { Veiculo } from '../../dominio/entidade/veiculo';
import * as XLSX from 'xlsx';

interface GridData {
  [key: string]: any;
}

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
  filterColumns: FiltroMetadata[] = [];
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
  searchInputs: { [key: string]: string | string[] } = {};
  
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
    private readonly toastService: ToastService,
    private readonly loggingService: LoggingService
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
    this.configurarColunas();
    this.configurarFiltros();
    this.initializeColumnSelections();
    this.initializeSearchInputs();
    this.configurarDebounce();
    this.configurarFormulario();
    await this.buscarDados();
  }

  // Configurar colunas baseadas na entidade
  private configurarColunas(): void {
    if (!this.entidade) {
      console.warn('Nenhuma entidade fornecida para o grid');
      this.gridColumns = [];
      this.originalColumns = [];
      return;
    }

    const entidadeConstructor = this.entidade.constructor as FormCampoConstrutor;
    if (!entidadeConstructor?.formFields) {
      console.warn('Nenhum campo definido na entidade para o grid');
      this.gridColumns = [];
      this.originalColumns = [];
      return;
    }

    // Filtrar apenas colunas visíveis por padrão na entidade
    const colunasVisiveis = entidadeConstructor.formFields.filter(field => field.visible !== false);
    
    // Salvar colunas originais (apenas as visíveis)
    this.originalColumns = [...colunasVisiveis];
    
    // Carregar configurações salvas se houver identificador
    if (this.identificador) {
      const savedConfig = this.loadGridConfig();
      if (savedConfig) {
        // Aplicar configurações salvas apenas para colunas originalmente visíveis
        this.gridColumns = this.originalColumns.map(col => ({
          ...col,
          visible: savedConfig.hasOwnProperty(col.key) ? savedConfig[col.key] : col.visible
        }));
      } else {
        // Usar configurações padrão
        this.gridColumns = [...this.originalColumns];
      }
    } else {
      // Usar configurações padrão se não houver identificador
      this.gridColumns = [...this.originalColumns];
    }
    
    this.loggingService.log('Colunas visíveis por padrão:', this.originalColumns.map(c => c.key));
    this.loggingService.log('Colunas do grid configuradas:', this.gridColumns.map(c => `${c.key}:${c.visible}`));
  }

  // Configurar filtros baseados na entidade
  private configurarFiltros(): void {
    if (!this.entidade) {
      console.warn('Nenhuma entidade fornecida para os filtros');
      this.filterColumns = [];
      return;
    }

    const entidadeConstructor = this.entidade.constructor as FiltroConstrutor;
    if (!entidadeConstructor?.filterFields) {
      console.warn('Nenhum filtro definido na entidade');
      this.filterColumns = [];
      return;
    }

    this.filterColumns = [...entidadeConstructor.filterFields];
    this.loggingService.log('Filtros definidos:', this.filterColumns.map(f => f.key));
  }

  // Inicializar inputs de busca
  private initializeSearchInputs(): void {
    this.filterColumns.forEach(filter => {
      switch (filter.type) {
        case 'bool':
        case 'enum':
          this.searchInputs[filter.key] = [];
          break;
        case 'texto':
        case 'numero':
        case 'data':
        default:
          this.searchInputs[filter.key] = '';
          break;
      }
    });
  }

  // Configurar debounce para filtros
  private configurarDebounce(): void {
    this.filtroLocal$
      .pipe(debounceTime(300))
      .subscribe(() => this.aplicarFiltroLocal());
  }

  // Configurar configurações do formulário
  private configurarFormulario(): void {
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
  }

  private initializeColumnSelections(): void {
    this.loggingService.log('=== INICIALIZANDO SELEÇÕES DE COLUNAS ===');
    this.loggingService.log('originalColumns:', this.originalColumns);
    this.loggingService.log('gridColumns:', this.gridColumns);
    
    if (this.originalColumns.length === 0) {
      console.warn('originalColumns está vazio. Não é possível criar seleções de colunas.');
      this.columnSelections = [];
      return;
    }

    this.columnSelections = this.originalColumns.map(col => ({
      label: col.label,
      value: col.key,
      checked: col.visible
    }));
    
    this.loggingService.log('columnSelections criadas:', this.columnSelections);
    this.updateAllChecked();
    this.loggingService.log('allChecked:', this.allChecked, 'indeterminado:', this.indeterminado);
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
    this.loggingService.log('=== ABRINDO SELETOR DE COLUNAS ===');
    this.loggingService.log('columnSelections antes de abrir:', this.columnSelections);
    this.loggingService.log('originalColumns:', this.originalColumns);
    this.loggingService.log('gridColumns:', this.gridColumns);
    
    if (this.columnSelections.length === 0) {
      console.warn('columnSelections está vazio. Reinicializando...');
      this.initializeColumnSelections();
    }
    
    this.isColumnSelectorVisible = true;
    this.loggingService.log('Modal de seleção de colunas aberto');
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
    if (!this.listaDados.length) {
      this.toastService.exibirMensagemErro('Erro', 'Não há dados para exportar');
      return;
    }

    try {
      // Filtra apenas as colunas visíveis
      const visibleColumns = this.gridColumns.filter(col => col.visible);
      
      // Prepara os dados para exportação
      const data = this.listaDados.map(row => {
        const newRow: Record<string, any> = {};
        visibleColumns.forEach(col => {
          newRow[col.label] = row[col.key];
        });
        return newRow;
      });

      // Cria a planilha
      const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(data);
      const wb: XLSX.WorkBook = XLSX.utils.book_new();
      XLSX.utils.book_append_sheet(wb, ws, 'Dados');

      // Gera o nome do arquivo com data e hora
      const now = new Date();
      const fileName = `exportacao_${now.getFullYear()}${(now.getMonth() + 1).toString().padStart(2, '0')}${now.getDate().toString().padStart(2, '0')}_${now.getHours().toString().padStart(2, '0')}${now.getMinutes().toString().padStart(2, '0')}.xlsx`;

      // Salva o arquivo
      XLSX.writeFile(wb, fileName);
      
      this.toastService.exibirMensagemSucesso('Sucesso', 'Dados exportados com sucesso');
    } catch (error) {
      console.error('Erro ao exportar dados:', error);
      this.toastService.exibirMensagemErro('Erro', 'Erro ao exportar dados');
    }
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
        campoOrdenacao: this.sortField || this.gridColumns[0]?.key || 'Id',
        descendente: this.sortOrder === 'descend'
      };

      // Se não forçar consulta e os parâmetros são iguais aos últimos, aplica filtro local
      if (!forcarConsulta && this.ultimaConsulta && this.parametrosIguais(parametros, this.ultimaConsulta)) {
        this.aplicarFiltroLocal();
        return;
      }

      const response = await firstValueFrom(
        this.apiService.post<ApiResponse<any>>(this.buscarTodosUrl + '/filtrar', parametros)
      );

      if (response.sucesso && response.dados) {
        // Verifica se a resposta tem a estrutura esperada do grid paginado
        if (response.dados.hasOwnProperty('items') && response.dados.hasOwnProperty('total')) {
          const gridData = response.dados as any;
          this.listaDados = gridData.items ?? [];
          this.totalRegistros = gridData.total ?? 0;
          this.paginaAtual = gridData.pagina || this.paginaAtual;
          this.tamanhoPagina = gridData.tamanhoPagina || this.tamanhoPagina;
        } else {
          // Fallback para estrutura antiga
          this.listaDados = Array.isArray(response.dados) ? response.dados : [];
          this.totalRegistros = this.listaDados.length;
        }
        
        this.dadosFiltradosLocalmente = [...this.listaDados];
        this.ultimaConsulta = parametros;
      } else {
        this.toastService.exibirMensagemErro('Erro', response.mensagem || 'Erro ao buscar dados');
        this.listaDados = [];
        this.totalRegistros = 0;
      }
    } catch (error) {
      console.error('Erro ao buscar dados:', error);
      this.toastService.exibirMensagemErro('Erro', 'Erro ao buscar dados');
      this.listaDados = [];
      this.totalRegistros = 0;
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
    this.filterColumns.forEach(filter => {
      switch (filter.type) {
        case 'bool':
        case 'enum':
          this.searchInputs[filter.key] = [];
          break;
        case 'texto':
        case 'numero':
        case 'data':
        default:
          this.searchInputs[filter.key] = '';
          break;
      }
    });
    this.paginaAtual = 1;
    this.sortField = '';
    this.sortOrder = null;
    this.buscarDados(true);
  }

  // Método para limpar filtro específico
  clearFilter(key: string): void {
    const filter = this.filterColumns.find(f => f.key === key);
    if (filter) {
      switch (filter.type) {
        case 'bool':
        case 'enum':
          this.searchInputs[key] = [];
          break;
        case 'texto':
        case 'numero':
        case 'data':
        default:
          this.searchInputs[key] = '';
          break;
      }
    } else {
      this.searchInputs[key] = '';
    }
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
      .filter(([_, value]) => {
        if (Array.isArray(value)) {
          return value.length > 0;
        }
        return value && value.trim() !== '';
      })
      .flatMap(([campo, valor]) => {
        if (Array.isArray(valor)) {
          // Para filtros multi-seleção, criar um filtro para cada valor selecionado
          return valor.map(v => ({
            campo,
            valor: String(v).trim(),
            operador: 'contains' // ou 'equals' dependendo do tipo
          }));
        } else {
          return [{
            campo,
            valor: String(valor).trim(),
            operador: 'contains'
          }];
        }
      });
  }

  // Adicione este método para lidar com a ordenação
  onSort(sort: { key: string; value: NzTableSortOrder }): void {
    this.sortField = sort.key;
    this.sortOrder = sort.value;
    this.buscarDados(true);
  }

  // Método para tracking do ngFor
  trackByFn(index: number, item: any): any {
    return item?.id || index;
  }

  // Método para formatar valores das células
  formatCellValue(value: any, columnKey: string): string {
    if (value === null || value === undefined) {
      return '';
    }

    // Formatação específica por tipo de coluna
    switch (columnKey) {
      case 'situacao':
      case 'possuiBanheiro':
      case 'possuiClimatizador':
        return value ? 'Sim' : 'Não';
        
      case 'situacaoDescricao':
      case 'possuiBanheiroDescricao':
      case 'possuiClimatizadorDescricao':
        return String(value);
        
      case 'createdAt':
      case 'updatedAt':
        if (value) {
          try {
            const date = new Date(value);
            return date.toLocaleDateString('pt-BR') + ' ' + date.toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit' });
          } catch {
            return String(value);
          }
        }
        return '';
        
      default:
        return String(value);
    }
  }

  // Verifica se um campo é boolean (Sim/Não)
  isBooleanField(columnKey: string): boolean {
    const booleanFields = [
      'situacao', 
      'situacaoDescricao',
      'possuiBanheiro', 
      'possuiBanheiroDescricao',
      'possuiClimatizador', 
      'possuiClimatizadorDescricao'
    ];
    return booleanFields.includes(columnKey.toLowerCase());
  }

  // Verifica se um campo é enum
  isEnumField(columnKey: string): boolean {
    const enumFields = [
      'tipo', 
      'tipoDescricao',
      'status',
      'statusDescricao'
    ];
    return enumFields.includes(columnKey.toLowerCase());
  }

  // Obtém as opções de um campo enum
  getEnumOptions(columnKey: string): { value: string; label: string }[] {
    const key = columnKey.toLowerCase();
    
    switch (key) {
      case 'tipo':
      case 'tipodescricao':
        return [
          { value: '0', label: 'Ônibus Urbano' },
          { value: '1', label: 'Microônibus' },
          { value: '2', label: 'Van' },
          { value: '3', label: 'Carro' },
          { value: '4', label: 'Ônibus Rodoviário' },
          { value: '5', label: 'Ônibus Articulado' }
        ];
      
      case 'status':
      case 'statusdescricao':
        return [
          { value: 'Ativo', label: 'Ativo' },
          { value: 'Inativo', label: 'Inativo' },
          { value: 'Manutencao', label: 'Manutenção' },
          { value: 'Disponivel', label: 'Disponível' }
        ];
        
      default:
        return [];
    }
  }
}
