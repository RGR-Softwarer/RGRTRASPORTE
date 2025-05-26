import { Router } from '@angular/router';
import { AppContextService } from './../context/app.context';
import { AppContext } from './../../dominio/entidade/app.context';
import { ApiService } from './../http/api.service';
import { Injectable } from '@angular/core';
import { catchError, map } from 'rxjs/operators';
import { of, throwError } from 'rxjs';
import { ToastService } from '../utils/notificacao/toast.service';
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
    private toastService: ToastService, 
    private appContextService: AppContextService, 
    private router: Router
  ) { }

  public authenticate(email: string, senha: string): void {
    this.apiService.post<LoginResponse>(ClienteUrls.Login, { email, senha })
    .pipe(
      map((response) => {
        if (response.sucesso) {
          this.toastService.exibirMensagemSucesso('Sucesso', 'Login bem-sucedido');
          this.appContextService.salvaAppContext(new AppContext(email, response.dados.token));
          this.router.navigate(['']);
          return true;
        }
        return false;
      }),
      catchError(this.handleError)
    )
    .subscribe();
  }

  private handleError = (error: any) => {
    let errorMessage = 'Algo deu errado; por favor tente novamente mais tarde.';
    if (error.status === 401) {
      errorMessage = 'Senha Inválida';
      this.toastService.exibirMensagemErro('Erro', 'Senha Inválida');
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
