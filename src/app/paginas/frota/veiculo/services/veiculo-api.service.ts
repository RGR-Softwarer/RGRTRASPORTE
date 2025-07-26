import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService } from '../../../../services/http/api.service';
import { ConfigService } from '../../../../services/config/config.service';
import { Veiculo } from '../../../../dominio/entidade/veiculo';
import { 
  ApiResponse, 
  ApiListResponse, 
  ApiCreateResponse, 
  ApiUpdateResponse,
  ApiDeleteResponse 
} from '../../../../dominio/interface/grid/api-response';

export interface VeiculoFilter {
  placa?: string;
  marca?: string;
  modelo?: string;
  status?: string;
  ativo?: boolean;
}

export interface VeiculoSearchParams extends VeiculoFilter {
  page?: number;
  pageSize?: number;
  sortBy?: string;
  sortDirection?: 'asc' | 'desc';
}

@Injectable({
  providedIn: 'root'
})
export class VeiculoApiService {
  private readonly baseUrl: string;

  constructor(
    private apiService: ApiService,
    private configService: ConfigService
  ) { 
    this.baseUrl = `${this.configService.getApiBaseUrl()}/Veiculo`;
  }

  // Buscar todos os veículos com paginação
  buscarTodos(params?: VeiculoSearchParams): Observable<Veiculo[]> {
    return this.apiService.getList<Veiculo>(this.baseUrl).pipe(
      map(response => response.data || [])
    );
  }

  // Buscar veículo por ID
  buscarPorId(id: number): Observable<Veiculo> {
    return this.apiService.getById<Veiculo>(this.baseUrl, id).pipe(
      map(response => response.data)
    );
  }

  // Criar novo veículo
  criar(veiculo: Omit<Veiculo, 'id'>): Observable<Veiculo> {
    return this.apiService.create<Veiculo>(this.baseUrl, veiculo).pipe(
      map(response => response.data)
    );
  }

  // Atualizar veículo existente
  atualizar(id: number, veiculo: Partial<Veiculo>): Observable<Veiculo> {
    return this.apiService.update<Veiculo>(this.baseUrl, id, veiculo).pipe(
      map(response => response.data)
    );
  }

  // Salvar (criar ou atualizar)
  salvar(veiculo: Veiculo): Observable<Veiculo> {
    if (veiculo.id) {
      return this.atualizar(veiculo.id, veiculo);
    }
    return this.criar(veiculo);
  }

  // Deletar veículo
  deletar(id: number): Observable<ApiDeleteResponse> {
    return this.apiService.remove(this.baseUrl, id);
  }

  // Buscar com filtros avançados
  buscarComFiltros(filters: VeiculoFilter): Observable<Veiculo[]> {
    const params = new URLSearchParams();
    Object.entries(filters).forEach(([key, value]) => {
      if (value !== undefined && value !== null && value !== '') {
        params.append(key, value.toString());
      }
    });
    
    const url = `${this.baseUrl}/filtros?${params.toString()}`;
    return this.apiService.getList<Veiculo>(url).pipe(
      map(response => response.data || [])
    );
  }
} 