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
 
  public get(url: string, params?: HttpParams): Observable<any> {
    return this.http.get(url, { headers: this.headers, params });
  }

  public post(url: string, data: any): Observable<any> {
    return this.http.post(url, data, { headers: this.headers });
  }

  public put(url: string, data: any): Observable<any> {
    return this.http.put(url, data, { headers: this.headers });
  }

  public delete(url: string): Observable<any> {
    return this.http.delete(url, { headers: this.headers });
  }

  public patch(url: string, data: any): Observable<any> {
    return this.http.patch(url, data, { headers: this.headers });
  }

  public getHeaders(): HttpHeaders {
    return this.headers;
  }

  public addHeader(key: string, value: string | string[]): HttpService {
    this.headers = this.headers.append(key, value);
    return this;
  }
}