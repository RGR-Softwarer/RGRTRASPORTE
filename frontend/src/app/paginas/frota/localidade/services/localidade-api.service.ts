import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService } from '../../../../services/http/api.service';
import { ConfigService } from '../../../../services/config/config.service';
import { Localidade } from '../../../../dominio/entidade/localidade';
import { 
  ApiResponse, 
  ApiListResponse, 
  ApiCreateResponse, 
  ApiUpdateResponse,
  ApiDeleteResponse 
} from '../../../../dominio/interface/grid/api-response';

export interface LocalidadeFilter {
  nome?: string;
  estado?: string;
  cidade?: string;
  ativo?: boolean;
}

export interface LocalidadeSearchParams extends LocalidadeFilter {
  page?: number;
  pageSize?: number;
  sortBy?: string;
  sortDirection?: 'asc' | 'desc';
}

@Injectable({
  providedIn: 'root'
})
export class LocalidadeApiService {
  private readonly baseUrl: string;

  constructor(
    private apiService: ApiService,
    private configService: ConfigService
  ) { 
    this.baseUrl = `${this.configService.getApiBaseUrl()}/Localidade`;
  }

  // Buscar todas as localidades
  buscarTodos(params?: LocalidadeSearchParams): Observable<Localidade[]> {
    const queryParams = new URLSearchParams();
    if (params?.nome) queryParams.append('nome', params.nome);
    if (params?.estado) queryParams.append('estado', params.estado);
    if (params?.ativo !== undefined) queryParams.append('ativo', params.ativo.toString());
    
    const url = queryParams.toString() ? `${this.baseUrl}?${queryParams}` : this.baseUrl;
    return this.apiService.getList<Localidade>(url).pipe(
      map(response => response.data || [])
    );
  }

  // Buscar localidade por ID
  buscarPorId(id: number): Observable<Localidade> {
    return this.apiService.getById<Localidade>(this.baseUrl, id).pipe(
      map(response => response.data)
    );
  }

  // Criar nova localidade
  criar(localidade: Omit<Localidade, 'id'>): Observable<Localidade> {
    return this.apiService.create<Localidade>(this.baseUrl, localidade).pipe(
      map(response => response.data)
    );
  }

  // Atualizar localidade existente
  atualizar(id: number, localidade: Partial<Localidade>): Observable<Localidade> {
    return this.apiService.update<Localidade>(this.baseUrl, id, localidade).pipe(
      map(response => response.data)
    );
  }

  // Salvar (criar ou atualizar)
  salvar(localidade: Localidade): Observable<Localidade> {
    if (localidade.id) {
      return this.atualizar(localidade.id, localidade);
    }
    return this.criar(localidade);
  }

  // Deletar localidade
  deletar(id: number): Observable<ApiDeleteResponse> {
    return this.apiService.remove(this.baseUrl, id);
  }

  // Buscar com filtros avançados
  buscarComFiltros(filters: LocalidadeFilter): Observable<Localidade[]> {
    const params = new URLSearchParams();
    Object.entries(filters).forEach(([key, value]) => {
      if (value !== undefined && value !== null && value !== '') {
        params.append(key, value.toString());
      }
    });
    
    const url = `${this.baseUrl}?${params.toString()}`;
    return this.apiService.getList<Localidade>(url).pipe(
      map(response => response.data || [])
    );
  }
}