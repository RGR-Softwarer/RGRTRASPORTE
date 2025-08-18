import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService } from '../../../../services/http/api.service';
import { ConfigService } from '../../../../services/config/config.service';
import { Viagem } from '../../../../dominio/entidade/viagem';
import { 
  ApiResponse, 
  ApiListResponse, 
  ApiCreateResponse, 
  ApiUpdateResponse,
  ApiDeleteResponse 
} from '../../../../dominio/interface/grid/api-response';

export interface ViagemFilter {
  dataInicio?: Date;
  dataFim?: Date;
  localidadeOrigemId?: number;
  localidadeDestinoId?: number;
  ativo?: boolean;
}

export interface ViagemSearchParams extends ViagemFilter {
  page?: number;
  pageSize?: number;
  sortBy?: string;
  sortDirection?: 'asc' | 'desc';
}

export interface CriarViagemCommand {
  dataViagem: Date;
  horarioSaida: string; // TimeSpan como string
  horarioChegada: string; // TimeSpan como string
  veiculoId: number;
  motoristaId: number;
  localidadeOrigemId: number;
  localidadeDestinoId: number;
  quantidadeVagas: number;
  distancia?: number;
  descricaoViagem?: string;
  polilinhaRota?: string;
  ativo: boolean;
  gatilhoViagemId?: number;
}

export interface EditarViagemCommand {
  id: number;
  dataViagem: Date;
  horarioSaida: string;
  horarioChegada: string;
  veiculoId: number;
  localidadeOrigemId: number;
  localidadeDestinoId: number;
  quantidadeVagas: number;
  ativo: boolean;
  gatilhoViagemId?: number;
}

export interface CancelarViagemCommand {
  id: number;
  motivo?: string;
}

export interface IniciarViagemCommand {
  id: number;
}

export interface FinalizarViagemCommand {
  id: number;
}

@Injectable({
  providedIn: 'root'
})
export class ViagemApiService {
  private readonly baseUrl: string;

  constructor(
    private apiService: ApiService,
    private configService: ConfigService
  ) { 
    this.baseUrl = `${this.configService.getApiBaseUrl()}/Viagem`;
  }

  // Buscar todas as viagens
  buscarTodos(params?: ViagemSearchParams): Observable<Viagem[]> {
    const queryParams = new URLSearchParams();
    if (params?.dataInicio) queryParams.append('DataInicio', params.dataInicio.toISOString());
    if (params?.dataFim) queryParams.append('DataFim', params.dataFim.toISOString());
    if (params?.localidadeOrigemId) queryParams.append('LocalidadeOrigemId', params.localidadeOrigemId.toString());
    if (params?.localidadeDestinoId) queryParams.append('LocalidadeDestinoId', params.localidadeDestinoId.toString());
    if (params?.ativo !== undefined) queryParams.append('Ativo', params.ativo.toString());
    
    const url = queryParams.toString() ? `${this.baseUrl}?${queryParams}` : this.baseUrl;
    return this.apiService.getList<Viagem>(url).pipe(
      map(response => response.data || [])
    );
  }

  // Buscar viagem por ID
  buscarPorId(id: number, auditado: boolean = false): Observable<Viagem> {
    if (auditado) {
      const url = `${this.baseUrl}/${id}?auditado=true`;
      return this.apiService.get<Viagem>(url).pipe(
        map(response => response.data)
      );
    }
    return this.apiService.getById<Viagem>(this.baseUrl, id).pipe(
      map(response => response.data)
    );
  }

  // Criar nova viagem
  criar(viagem: CriarViagemCommand): Observable<Viagem> {
    return this.apiService.create<Viagem>(this.baseUrl, viagem).pipe(
      map(response => response.data)
    );
  }

  // Atualizar viagem existente
  atualizar(id: number, viagem: EditarViagemCommand): Observable<Viagem> {
    return this.apiService.update<Viagem>(this.baseUrl, id, viagem).pipe(
      map(response => response.data)
    );
  }

  // Salvar (criar ou atualizar)
  salvar(viagem: Viagem): Observable<Viagem> {
    if (viagem.id) {
      const editarCommand: EditarViagemCommand = {
        id: viagem.id,
        dataViagem: viagem.dataViagem!,
        horarioSaida: viagem.horarioSaida!,
        horarioChegada: viagem.horarioChegada!,
        veiculoId: viagem.veiculoId!,
        localidadeOrigemId: viagem.localidadeOrigemId!,
        localidadeDestinoId: viagem.localidadeDestinoId!,
        quantidadeVagas: viagem.quantidadeVagas!,
        ativo: viagem.ativo!,
        gatilhoViagemId: viagem.gatilhoViagemId
      };
      return this.atualizar(viagem.id, editarCommand);
    }
    
    const criarCommand: CriarViagemCommand = {
      dataViagem: viagem.dataViagem!,
      horarioSaida: viagem.horarioSaida!,
      horarioChegada: viagem.horarioChegada!,
      veiculoId: viagem.veiculoId!,
      motoristaId: viagem.motoristaId!,
      localidadeOrigemId: viagem.localidadeOrigemId!,
      localidadeDestinoId: viagem.localidadeDestinoId!,
      quantidadeVagas: viagem.quantidadeVagas!,
      distancia: viagem.distancia,
      descricaoViagem: viagem.descricaoViagem,
      polilinhaRota: viagem.polilinhaRota,
      ativo: viagem.ativo!,
      gatilhoViagemId: viagem.gatilhoViagemId
    };
    return this.criar(criarCommand);
  }

  // Cancelar viagem
  cancelar(id: number, motivo?: string): Observable<any> {
    const url = `${this.baseUrl}/${id}/cancelar`;
    const command: CancelarViagemCommand = { id, motivo };
    return this.apiService.put<any>(url, command).pipe(
      map(response => response.data)
    );
  }

  // Iniciar viagem
  iniciar(id: number): Observable<any> {
    const url = `${this.baseUrl}/${id}/iniciar`;
    const command: IniciarViagemCommand = { id };
    return this.apiService.put<any>(url, command).pipe(
      map(response => response.data)
    );
  }

  // Finalizar viagem
  finalizar(id: number): Observable<any> {
    const url = `${this.baseUrl}/${id}/finalizar`;
    const command: FinalizarViagemCommand = { id };
    return this.apiService.put<any>(url, command).pipe(
      map(response => response.data)
    );
  }

  // Obter rota da viagem
  obterRotaViagem(id: number): Observable<any> {
    const url = `${this.baseUrl}/ObterRotaViagem/${id}`;
    return this.apiService.get<any>(url).pipe(
      map(response => response.data)
    );
  }

  // Buscar com filtros
  buscarComFiltros(filters: ViagemFilter): Observable<Viagem[]> {
    const params = new URLSearchParams();
    Object.entries(filters).forEach(([key, value]) => {
      if (value !== undefined && value !== null && value !== '') {
        if (value instanceof Date) {
          params.append(key, value.toISOString());
        } else {
          params.append(key, value.toString());
        }
      }
    });
    
    const url = `${this.baseUrl}?${params.toString()}`;
    return this.apiService.getList<Viagem>(url).pipe(
      map(response => response.data || [])
    );
  }
}