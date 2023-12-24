import { Injectable } from '@angular/core';
import { NzNotificationService } from 'ng-zorro-antd/notification';

@Injectable({
  providedIn: 'root'
})
export class ToastService {

  constructor(private notificationService: NzNotificationService) {}

  exibirMensagemSucesso(titulo: string, conteudo: string) {
    this.notificationService.success(titulo, conteudo);
  }

  exibirMensagemErro(titulo: string, conteudo: string) {
    this.notificationService.error(titulo, conteudo);
  }
}
