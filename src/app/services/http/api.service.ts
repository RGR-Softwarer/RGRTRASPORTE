import { Injectable } from '@angular/core';
import { HttpService } from './http.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../dominio/interface/grid/api-response';

@Injectable({
  providedIn: 'root'
})
export class ApiService extends HttpService {

  constructor(http: HttpClient) {
    super(http);
    this.addHeader('Authorization', environment.authToken);
  }

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
