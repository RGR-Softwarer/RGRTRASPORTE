import { Injectable } from '@angular/core';
import { AppContext } from '../../dominio/entidade/app.context';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AppContextService {
  private appSubject = new BehaviorSubject<AppContext | null>(null);
  public app$ = this.appSubject.asObservable();

  salvaAppContext(app: AppContext) {
    localStorage.setItem('appContext', JSON.stringify(app));
    this.appSubject.next(app);
  }

  limparAppContext() {
    this.appSubject.next(null);
  }

  usuarioLogado(): boolean {
    this.obterUsuarioLogado();
    return this.appSubject.getValue() != null;
  }

  obterUsuarioLogado(): AppContext | null {
    if (this.appSubject.getValue() == null) {
      return this.obterLocalStorage();
    }

    return this.appSubject.getValue();
  }

  obterLocalStorage(): AppContext | null {
    let appContextData = localStorage.getItem('appContext');
    if (appContextData != null && appContextData.trim() !== '') {
      let objeto = JSON.parse(appContextData);
      let appContext = new AppContext(objeto.username, objeto.token);
      this.appSubject.next(appContext);
      return appContext;
    } else {
      return null;
    }

  }
}