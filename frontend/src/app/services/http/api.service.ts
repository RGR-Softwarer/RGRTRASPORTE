import { Injectable } from '@angular/core';
import { HttpService } from './http.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { 
  ApiResponse, 
  ApiListResponse, 
  ApiCreateResponse, 
  ApiUpdateResponse, 
  ApiDeleteResponse 
} from '../../dominio/interface/grid/api-response';

@Injectable({
  providedIn: 'root'
})
export class ApiService extends HttpService {

  constructor(http: HttpClient) {
    super(http);
  }

  // Buscar lista com paginação
  getList<T>(url: string, params?: HttpParams): Observable<ApiListResponse<T>> {
    return super.get<ApiListResponse<T>>(url, params);
  }

  // Buscar item único
  getById<T>(url: string, id: string | number): Observable<ApiResponse<T>> {
    return super.get<ApiResponse<T>>(`${url}/${id}`);
  }

  // Criar novo item
  create<T>(url: string, data: Partial<T>): Observable<ApiCreateResponse<T>> {
    return super.post<ApiCreateResponse<T>>(url, data);
  }

  // Atualizar item existente
  update<T>(url: string, id: string | number, data: Partial<T>): Observable<ApiUpdateResponse<T>> {
    return super.put<ApiUpdateResponse<T>>(`${url}/${id}`, data);
  }

  // Deletar item
  remove(url: string, id: string | number): Observable<ApiDeleteResponse> {
    return super.delete<ApiDeleteResponse>(`${url}/${id}`);
  }

  // Métodos genéricos mantidos para compatibilidade
  override get<T, R = ApiResponse<T>>(url: string, params?: HttpParams): Observable<R> {
    return super.get(url, params);
  }

  override post<T, R = ApiResponse<T>>(url: string, data: any): Observable<R> {
    return super.post(url, data);
  }

  override put<T, R = ApiResponse<T>>(url: string, data: any): Observable<R> {
    return super.put(url, data);
  }

  override delete<T, R = ApiResponse<T>>(url: string): Observable<R> {
    return super.delete(url);
  }

  override patch<T, R = ApiResponse<T>>(url: string, data: any): Observable<R> {
    return super.patch(url, data);
  }
}
