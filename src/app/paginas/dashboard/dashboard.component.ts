import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormControl, FormGroup, NonNullableFormBuilder, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Subject, takeUntil } from 'rxjs';
import { AppContext } from '../../dominio/entidade/app.context';
import { AppContextService } from '../../services/context/app.context';
import { ConfiguracaoGrid, RolagemTabela, TamanhoTabela, LayoutTabela, PosicaoPaginacao, TipoPaginacao } from '../../dominio/interface/grid/configuracao-grid';

interface DashboardData {
  nome: string;
  idade: string;
  endereco: string;
  descricao: string;
  marcado: boolean;
  expandido: boolean;
}

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class DashboardComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  
  userContext: AppContext | null = null;
  formularioConfiguracao!: FormGroup<{ [K in keyof ConfiguracaoGrid]: FormControl<ConfiguracaoGrid[K]> }>;
  minhaListaDeObjetos: DashboardData[] = [];
  loading: boolean = false;
  
  // Dados da tabela (simplificado sem TableColumn/TableAction)

  constructor(
    private appContextService: AppContextService, 
    private formBuilder: NonNullableFormBuilder
  ) {
    this.inicializarFormulario();
  }

  ngOnInit(): void {
    this.carregarDados();
    this.observarMudancasFormulario();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private inicializarFormulario(): void {
    this.formularioConfiguracao = this.formBuilder.group({
      comBorda: [false],
      carregando: [false],
      paginacao: [true],
      alteradorTamanho: [false],
      titulo: [true],
      cabecalho: [true],
      rodape: [true],
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
      tituloTabela: ['Dashboard - Dados do Sistema'],
      rodapeTabela: ['Total de registros carregados'],
      adicionar: [false],
      action: [false]
    });
  }

  private carregarDados(): void {
    this.loading = true;
    
    try {
      this.userContext = this.appContextService.obterUsuarioLogado();
      console.log('Contexto do usuário:', this.userContext);
      
      // Simula carregamento assíncrono
      setTimeout(() => {
        this.minhaListaDeObjetos = this.gerarDados();
        this.loading = false;
      }, 1000);
    } catch (error) {
      console.error('Erro ao carregar dados do dashboard:', error);
      this.loading = false;
    }
  }

  private observarMudancasFormulario(): void {
    this.formularioConfiguracao.valueChanges
      .pipe(takeUntil(this.destroy$))
      .subscribe(values => {
        console.log('Configuração da grid alterada:', values);
      });
  }

  private gerarDados(): DashboardData[] {
    const dados: DashboardData[] = [];
    const nomes = ['João Silva', 'Maria Santos', 'Pedro Costa', 'Ana Oliveira', 'Carlos Ferreira'];
    const enderecos = ['São Paulo', 'Rio de Janeiro', 'Belo Horizonte', 'Porto Alegre', 'Salvador'];
    
    for (let i = 1; i <= 5; i++) {
      const nomeAleatorio = nomes[Math.floor(Math.random() * nomes.length)];
      const enderecoAleatorio = enderecos[Math.floor(Math.random() * enderecos.length)];
      const idade = 18 + Math.floor(Math.random() * 50);
      
      dados.push({
        nome: nomeAleatorio,
        idade: idade.toString(),
        endereco: `${enderecoAleatorio}, ${i}`,
        descricao: `Meu nome é ${nomeAleatorio}, tenho ${idade} anos, morando em ${enderecoAleatorio}.`,
        marcado: Math.random() > 0.7,
        expandido: false
      });
    }
    
    return dados;
  }

  /**
   * Atualiza os dados do dashboard
   */
  atualizarDados(): void {
    this.carregarDados();
  }

  /**
   * Obtém estatísticas dos dados
   */
  obterEstatisticas(): { total: number; marcados: number; idades: { min: number; max: number; media: number } } {
    const total = this.minhaListaDeObjetos.length;
    const marcados = this.minhaListaDeObjetos.filter(item => item.marcado).length;
    
    const idades = this.minhaListaDeObjetos.map(item => parseInt(item.idade));
    const idadeMin = Math.min(...idades);
    const idadeMax = Math.max(...idades);
    const idadeMedia = idades.reduce((sum, idade) => sum + idade, 0) / idades.length;
    
    return {
      total,
      marcados,
      idades: {
        min: idadeMin,
        max: idadeMax,
        media: Math.round(idadeMedia)
      }
    };
  }

  /**
   * TrackBy function para performance da tabela
   */
  trackByFn(index: number, item: DashboardData): string {
    return `${item.nome}-${item.idade}-${index}`;
  }

  /**
   * Ações da tabela
   */
  verRegistro(registro: DashboardData): void {
    console.log('Visualizando registro:', registro);
    // Implementar lógica de visualização
  }

  editarRegistro(registro: DashboardData): void {
    console.log('Editando registro:', registro);
    // Implementar lógica de edição
  }

  onTableAction(event: { action: string, row: any }): void {
    console.log('Ação da tabela:', event.action, 'Registro:', event.row);
  }
}