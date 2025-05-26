export interface ApiResponse<T> {
    sucesso: boolean;
    dados: T;
    mensagem?: string;
} 