import { Router } from '@angular/router';
import { AppContextService } from './../context/app.context';
import { AppContext } from './../../dominio/entidade/app.context';
import { ApiService } from './../http/api.service';
import { Injectable } from '@angular/core';
import { catchError, map } from 'rxjs/operators';
import { of, throwError } from 'rxjs';
import { NotificationService } from '../../shared/services/notification.service';
import { ClienteUrls } from '../../dominio/enum/cliente-url-enum';
import { ApiResponse } from '../../dominio/interface/grid/api-response';
import { environment } from '../../../environments/environment';

interface LoginResponse {
  token: string;
}

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(
    private apiService: ApiService, 
    private notificationService: NotificationService, 
    private appContextService: AppContextService, 
    private router: Router
  ) { }

  public authenticate(email: string, senha: string): void {
    // Em ambiente local, permite login com qualquer usuário
    if (!environment.production) {
      this.simulateLocalLogin(email);
      return;
    }

    // Em produção, usa a autenticação real
    this.apiService.post<LoginResponse>(ClienteUrls.Login, { email, senha })
    .pipe(
      map((response) => {
        if (response.success) {
          this.notificationService.success('Sucesso', 'Login bem-sucedido');
          this.appContextService.salvaAppContext(new AppContext(email, response.data.token, ""));
          this.router.navigate(['']);
          return true;
        }
        return false;
      }),
      catchError(this.handleError)
    )
    .subscribe();
  }

  private simulateLocalLogin(email: string): void {
    // Simula uma resposta de sucesso para ambiente local
    const mockToken = this.generateMockToken();
    const mockResponse: ApiResponse<LoginResponse> = {
      success: true,
      data: {
        token: mockToken
      },
      message: 'Login simulado para ambiente local'
    };

    // Simula um pequeno delay para parecer real
    setTimeout(() => {
      this.notificationService.success('Sucesso', 'Login local bem-sucedido');
      this.appContextService.salvaAppContext(new AppContext(email, mockToken, ""));
      this.router.navigate(['']);
    }, 500);
  }

  private generateMockToken(): string {
    // Gera um token mock para ambiente local
    const timestamp = Date.now();
    const randomString = Math.random().toString(36).substring(2);
    return `local_token_${timestamp}_${randomString}`;
  }

  private handleError = (error: any) => {
    let errorMessage = 'Algo deu errado; por favor tente novamente mais tarde.';
    if (error.status === 401) {
      errorMessage = 'Senha Inválida';
      this.notificationService.error('Erro', 'Senha Inválida');
      return of(false);
    }
    if (error.error instanceof ErrorEvent) {
      console.error('Ocorreu um erro:', error.error.message);
    } else {
      console.error(`Backend retornou código ${error.status}, body was: `, error.error);
    }
    return throwError(() => new Error(errorMessage));
  }
}
