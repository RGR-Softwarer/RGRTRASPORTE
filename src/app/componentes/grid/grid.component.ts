import { Component, OnInit, Input, OnDestroy, ViewChild, ElementRef, ChangeDetectorRef } from '@angular/core';
import { FormControl, FormGroup, NonNullableFormBuilder, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Subject, debounceTime, firstValueFrom, takeUntil } from 'rxjs';
import { ApiService } from '../../services/http/api.service';
import { LoggingService } from '../../services/utils/log/logging.service';
import { ConfiguracaoGrid, RolagemTabela, TamanhoTabela, LayoutTabela, PosicaoPaginacao, TipoPaginacao } from '../../dominio/interface/grid/configuracao-grid';
import { Action } from '../../dominio/interface/grid/action-grid';
import { DecoratorUtils, FiltroMetadata } from '../../services/decorator/formulario-decorator';
import { InputFieldComponent } from '../../shared/components/form/input-field/input-field.component';
import { SelectFieldComponent } from '../../shared/components/form/select-field/select-field.component';

import { ButtonComponent } from '../../shared/components/button/button.component';
import { ModalComponent } from '../../shared/components/modal/modal.component';
import { NotificationService } from '../../shared/services/notification.service';

interface GridData {
  [key: string]: any;
}

interface GridItem {
  id?: number | string;
  [key: string]: any;
}

interface FormCamposMetadata {
  key: string;
  label: string;
  visible: boolean;
  type?: string;
}

interface ParametrosBusca {
  pagina?: number;
  tamanhoPagina?: number;
  filtros?: { [key: string]: any };
  ordenacao?: { campo: string; direcao: 'asc' | 'desc' };
}

@Component({
  selector: 'app-grid',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    InputFieldComponent,
    SelectFieldComponent,
    ButtonComponent,
    ModalComponent
  ],
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.scss']
})
export class GridComponent implements OnInit, OnDestroy {
  @Input() formularioConfiguracao!: FormGroup<{ [K in keyof ConfiguracaoGrid]: FormControl<ConfiguracaoGrid[K]> }>;
  @Input() buscarTodosUrl: string = '';
  @Input() adicionarUrl: string = '';
  @Input() acoes?: Action[] = [{ label: 'Editar', acao: this.editar.bind(this) }];
  @Input() entidade: any;
  @Input() identificador: string = '';
  @Input() filtroInicial?: string; // Filtro inicial para aplicar na grid

  gridColumns: FormCamposMetadata[] = []; 
  filterColumns: FiltroMetadata[] = [];
  listaDados: GridItem[] = [];
  dadosExibidos: readonly GridItem[] = [];
  todosMarcados = false;
  indeterminado = false;
  colunaFixa = false;
  rolagemX: string | null = null;
  rolagemY: string | null = null;
  valorConfiguracao: ConfiguracaoGrid = {
    comBorda: true,
    carregando: false,
    paginacao: true,
    alteradorTamanho: false,
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
    rolagemTabela: 'unset',
    layoutTabela: 'auto',
    posicao: 'bottom',
    tipoPaginacao: 'default',
    tituloTabela: 'Título da Tabela',
    rodapeTabela: 'Rodapé da Tabela',
    adicionar: false,
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
  painelFiltrosExpandido = false;

  // Controle de filtragem local
  private destroy$ = new Subject<void>();
  private filtroLocal$ = new Subject<void>();
  dadosFiltradosLocalmente: any[] = [];
  ultimaConsulta: ParametrosBusca | null = null;

  formularioConfiguracaoPadrao: FormGroup<{ [K in keyof ConfiguracaoGrid]: FormControl<ConfiguracaoGrid[K]> }>;

  listOfSwitch = [
    { label: 'Com Borda', value: 'comBorda' },
    { label: 'Carregando', value: 'carregando' },
    { label: 'Paginação', value: 'paginacao' },
    { label: 'Alterador de Tamanho', value: 'alteradorTamanho' },
    { label: 'Título', value: 'titulo' },
    { label: 'Cabeçalho', value: 'cabecalho' },
    { label: 'Rodapé', value: 'rodape' },
    { label: 'Expansível', value: 'expansivel' },
    { label: 'Caixa de Seleção', value: 'caixaSelecao' },
    { label: 'Cabeçalho Fixo', value: 'cabecalhoFixo' },
    { label: 'Sem Resultado', value: 'semResultado' },
    { label: 'Elipse', value: 'elipse' },
    { label: 'Simples', value: 'simples' },
    { label: 'Mostrar Opções', value: 'mostrarOpcoes' },
    { label: 'Adicionar', value: 'adicionar' },
    { label: 'Ação', value: 'action' }
  ];

  listOfRadio = [
    { label: 'Pequeno', value: 'small' },
    { label: 'Médio', value: 'middle' },
    { label: 'Padrão', value: 'default' }
  ];

  listOfPaginationType = [
    { label: 'Padrão', value: 'default' },
    { label: 'Simples', value: 'simple' }
  ];

  listOfPaginationPosition = [
    { label: 'Topo', value: 'top' },
    { label: 'Rodapé', value: 'bottom' }
  ];

  listOfTableLayout = [
    { label: 'Automático', value: 'auto' },
    { label: 'Fixo', value: 'fixed' }
  ];

  listOfScroll = [
    { label: 'Não Definido', value: 'unset' },
    { label: 'Rolagem', value: 'scroll' },
    { label: 'Fixo', value: 'fixed' }
  ];

  isColumnSelectorVisible = false;
  columnSelections: any[] = [];
  private originalColumns: FormCamposMetadata[] = [];
  allChecked = false;

  // Ordenação
  sortField: string = '';
  sortOrder: 'ascend' | 'descend' | null = null;

  constructor(
    private readonly formBuilder: NonNullableFormBuilder,
    private readonly router: Router,
    private readonly apiService: ApiService,
    private readonly loggingService: LoggingService,
    private readonly changeDetectorRef: ChangeDetectorRef,
    private notificationService: NotificationService
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
      tamanho: ['small' as TamanhoTabela],
      tipoPaginacao: ['default' as TipoPaginacao],
      rolagemTabela: ['unset' as RolagemTabela],
      layoutTabela: ['auto' as LayoutTabela],
      posicao: ['bottom' as PosicaoPaginacao],
      tituloTabela: ['Título da Tabela'],
      rodapeTabela: ['Rodapé da Tabela'],
      adicionar: [false],
      action: [true]
    });

    // Configura debounce para filtragem local
    this.filtroLocal$
      .pipe(takeUntil(this.destroy$), debounceTime(300))
      .subscribe(() => this.aplicarFiltroLocal());

    // Inicializa as configurações padrão da grid
    this.valorConfiguracao = {
      ...this.formularioConfiguracaoPadrao.value,
      ...this.valorConfiguracao
    };
    if (this.valorConfiguracao.adicionar === undefined) {
      this.valorConfiguracao.adicionar = true;
    }
    
    // Inicializar arrays vazios
    this.dadosFiltradosLocalmente = [];
    this.listaDados = [];
    this.dadosEntrada = [];
    
    // Garantir que o botão adicionar apareça se houver URL
    if (this.adicionarUrl && this.adicionarUrl.trim() !== '') {
      this.valorConfiguracao.adicionar = true;
    }
  }

  ngOnInit(): void {
    this.validarInputs();
    this.inicializarGrid();
    this.buscarDados();
    
    // Garantir que o botão adicionar apareça se houver URL
    if (this.adicionarUrl && this.adicionarUrl.trim() !== '') {
      this.valorConfiguracao.adicionar = true;
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private validarInputs(): void {
    if (!this.buscarTodosUrl) {
      console.warn('GridComponent: buscarTodosUrl é obrigatório');
    }
    if (!this.entidade) {
      console.warn('GridComponent: entidade é obrigatória');
    }
  }

  private async inicializarGrid(): Promise<void> {
    // Configurar colunas da grid
    this.configurarColunas();
    
    // Configurar filtros
    this.configurarFiltros();
    
    // Inicializar inputs de busca
    this.initializeSearchInputs();
    
    // Configurar debounce para filtros
    this.configurarDebounce();
    
    // Configurar seleção de colunas
    this.initializeColumnSelections();
  }

  // Configurar colunas baseadas na entidade
  private configurarColunas(): void {
    if (!this.entidade) {
      console.warn('❌ Nenhuma entidade fornecida para o grid');
      this.gridColumns = [];
      this.originalColumns = [];
      return;
    }

    // Sempre criar uma instância se a entidade for uma classe
    let entidadeParaUsar = this.entidade;
    if (typeof this.entidade === 'function') {
      try {
        entidadeParaUsar = new this.entidade();
      } catch (error) {
        console.error('❌ Erro ao criar instância da entidade:', error);
        this.gridColumns = [];
        this.originalColumns = [];
        return;
      }
    }

    const allColumns = DecoratorUtils.getFormFields(entidadeParaUsar);
    
    if (allColumns.length === 0) {
      console.warn('❌ Nenhum campo definido na entidade para o grid');
      this.gridColumns = [];
      this.originalColumns = [];
      return;
    }

    // Filtrar apenas colunas visíveis por padrão na entidade
    const colunasVisiveis = allColumns.filter(field => field.visible);
    
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
  }

  // Configurar filtros baseados na entidade
  private configurarFiltros(): void {
    if (!this.entidade) {
      console.warn('❌ Nenhuma entidade fornecida para os filtros');
      this.filterColumns = [];
      return;
    }

    // Sempre criar uma instância se a entidade for uma classe
    let entidadeParaUsar = this.entidade;
    if (typeof this.entidade === 'function') {
      try {
        entidadeParaUsar = new this.entidade();
      } catch (error) {
        console.error('❌ Erro ao criar instância da entidade para filtros:', error);
        this.filterColumns = [];
        return;
      }
    }

    this.filterColumns = DecoratorUtils.getFilterFields(entidadeParaUsar);
    
    if (this.filterColumns.length === 0) {
      console.warn('Nenhum filtro definido na entidade');
    }
  }

  // Inicializar inputs de busca
  private initializeSearchInputs(): void {
    this.filterColumns.forEach(filter => {
      this.searchInputs[filter.key] = '';
    });
  }

  // Configurar debounce para filtros
  private configurarDebounce(): void {
    this.filtroLocal$
      .pipe(takeUntil(this.destroy$), debounceTime(300))
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
    if (this.originalColumns.length === 0) {
      console.warn('originalColumns está vazio. Não é possível criar seleções de colunas.');
      this.columnSelections = [];
      return;
    }

    // Carregar configurações salvas se houver identificador
    let savedConfig: Record<string, boolean> | null = null;
    if (this.identificador) {
      savedConfig = this.loadGridConfig();
    }

    this.columnSelections = this.originalColumns.map(col => {
      // Se há configuração salva, usar ela; senão usar o estado atual da coluna
      const isVisible = savedConfig 
        ? (savedConfig.hasOwnProperty(col.key) ? savedConfig[col.key] : col.visible)
        : col.visible;
      
      return {
        label: col.label,
        value: col.key,
        checked: isVisible
      };
    });
    
    this.updateAllChecked();
  }

  mudancaDadosPaginaAtual($event: readonly GridItem[]): void {
    this.dadosExibidos = $event;
    this.atualizarStatus();
  }

  atualizarStatus(): void {
    const dadosValidos = this.dadosExibidos.filter(valor => !valor['desativado']);
    const todosMarcados = dadosValidos.length > 0 && dadosValidos.every(valor => valor['marcado'] === true);
    const todosDesmarcados = dadosValidos.every(valor => !valor['marcado']);
    this.todosMarcados = todosMarcados;
    this.indeterminado = !todosMarcados && !todosDesmarcados;
  }

  editar(item: any): void {
    // Construir a URL de edição com o ID do item
    const editarUrl = this.adicionarUrl.replace('/adicionar', `/editar/${item.id || item.Id || item.ID}`);
    
    this.router.navigate([editarUrl], { 
      state: { 
        isEditMode: true, 
        objeto: JSON.stringify(item) // Converte objeto para string JSON para evitar problemas de serialização
      }
    });
  }  

  adicionar(): void {
    this.router.navigate([this.adicionarUrl], { 
      queryParams: { 
        isEditMode: false // Define que não está no modo de edição
      }
    });
  }

  async obterTodos(): Promise<any[]> {
    const startTime = performance.now();
    console.log('🚀 [PERFORMANCE] Iniciando obterTodos()...');
    
    try {
      const response = await firstValueFrom(
        this.apiService.get<any[]>(this.buscarTodosUrl)
      );
      
      const apiTime = performance.now() - startTime;
      console.log(`⚡ [PERFORMANCE] API respondeu em ${apiTime.toFixed(2)}ms`);
      
      if (response && response.sucesso && response.dados) {
        const dados = Array.isArray(response.dados) ? response.dados : [];
        const totalTime = performance.now() - startTime;
        console.log(`✅ [PERFORMANCE] obterTodos() finalizado em ${totalTime.toFixed(2)}ms - ${dados.length} registros`);
        return dados;
      } else {
        console.warn('Resposta da API não contém dados válidos:', response);
        return [];
      }
    } catch (error: unknown) {
      console.error('Erro ao obter dados:', error);
      this.notificationService.error('Erro', 'Erro ao obter dados');
      return [];
    }
  }
  
  showColumnSelector(): void {
    this.loggingService.log('=== ABRINDO SELETOR DE COLUNAS ===');
    this.loggingService.log('columnSelections antes de abrir:', this.columnSelections);
    this.loggingService.log('originalColumns:', this.originalColumns);
    this.loggingService.log('gridColumns:', this.gridColumns);
    
    // Debug do cache
    this.debugGridConfig();
    
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
      
      console.log('💾 Salvando configuração do grid:', config);
      this.saveGridConfig(config);
    }
  }

  private loadGridConfig(): Record<string, boolean> | null {
    try {
      const key = `grid-config-${this.identificador}`;
      const saved = localStorage.getItem(key);
      if (saved) {
        const config = JSON.parse(saved);
        console.log('📂 Configuração carregada do cache:', { key, config });
        return config;
      } else {
        console.log('📂 Nenhuma configuração encontrada no cache para:', key);
        return null;
      }
    } catch (error) {
      console.warn('❌ Erro ao carregar configuração do cache:', error);
      return null;
    }
  }

  private saveGridConfig(config: Record<string, boolean>): void {
    try {
      const key = `grid-config-${this.identificador}`;
      localStorage.setItem(key, JSON.stringify(config));
      console.log('💾 Configuração salva com sucesso:', { key, config });
    } catch (error) {
      console.warn('❌ Não foi possível salvar a configuração do grid:', error);
    }
  }

  updateAllChecked(): void {
    const validCount = this.columnSelections.filter(item => item.checked).length;
    this.allChecked = validCount === this.columnSelections.length;
    this.indeterminado = validCount > 0 && validCount < this.columnSelections.length;
  }

  onItemCheckedChange(): void {
    this.updateAllChecked();
  }

  onAllChecked(checked: boolean): void {
    this.columnSelections.forEach(item => item.checked = checked);
    this.updateAllChecked();
  }

  async buscarDados(forcarConsulta: boolean = false): Promise<void> {
    try {
      this.carregando = true;
      
      // Verificar se já temos dados e se não precisamos forçar consulta
      if (!forcarConsulta && this.dadosEntrada.length > 0) {
        console.log('📊 Usando dados em cache, não consultando API');
        this.listaDados = [...this.dadosEntrada];
        this.dadosFiltradosLocalmente = [...this.dadosEntrada];
        this.totalRegistros = this.listaDados.length;
        this.carregando = false;
        this.aplicarFiltroLocal();
        
        // Aplicar filtro inicial se fornecido
        if (this.filtroInicial && this.filtroInicial.trim() !== '') {
          this.aplicarFiltroInicial();
        }
        
        // Forçar detecção de mudanças
        this.changeDetectorRef.detectChanges();
        return;
      }

      console.log('🔄 Buscando dados da API...');
      const dados = await this.obterTodos();
      
      this.dadosEntrada = dados;
      this.listaDados = [...dados];
      this.dadosFiltradosLocalmente = [...dados];
      this.totalRegistros = dados.length;
      
      console.log(`✅ Dados carregados: ${dados.length} registros`);
      
      // Aplicar filtros após carregar dados
      this.aplicarFiltroLocal();
      
      // Aplicar filtro inicial se fornecido
      if (this.filtroInicial && this.filtroInicial.trim() !== '') {
        this.aplicarFiltroInicial();
      }
      
      // Forçar detecção de mudanças
      this.changeDetectorRef.detectChanges();
    } catch (error) {
      console.error('❌ Erro ao buscar dados:', error);
      this.notificationService.error('Erro', 'Falha ao carregar dados');
    } finally {
      this.carregando = false;
    }
  }

  private aplicarFiltroLocal(): void {
    let dadosFiltrados = [...this.listaDados];
    
    // Aplicar filtro de busca geral
    if (this.searchValue && this.searchValue.trim() !== '') {
      dadosFiltrados = dadosFiltrados.filter(item =>
        Object.values(item).some(value =>
          String(value).toLowerCase().includes(this.searchValue.toLowerCase())
        )
      );
    }
    
    // Aplicar filtros específicos
    Object.keys(this.searchInputs).forEach(key => {
      const valor = this.searchInputs[key];
      // Só aplicar filtro se há um valor definido e não é string vazia
      if (valor !== null && valor !== undefined && valor.toString().trim() !== '') {
        dadosFiltrados = dadosFiltrados.filter(item => {
          const itemValue = item[key];
          // Para campos boolean
          if (typeof itemValue === 'boolean') {
            return itemValue === (valor === 'true');
          }
          // Para campos de texto
          if (typeof itemValue === 'string') {
            return itemValue.toLowerCase().includes(valor.toString().toLowerCase());
          }
          // Para campos enum e outros
          return String(itemValue).toLowerCase() === String(valor).toLowerCase();
        });
      }
    });
    
    this.dadosFiltradosLocalmente = dadosFiltrados;
    this.totalRegistros = this.dadosFiltradosLocalmente.length;
    this.paginaAtual = 1;
    
    // Aplicar ordenação se houver
    if (this.sortField && this.sortOrder) {
      this.dadosFiltradosLocalmente.sort((a, b) => {
        const aValue = a[this.sortField];
        const bValue = b[this.sortField];
        
        if (aValue === null || aValue === undefined) return 1;
        if (bValue === null || bValue === undefined) return -1;
        
        const comparison = aValue < bValue ? -1 : aValue > bValue ? 1 : 0;
        return this.sortOrder === 'descend' ? -comparison : comparison;
      });
    }
    
    // Forçar detecção de mudanças
    this.changeDetectorRef.detectChanges();
  }

  reset(): void {
    this.searchValue = '';
    this.searchInputs = {};
    this.paginaAtual = 1;
    this.sortField = '';
    this.sortOrder = null;
    
    // Resetar filtros
    this.filterColumns.forEach(filter => {
      this.searchInputs[filter.key] = '';
    });
    
    this.aplicarFiltroLocal();
    this.notificationService.success('Sucesso', 'Filtros resetados');
  }

  clearFilter(key: string): void {
    const filter = this.filterColumns.find(f => f.key === key);
    if (filter) {
      this.searchInputs[key] = '';
      this.filtroLocal$.next();
      console.log(`🧹 Filtro "${key}" limpo`);
    }
  }

  onPageIndexChange(page: number): void {
    this.paginaAtual = page;
    console.log(`📄 Página alterada para: ${page}`);
  }

  onPageSizeChange(size: number): void {
    this.tamanhoPagina = size;
    this.paginaAtual = 1; // Reset para primeira página
    console.log(`📏 Tamanho da página alterado para: ${size}`);
  }

  onFilterChange(): void {
    this.filtroLocal$.next();
  }

  onSort(sort: { key: string; value: 'ascend' | 'descend' | null }): void {
    this.sortField = sort.key;
    this.sortOrder = sort.value;
    
    // Aplicar ordenação local se necessário
    if (this.sortOrder) {
      this.dadosFiltradosLocalmente.sort((a, b) => {
        const aValue = a[this.sortField];
        const bValue = b[this.sortField];
        
        if (aValue === null || aValue === undefined) return 1;
        if (bValue === null || bValue === undefined) return -1;
        
        const comparison = aValue < bValue ? -1 : aValue > bValue ? 1 : 0;
        return this.sortOrder === 'descend' ? -comparison : comparison;
      });
    }
  }

  onSortChange(key: string, value: string | null): void {
    this.onSort({ key, value: value as 'ascend' | 'descend' | null });
  }

  trackByFn(index: number, item: any): any {
    return item.id || index;
  }

  formatCellValue(value: any, columnKey: string): string {
    if (value === null || value === undefined) {
      return '-';
    }

    if (typeof value === 'boolean') {
      return value ? 'Sim' : 'Não';
    }

    if (typeof value === 'object') {
      return JSON.stringify(value);
    }

    return String(value);
  }

  isBooleanField(columnKey: string): boolean {
    const column = this.gridColumns.find(col => col.key === columnKey);
    return column?.type === 'boolean' || column?.type === 'bool';
  }

  isEnumField(columnKey: string): boolean {
    const column = this.gridColumns.find(col => col.key === columnKey);
    return column?.type === 'enum';
  }

  getEnumOptions(columnKey: string): { value: string; label: string }[] {
    // Implementar lógica para obter opções de enum
    return [];
  }

  clearGridConfig(): void {
    if (this.identificador) {
      try {
        const key = `grid-config-${this.identificador}`;
        localStorage.removeItem(key);
        console.log('🗑️ Configuração do grid removida:', key);
        
        // Recarregar configurações padrão
        this.initializeColumnSelections();
        this.configurarColunas();
        
        this.notificationService.success('Sucesso', 'Configuração do grid resetada');
      } catch (error) {
        console.warn('❌ Erro ao limpar configuração do grid:', error);
        this.notificationService.error('Erro', 'Falha ao limpar configuração');
      }
    }
  }

  debugGridConfig(): void {
    if (this.identificador) {
      const key = `grid-config-${this.identificador}`;
      const saved = localStorage.getItem(key);
      console.log('🔍 [DEBUG] Configuração atual do grid:', {
        key,
        saved: saved ? JSON.parse(saved) : null,
        columnSelections: this.columnSelections,
        gridColumns: this.gridColumns
      });
    }
  }

  getScrollConfig(): { x?: string; y?: string } {
    // Sempre habilita scroll horizontal para muitas colunas
    return { x: 'max-content', y: this.rolagemY || undefined };
  }

  exportToExcel(): void {
    try {
      // Implementação básica de exportação para Excel
      const data = this.dadosFiltradosLocalmente.length > 0 ? this.dadosFiltradosLocalmente : this.listaDados;
      
      if (data.length === 0) {
        this.notificationService.warning('Aviso', 'Nenhum dado para exportar');
        return;
      }

      // Criar cabeçalhos baseados nas colunas visíveis
      const headers = this.gridColumns
        .filter(col => col.visible)
        .map(col => col.label);

      // Criar linhas de dados
      const rows = data.map(item => 
        this.gridColumns
          .filter(col => col.visible)
          .map(col => this.formatCellValue(item[col.key], col.key))
      );

      // Combinar cabeçalhos e dados
      const csvContent = [headers, ...rows]
        .map(row => row.map(cell => `"${cell}"`).join(','))
        .join('\n');

      // Criar e baixar arquivo
      const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
      const link = document.createElement('a');
      const url = URL.createObjectURL(blob);
      link.setAttribute('href', url);
      link.setAttribute('download', `export_${new Date().toISOString().split('T')[0]}.csv`);
      link.style.visibility = 'hidden';
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);

      this.notificationService.success('Sucesso', 'Dados exportados com sucesso');
    } catch (error) {
      console.error('Erro ao exportar dados:', error);
      this.notificationService.error('Erro', 'Falha ao exportar dados');
    }
  }

  getPageNumbers(): number[] {
    const totalPages = this.getTotalPages();
    const currentPage = this.paginaAtual;
    const pages: number[] = [];
    
    const start = Math.max(1, currentPage - 2);
    const end = Math.min(totalPages, currentPage + 2);
    
    for (let i = start; i <= end; i++) {
      pages.push(i);
    }
    
    return pages;
  }

  getTotalPages(): number {
    return Math.ceil(this.totalRegistros / this.tamanhoPagina);
  }

  getPaginationInfo(): string {
    const start = (this.paginaAtual - 1) * this.tamanhoPagina + 1;
    const end = Math.min(this.paginaAtual * this.tamanhoPagina, this.totalRegistros);
    return `${start}-${end} de ${this.totalRegistros} registros`;
  }

  private aplicarFiltroInicial(): void {
    if (!this.filtroInicial || this.filtroInicial.trim() === '') {
      return;
    }

    // Aplicar o filtro inicial no campo de busca geral
    this.searchValue = this.filtroInicial;
    
    // Aplicar o filtro nos campos específicos se houver correspondência
    if (this.filterColumns.length > 0) {
      // Tentar aplicar o filtro no primeiro campo de texto disponível
      const primeiroCampoTexto = this.filterColumns.find(f => f.type === 'texto');
      if (primeiroCampoTexto) {
        this.searchInputs[primeiroCampoTexto.key] = this.filtroInicial;
      }
    }
    
    // Aplicar o filtro
    this.aplicarFiltroLocal();
    
    console.log(`✅ Filtro inicial aplicado: "${this.filtroInicial}"`);
  }
} 
