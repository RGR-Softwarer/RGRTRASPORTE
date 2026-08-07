import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  private headers: HttpHeaders;

  constructor(private http: HttpClient) { 
    this.headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
  }
 
  public get<T, R = T>(url: string, params?: HttpParams): Observable<R> {
    return this.http.get<R>(url, { headers: this.headers, params });
  }

  public post<T, R = T>(url: string, data: any): Observable<R> {
    return this.http.post<R>(url, data, { headers: this.headers });
  }

  public put<T, R = T>(url: string, data: any): Observable<R> {
    return this.http.put<R>(url, data, { headers: this.headers });
  }

  public delete<T, R = T>(url: string): Observable<R> {
    return this.http.delete<R>(url, { headers: this.headers });
  }

  public patch<T, R = T>(url: string, data: any): Observable<R> {
    return this.http.patch<R>(url, data, { headers: this.headers });
  }

  public getHeaders(): HttpHeaders {
    return this.headers;
  }

  public addHeader(key: string, value: string | string[]): HttpService {
    this.headers = this.headers.append(key, value);
    return this;
  }
}