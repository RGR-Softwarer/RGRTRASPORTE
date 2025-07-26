export interface ApiResponse<T = any> {
  success: boolean;
  data: T;
  message?: string;
  errors?: string[];
  pagination?: PaginationInfo;
  timestamp?: string;
}

export interface PaginationInfo {
  currentPage: number;
  totalPages: number;
  pageSize: number;
  totalItems: number;
  hasNext: boolean;
  hasPrevious: boolean;
}

export interface ApiError {
  code: string;
  message: string;
  field?: string;
  details?: Record<string, any>;
}

export interface ApiListResponse<T> extends ApiResponse<T[]> {
  pagination: PaginationInfo;
}

export interface ApiCreateResponse<T> extends ApiResponse<T> {
  createdId: string | number;
}

export interface ApiUpdateResponse<T> extends ApiResponse<T> {
  updatedAt: string;
}

export interface ApiDeleteResponse {
  success: boolean;
  message: string;
  deletedId: string | number;
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