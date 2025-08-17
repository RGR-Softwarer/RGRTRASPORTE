import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService } from '../../../../services/http/api.service';
import { ConfigService } from '../../../../services/config/config.service';
import { Passageiro } from '../../../../dominio/entidade/passageiro';
import { SexoEnum } from '../../../../dominio/enum/sexo-enum';
import { 
  ApiResponse, 
  ApiListResponse, 
  ApiCreateResponse, 
  ApiUpdateResponse,
  ApiDeleteResponse 
} from '../../../../dominio/interface/grid/api-response';

export interface PassageiroFilter {
  nome?: string;
  cpf?: string;
  situacao?: boolean;
}

export interface PassageiroSearchParams extends PassageiroFilter {
  page?: number;
  pageSize?: number;
  sortBy?: string;
  sortDirection?: 'asc' | 'desc';
}

export interface PassageiroPaginatedQuery {
  filtros?: Array<{campo: string, valor: string}>;
  paginaAtual?: number;
  tamanhoPagina?: number;
  campoOrdenacao?: string;
  descendente?: boolean;
}

export interface ObterPassageirosQuery {
  nome?: string;
  cpf?: string;
  situacao?: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class PassageiroApiService {
  private readonly baseUrl: string;

  constructor(
    private apiService: ApiService,
    private configService: ConfigService
  ) { 
    this.baseUrl = `${this.configService.getApiBaseUrl()}/Passageiro`;
  }

  // Buscar todos os passageiros (usando GET com body)
  buscarTodos(query?: ObterPassageirosQuery): Observable<Passageiro[]> {
    return this.apiService.get<Passageiro[]>(this.baseUrl, query || {}).pipe(
      map(response => response.data || [])
    );
  }

  // Buscar passageiros com filtros avançados (usando POST /filtrar)
  buscarComFiltros(query: PassageiroPaginatedQuery): Observable<any> {
    const url = `${this.baseUrl}/filtrar`;
    return this.apiService.post<any>(url, query).pipe(
      map(response => response.data || { items: [], totalCount: 0 })
    );
  }

  // Buscar passageiro por ID
  buscarPorId(id: number): Observable<Passageiro> {
    return this.apiService.getById<Passageiro>(this.baseUrl, id).pipe(
      map(response => response.data)
    );
  }

  // Criar novo passageiro
  criar(passageiro: Omit<Passageiro, 'id'>): Observable<Passageiro> {
    return this.apiService.create<Passageiro>(this.baseUrl, passageiro).pipe(
      map(response => response.data)
    );
  }

  // Atualizar passageiro existente
  atualizar(id: number, passageiro: Partial<Passageiro>): Observable<Passageiro> {
    return this.apiService.update<Passageiro>(this.baseUrl, id, passageiro).pipe(
      map(response => response.data)
    );
  }

  // Salvar (criar ou atualizar)
  salvar(passageiro: Passageiro): Observable<Passageiro> {
    if (passageiro.id) {
      return this.atualizar(passageiro.id, passageiro);
    }
    return this.criar(passageiro);
  }

  // Deletar passageiro
  deletar(id: number): Observable<ApiDeleteResponse> {
    return this.apiService.remove(this.baseUrl, id);
  }

  // Buscar passageiros com filtros simples
  buscarComFiltrosSimples(filters: PassageiroFilter): Observable<Passageiro[]> {
    const query: ObterPassageirosQuery = {};
    if (filters.nome) query.nome = filters.nome;
    if (filters.cpf) query.cpf = filters.cpf;
    if (filters.situacao !== undefined) query.situacao = filters.situacao;
    
    return this.buscarTodos(query);
  }
}