import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';

@Injectable({
  providedIn: 'root'
})
export class ToastService {

  constructor(private messageService: MessageService) {}

  exibirMensagemSucesso(summary: string, detail: string) {
    this.messageService.add({severity:'success', summary, detail});
  }

  exibirMensagemErro(summary: string, detail: string) {
    this.messageService.add({severity:'error', summary, detail});
  }

}
