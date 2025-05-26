export interface ResponseGrid<T> {
    items: T[];
    total: number;
    pagina: number;
    tamanhoPagina: number;
} 