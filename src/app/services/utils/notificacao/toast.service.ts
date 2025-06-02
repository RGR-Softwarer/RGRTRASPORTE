import { Injectable } from '@angular/core';
import { NzNotificationService } from 'ng-zorro-antd/notification';

@Injectable({
  providedIn: 'root'
})
export class ToastService {

  constructor(private notificationService: NzNotificationService) {}

  exibirMensagemSucesso(titulo: string, conteudo: string): void {
    this.notificationService.success(titulo, conteudo);
  }

  exibirMensagemErro(titulo: string, conteudo: string): void {
    this.notificationService.error(titulo, conteudo);
  }

  exibirMensagemInfo(titulo: string, conteudo: string): void {
    this.notificationService.info(titulo, conteudo);
  }

  exibirMensagemAviso(titulo: string, conteudo: string): void {
    this.notificationService.warning(titulo, conteudo);
  }
}
