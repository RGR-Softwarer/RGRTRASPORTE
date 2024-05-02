import { Component } from '@angular/core';
import { Viagem } from '../../../../dominio/entidade/viagem';
import { ApiService } from '../../../../services/http/api.service';
import { TrasportadorUrls } from '../../../../dominio/enum/trasportador-url-enum';
import { ToastService } from '../../../../services/utils/notificacao/toast.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-viagem-formulario',
  templateUrl: './viagem-formulario.component.html',
  styleUrl: './viagem-formulario.component.scss'
})
export class ViagemFormularioComponent {

  constructor(private apiService: ApiService, private toastService: ToastService, private location: Location) { }

  viagem = new Viagem();

  salvar = (data: any) => {
    delete data.id;         
    this.apiService.post(TrasportadorUrls.ObterTodos + 'viagem', JSON.stringify(data)
    ).pipe(
    ).subscribe({
      next: data => {
        if (data.sucesso) {
          this.toastService.exibirMensagemSucesso('Sucesso', 'Viagem salvo com sucesso');
          this.location.back();
        } else
          this.toastService.exibirMensagemErro('Erro', 'Erro ao salvar Viagem');
      },
      error: () => this.toastService.exibirMensagemErro('Erro', 'Erro ao salvar Viagem')
    });
  }
}
