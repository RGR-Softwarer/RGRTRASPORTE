import { Router } from '@angular/router';
import { AppContextService } from './../context/app.context';
import { AppContext } from './../../dominio/entidade/app.context';
import { ApiService } from './../http/api.service';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { of, throwError } from 'rxjs';
import { ToastService } from '../utils/notificacao/toast.service';
import { ClienteUrls } from '../../dominio/enum/cliente-url-enum';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private apiService: ApiService, private toastService: ToastService, private AppContextService: AppContextService, private router: Router) { }

  public authenticate(email: string, senha: string): boolean {
    this.apiService.post(ClienteUrls.Login, { email, senha }).pipe(
      catchError(this.handleError)
    ).subscribe({
      next: data => {
        if (data.sucesso) {
          this.toastService.exibirMensagemSucesso('Sucesso', 'Login bem-sucedido');
          this.AppContextService.salvaAppContext(new AppContext(email, data.dados));
          this.router.navigate(['']);
          return true;
        }
        return false;
      },
      error: error => {
        console.error('Login failed:', error);
      },
      complete: () => {
        console.log('Login request completed');
      }
    });
    return false;
  }

  private handleError = (error: any) => {
    let errorMessage = 'Something bad happened; please try again later.';
    if (error.status === 401) {
      errorMessage = 'Senha Inválida';
      this.toastService.exibirMensagemErro('Erro', 'Senha Inválida');
      return of(false);
    }
    if (error.error instanceof ErrorEvent) {
      console.error('An error occurred:', error.error.message);
    } else {
      console.error(`Backend returned code ${error.status}, body was: `, error.error);
    }
    return throwError(() => new Error(errorMessage));
  }

}
