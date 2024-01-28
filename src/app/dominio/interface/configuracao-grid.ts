import { NzTableLayout, NzTablePaginationPosition, NzTablePaginationType, NzTableSize } from 'ng-zorro-antd/table';

export type RolagemTabela = 'unset' | 'scroll' | 'fixed';

export interface ConfiguracaoGrid {
comBorda: boolean;
  carregando: boolean;
  paginacao: boolean;
  alteradorTamanho: boolean;
  titulo: boolean;
  cabecalho: boolean;
  rodape: boolean;
  expansivel: boolean;
  caixaSelecao: boolean;
  cabecalhoFixo: boolean;
  semResultado: boolean;
  elipse: boolean;
  simples: boolean;
  mostrarOpcoes: boolean;
  tamanho: NzTableSize;
  rolagemTabela: RolagemTabela;
  layoutTabela: NzTableLayout;
  posicao: NzTablePaginationPosition;
  tipoPaginacao: NzTablePaginationType;
}