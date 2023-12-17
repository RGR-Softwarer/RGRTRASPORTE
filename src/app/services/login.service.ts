import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  
  constructor(private http: HttpClient) {}

  authenticate(email: string, password: string, rememberMe: boolean): void {
    // Implement authentication logic here
    // This is just a placeholder for the actual authentication call
    console.log('Authenticating', { email, password, rememberMe });
  }
}
