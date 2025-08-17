import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService } from '../../../../services/http/api.service';
import { ConfigService } from '../../../../services/config/config.service';
import { ModeloVeicular } from '../../../../dominio/entidade/veiculo/modelo-veicular';
import { TipoModeloVeiculoEnum } from '../../../../dominio/enum/veiculo/tipo-modelo-veiculo-enum';
import { 
  ApiResponse, 
  ApiListResponse, 
  ApiCreateResponse, 
  ApiUpdateResponse,
  ApiDeleteResponse 
} from '../../../../dominio/interface/grid/api-response';

export interface ModeloVeicularFilter {
  descricaoFiltro?: string;
  tipoFiltro?: TipoModeloVeiculoEnum;
  ativoFiltro?: boolean;
}

export interface ModeloVeicularSearchParams extends ModeloVeicularFilter {
  pagina?: number;
  tamanhoPagina?: number;
  sortBy?: string;
  sortDirection?: 'asc' | 'desc';
}

export interface ModeloVeicularPaginatedQuery {
  filtros?: Array<{campo: string, valor: string}>;
  paginaAtual?: number;
  tamanhoPagina?: number;
  campoOrdenacao?: string;
  descendente?: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class ModeloVeicularApiService {
  private readonly baseUrl: string;

  constructor(
    private apiService: ApiService,
    private configService: ConfigService
  ) { 
    this.baseUrl = `${this.configService.getApiBaseUrl()}/ModeloVeicular`;
  }

  // Buscar todos os modelos veiculares com paginação
  buscarTodos(params?: ModeloVeicularSearchParams): Observable<ModeloVeicular[]> {
    const queryParams = new URLSearchParams();
    if (params?.descricaoFiltro) queryParams.append('DescricaoFiltro', params.descricaoFiltro);
    if (params?.tipoFiltro) queryParams.append('TipoFiltro', params.tipoFiltro);
    if (params?.ativoFiltro !== undefined) queryParams.append('AtivoFiltro', params.ativoFiltro.toString());
    if (params?.pagina) queryParams.append('Pagina', params.pagina.toString());
    if (params?.tamanhoPagina) queryParams.append('TamanhoPagina', params.tamanhoPagina.toString());
    
    const url = queryParams.toString() ? `${this.baseUrl}?${queryParams}` : this.baseUrl;
    return this.apiService.getList<ModeloVeicular>(url).pipe(
      map(response => response.data || [])
    );
  }

  // Buscar modelos com filtros avançados (usando POST /filtrar)
  buscarComFiltros(query: ModeloVeicularPaginatedQuery): Observable<any> {
    const url = `${this.baseUrl}/filtrar`;
    return this.apiService.post<any>(url, query).pipe(
      map(response => response.data || { items: [], totalCount: 0 })
    );
  }

  // Buscar modelo veicular por ID
  buscarPorId(id: number): Observable<ModeloVeicular> {
    return this.apiService.getById<ModeloVeicular>(this.baseUrl, id).pipe(
      map(response => response.data)
    );
  }

  // Criar novo modelo veicular
  criar(modelo: Omit<ModeloVeicular, 'id'>): Observable<ModeloVeicular> {
    return this.apiService.create<ModeloVeicular>(this.baseUrl, modelo).pipe(
      map(response => response.data)
    );
  }

  // Atualizar modelo veicular existente
  atualizar(id: number, modelo: Partial<ModeloVeicular>): Observable<ModeloVeicular> {
    return this.apiService.update<ModeloVeicular>(this.baseUrl, id, modelo).pipe(
      map(response => response.data)
    );
  }

  // Salvar (criar ou atualizar)
  salvar(modelo: ModeloVeicular): Observable<ModeloVeicular> {
    if (modelo.id) {
      return this.atualizar(modelo.id, modelo);
    }
    return this.criar(modelo);
  }

  // Deletar modelo veicular
  deletar(id: number): Observable<ApiDeleteResponse> {
    return this.apiService.remove(this.baseUrl, id);
  }

  // Seed - popular dados iniciais
  seed(forcarRecriacao: boolean = false): Observable<any> {
    const url = `${this.baseUrl}/seed`;
    return this.apiService.post<any>(url, { forcarRecriacao }).pipe(
      map(response => response.data)
    );
  }
}