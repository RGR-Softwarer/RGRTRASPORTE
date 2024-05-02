import { Component } from '@angular/core';
import { Pacote } from '../../../../dominio/entidade/pacote';
import { ApiService } from '../../../../services/http/api.service';
import { TrasportadorUrls } from '../../../../dominio/enum/trasportador-url-enum';
import { ToastService } from '../../../../services/utils/notificacao/toast.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-pacote-sformulario',
  templateUrl: './pacote-formulario.component.html',
  styleUrl: './pacote-formulario.component.scss'
})
export class PacoteFormularioComponent {

  constructor(private apiService: ApiService, private toastService: ToastService, private location: Location) { }

  pacote = new Pacote();

  salvar = (data: any) => {
    delete data.id;         
    this.apiService.post(TrasportadorUrls.ObterTodos + 'pacote', JSON.stringify(data)
    ).pipe(
    ).subscribe({
      next: data => {
        if (data.sucesso) {
          this.toastService.exibirMensagemSucesso('Sucesso', 'Pacote salvo com sucesso');
          this.location.back();
        } else
          this.toastService.exibirMensagemErro('Erro', 'Erro ao salvar pacote');
      },
      error: () => this.toastService.exibirMensagemErro('Erro', 'Erro ao salvar pacote')
    });
  }
}
