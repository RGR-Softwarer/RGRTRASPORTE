export interface FiltroGrid {
    campo: string;
    valor: string;
}

export interface ParametrosBusca {
    filtros: FiltroGrid[];
    paginaAtual: number;
    tamanhoPagina: number;
    campoOrdenacao: string;
    descendente: boolean;
} 