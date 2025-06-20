import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService } from '../../../../services/http/api.service';
import { Veiculo } from '../../../../dominio/entidade/veiculo';
import { environment } from '../../../../../environments/environment';
import { ApiResponse } from '../../../../dominio/interface/grid/api-response';

@Injectable({
  providedIn: 'root'
})
export class VeiculoApiService {
  private readonly baseUrl = `${environment.apiBaseUrl}/Veiculo`;

  constructor(private apiService: ApiService) { }

  buscarTodos(): Observable<Veiculo[]> {
    return this.apiService.get<Veiculo[]>(this.baseUrl).pipe(
        map(response => (response as any).dados || (response as any).data || response)
    );
  }

  buscarPorId(id: number): Observable<Veiculo> {
    return this.apiService.get<Veiculo>(`${this.baseUrl}/${id}`).pipe(
        map(response => (response as any).dados || (response as any).data || response)
    );
  }

  salvar(veiculo: Veiculo): Observable<Veiculo> {
    if (veiculo.id) {
      return this.apiService.put<Veiculo>(`${this.baseUrl}/${veiculo.id}`, veiculo).pipe(
        map(response => (response as any).dados || (response as any).data || response)
      );
    }
    return this.apiService.post<Veiculo>(this.baseUrl, veiculo).pipe(
        map(response => (response as any).dados || (response as any).data || response)
    );
  }

  deletar(id: number): Observable<any> {
    return this.apiService.delete<any>(`${this.baseUrl}/${id}`);
  }
} 