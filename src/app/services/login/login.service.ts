import { Router } from '@angular/router';
import { AppContextService } from './../context/app.context';
import { AppContext } from './../../dominio/entidade/app.context';
import { ApiService } from './../http/api.service';
import { Injectable } from '@angular/core';
import { catchError, map, first } from 'rxjs/operators';
import { of, throwError } from 'rxjs';
import { NotificationService } from '../../shared/services/notification.service';
import { ClienteUrls } from '../../dominio/enum/cliente-url-enum';
import { ApiResponse } from '../../dominio/interface/grid/api-response';

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

  public authenticate(email: string, senha: string): Promise<boolean> {
    return new Promise((resolve, reject) => {
      this.apiService.post<any>(ClienteUrls.Login, { email, senha })
        .pipe(
          first(),
          map((response) => {
            // Processar resposta do backend (RetornoGenericoDto com 'dados' e 'sucesso')
            const responseData = response as any;
            const isSuccess = responseData.sucesso || responseData.success;
            const loginData = responseData.dados || responseData.data;
            
            if (isSuccess && loginData) {
              // Extrair token e informações do usuário
              const token = loginData.token || loginData.Token;
              const user = loginData.user || loginData.User;
              const nome = user?.nome || user?.Nome || email;
              
              if (token) {
                this.notificationService.success('Sucesso', 'Login bem-sucedido');
                this.appContextService.salvaAppContext(new AppContext(email, token, nome));
                this.router.navigate(['']);
                resolve(true);
                return true;
              }
            }
            
            const error = new Error('Login falhou - resposta inválida');
            reject(error);
            return false;
          }),
          catchError((error) => {
            this.handleError(error);
            reject(error);
            return throwError(() => error);
          })
        )
        .subscribe({
          error: (err) => reject(err)
        });
    });
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
