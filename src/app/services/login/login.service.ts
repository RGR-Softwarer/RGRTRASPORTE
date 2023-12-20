import { ApiService } from './../http/api.service';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { of, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ToastService } from '../utils/notificacao/toast.service';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private apiService: ApiService, private toastService: ToastService) { }

  public authenticate(email: string, senha: string): boolean {
    this.apiService.post(`${environment.apiBaseUrl}/Auth/login`, { email, senha }).pipe(
      catchError(this.handleError)
    ).subscribe({
      next: data => {
        console.log('Login successful:', data);
        if (data.sucesso) {
          this.toastService.exibirMensagemSucesso('Sucesso', 'Login bem-sucedido');
          return true
        }        
        return false;
      },
      error: error => {
        console.error('Login failed:', error);
        // Lógica de tratamento de erro
      },
      complete: () => {
        console.log('Login request completed');
        // Lógica após a conclusão da requisição (opcional)
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
