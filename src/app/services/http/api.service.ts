import { Injectable } from '@angular/core';
import { HttpService } from './http.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService extends HttpService {

  constructor(http: HttpClient) {
    super(http);
    this.addHeader('Authorization', environment.authToken);
   }
}
