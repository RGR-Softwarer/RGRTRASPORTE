export interface ApiResponse<T> {
    sucesso: boolean;
    dados: T;
    mensagem?: string;
}

// Interface específica para resposta de grid paginado
export interface GridResponse<T> {
    items: T[];
    total: number;
    pagina: number;
    tamanhoPagina: number;
}

// Tipo para resposta de grid através da ApiResponse
export type ApiGridResponse<T> = ApiResponse<GridResponse<T>>; 