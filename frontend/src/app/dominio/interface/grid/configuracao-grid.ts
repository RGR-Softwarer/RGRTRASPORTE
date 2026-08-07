export type RolagemTabela = 'unset' | 'scroll' | 'fixed';
export type TamanhoTabela = 'small' | 'middle' | 'default';
export type LayoutTabela = 'auto' | 'fixed';
export type PosicaoPaginacao = 'top' | 'bottom';
export type TipoPaginacao = 'default' | 'simple';

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
  tamanho: TamanhoTabela;
  rolagemTabela: RolagemTabela;
  layoutTabela: LayoutTabela;
  posicao: PosicaoPaginacao;
  tipoPaginacao: TipoPaginacao;
  tituloTabela: string;
  rodapeTabela: string;
  adicionar: boolean;
  action: boolean;
}