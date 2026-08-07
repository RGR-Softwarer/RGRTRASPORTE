import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../../../../services/http/api.service';
import { GatilhoViagem } from '../../../../dominio/entidade/gatilho-viagem';
import { TrasportadorUrlEnum } from '../../../../dominio/enum/trasportador-url-enum';

@Injectable({ providedIn: 'root' })
export class GatilhoViagemApiService {
    private readonly baseUrl = TrasportadorUrlEnum.GATILHOS_VIAGEM;

    constructor(private apiService: ApiService) {}

    obterTodos(): Observable<any> {
        return this.apiService.get<any>(this.baseUrl);
    }

    obterPorId(id: number): Observable<any> {
        return this.apiService.get<any>(`${this.baseUrl}/${id}`);
    }

    criar(gatilho: Partial<GatilhoViagem>): Observable<any> {
        return this.apiService.post<any>(this.baseUrl, gatilho);
    }

    atualizar(id: number, gatilho: Partial<GatilhoViagem>): Observable<any> {
        return this.apiService.put<any>(`${this.baseUrl}/${id}`, gatilho);
    }

    remover(id: number): Observable<any> {
        return this.apiService.delete<any>(`${this.baseUrl}/${id}`);
    }
}







