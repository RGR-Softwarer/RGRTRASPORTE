import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../../../../services/http/api.service';
import { Motorista } from '../../../../dominio/entidade/motorista';
import { TrasportadorUrlEnum } from '../../../../dominio/enum/trasportador-url-enum';

@Injectable({ providedIn: 'root' })
export class MotoristaApiService {
    private readonly baseUrl = TrasportadorUrlEnum.MOTORISTAS;

    constructor(private apiService: ApiService) {}

    obterTodos(): Observable<any> {
        return this.apiService.get<any>(this.baseUrl);
    }

    obterPorId(id: number): Observable<any> {
        return this.apiService.get<any>(`${this.baseUrl}/${id}`);
    }

    criar(motorista: Partial<Motorista>): Observable<any> {
        return this.apiService.post<any>(this.baseUrl, motorista);
    }

    atualizar(id: number, motorista: Partial<Motorista>): Observable<any> {
        return this.apiService.put<any>(`${this.baseUrl}/${id}`, motorista);
    }

    remover(id: number): Observable<any> {
        return this.apiService.delete<any>(`${this.baseUrl}/${id}`);
    }
}







