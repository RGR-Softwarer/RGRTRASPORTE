import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { GridItem } from './grid.service';
import { FiltroGrid, ParametrosBusca } from '../../dominio/interface/grid/filtros-grid';
import { ConfiguracaoGrid } from '../../dominio/interface/grid/configuracao-grid';

export interface GridEstado {
  dados: GridItem[];
  dadosFiltrados: GridItem[];
  carregando: boolean;
  paginaAtual: number;
  tamanhoPagina: number;
  totalRegistros: number;
  filtros: FiltroGrid[];
  ultimaConsulta: ParametrosBusca | null;
  configuracoes: ConfiguracaoGrid;
  colunasVisiveis: string[];
  ordenacao: {
    campo: string;
    direcao: 'ascend' | 'descend' | null;
  };
}

@Injectable({
  providedIn: 'root'
})
export class EstadoGridService {
  
  private estadoSubject = new BehaviorSubject<GridEstado>({
    dados: [],
    dadosFiltrados: [],
    carregando: false,
    paginaAtual: 1,
    tamanhoPagina: 10,
    totalRegistros: 0,
    filtros: [],
    ultimaConsulta: null,
    configuracoes: {
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
    },
    colunasVisiveis: [],
    ordenacao: {
      campo: '',
      direcao: null
    }
  });

  public estado$: Observable<GridEstado> = this.estadoSubject.asObservable();

  /**
   * Obtém o estado atual
   */
  getEstado(): GridEstado {
    return this.estadoSubject.value;
  }

  /**
   * Define os dados da grid
   */
  setDados(dados: GridItem[]): void {
    const estadoAtual = this.estadoSubject.value;
    this.estadoSubject.next({
      ...estadoAtual,
      dados,
      dadosFiltrados: dados
    });
  }

  /**
   * Define os dados filtrados
   */
  setDadosFiltrados(dadosFiltrados: GridItem[]): void {
    const estadoAtual = this.estadoSubject.value;
    this.estadoSubject.next({
      ...estadoAtual,
      dadosFiltrados
    });
  }

  /**
   * Define o estado de carregamento
   */
  setCarregando(carregando: boolean): void {
    const estadoAtual = this.estadoSubject.value;
    this.estadoSubject.next({
      ...estadoAtual,
      carregando
    });
  }

  /**
   * Define a página atual
   */
  setPaginaAtual(paginaAtual: number): void {
    const estadoAtual = this.estadoSubject.value;
    this.estadoSubject.next({
      ...estadoAtual,
      paginaAtual
    });
  }

  /**
   * Define o tamanho da página
   */
  setTamanhoPagina(tamanhoPagina: number): void {
    const estadoAtual = this.estadoSubject.value;
    this.estadoSubject.next({
      ...estadoAtual,
      tamanhoPagina
    });
  }

  /**
   * Define o total de registros
   */
  setTotalRegistros(totalRegistros: number): void {
    const estadoAtual = this.estadoSubject.value;
    this.estadoSubject.next({
      ...estadoAtual,
      totalRegistros
    });
  }

  /**
   * Define os filtros
   */
  setFiltros(filtros: FiltroGrid[]): void {
    const estadoAtual = this.estadoSubject.value;
    this.estadoSubject.next({
      ...estadoAtual,
      filtros
    });
  }

  /**
   * Define a última consulta
   */
  setUltimaConsulta(ultimaConsulta: ParametrosBusca | null): void {
    const estadoAtual = this.estadoSubject.value;
    this.estadoSubject.next({
      ...estadoAtual,
      ultimaConsulta
    });
  }

  /**
   * Define as configurações
   */
  setConfiguracoes(configuracoes: Partial<ConfiguracaoGrid>): void {
    const estadoAtual = this.estadoSubject.value;
    this.estadoSubject.next({
      ...estadoAtual,
      configuracoes: {
        ...estadoAtual.configuracoes,
        ...configuracoes
      }
    });
  }

  /**
   * Define as colunas visíveis
   */
  setColunasVisiveis(colunasVisiveis: string[]): void {
    const estadoAtual = this.estadoSubject.value;
    this.estadoSubject.next({
      ...estadoAtual,
      colunasVisiveis
    });
  }

  /**
   * Define a ordenação
   */
  setOrdenacao(ordenacao: { campo: string; direcao: 'ascend' | 'descend' | null }): void {
    const estadoAtual = this.estadoSubject.value;
    this.estadoSubject.next({
      ...estadoAtual,
      ordenacao
    });
  }

  /**
   * Atualiza múltiplas propriedades do estado
   */
  atualizarEstado(updates: Partial<GridEstado>): void {
    const estadoAtual = this.estadoSubject.value;
    this.estadoSubject.next({
      ...estadoAtual,
      ...updates
    });
  }

  /**
   * Reseta o estado para os valores padrão
   */
  resetarEstado(): void {
    this.estadoSubject.next({
      dados: [],
      dadosFiltrados: [],
      carregando: false,
      paginaAtual: 1,
      tamanhoPagina: 10,
      totalRegistros: 0,
      filtros: [],
      ultimaConsulta: null,
      configuracoes: {
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
      },
      colunasVisiveis: [],
      ordenacao: {
        campo: '',
        direcao: null
      }
    });
  }

  /**
   * Adiciona um filtro
   */
  adicionarFiltro(filtro: FiltroGrid): void {
    const estadoAtual = this.estadoSubject.value;
    const filtrosExistentes = estadoAtual.filtros.filter(f => f.campo !== filtro.campo);
    this.setFiltros([...filtrosExistentes, filtro]);
  }

  /**
   * Remove um filtro
   */
  removerFiltro(campo: string): void {
    const estadoAtual = this.estadoSubject.value;
    const filtrosAtualizados = estadoAtual.filtros.filter(f => f.campo !== campo);
    this.setFiltros(filtrosAtualizados);
  }

  /**
   * Limpa todos os filtros
   */
  limparFiltros(): void {
    this.setFiltros([]);
  }

  /**
   * Verifica se está carregando
   */
  isCarregando(): boolean {
    return this.estadoSubject.value.carregando;
  }

  /**
   * Obtém os dados atuais
   */
  getDados(): GridItem[] {
    return this.estadoSubject.value.dados;
  }

  /**
   * Obtém os dados filtrados
   */
  getDadosFiltrados(): GridItem[] {
    return this.estadoSubject.value.dadosFiltrados;
  }

  /**
   * Obtém os filtros atuais
   */
  getFiltros(): FiltroGrid[] {
    return this.estadoSubject.value.filtros;
  }
} 