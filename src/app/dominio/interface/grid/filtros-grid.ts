export interface FiltroGrid {
    campo: string;
    valor: string;
    operador?: string; // 'contains', 'equals', 'in', etc.
}

export interface ParametrosBusca {
    filtros: FiltroGrid[];
    paginaAtual: number;
    tamanhoPagina: number;
    campoOrdenacao: string;
    descendente: boolean;
} 