import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

export interface AppConfig {
  apiBaseUrl: string;
  apiTrasportador: string;
  authToken: string | null;
  production: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class ConfigService {
  private readonly config: AppConfig = {
    apiBaseUrl: environment.apiBaseUrl,
    apiTrasportador: environment.apiTrasportador,
    authToken: environment.authToken,
    production: environment.production
  };

  getApiBaseUrl(): string {
    return this.config.apiBaseUrl;
  }

  getApiTrasportadorUrl(): string {
    return this.config.apiTrasportador;
  }

  getAuthToken(): string | null {
    return this.config.authToken || this.getStoredToken();
  }

  setAuthToken(token: string | null): void {
    this.config.authToken = token;
    if (token) {
      localStorage.setItem('authToken', token);
    } else {
      localStorage.removeItem('authToken');
    }
  }

  isProduction(): boolean {
    return this.config.production;
  }

  private getStoredToken(): string | null {
    return localStorage.getItem('authToken');
  }

  clearConfig(): void {
    this.setAuthToken(null);
  }
} 