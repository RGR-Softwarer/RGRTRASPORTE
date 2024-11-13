import { Component } from '@angular/core';
import { ModeloVeicular } from '../../../../../dominio/entidade/veiculo/modelo-veicular';
import { ApiService } from '../../../../../services/http/api.service';
import { TrasportadorUrls } from '../../../../../dominio/enum/trasportador-url-enum';
import { ToastService } from '../../../../../services/utils/notificacao/toast.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-modelo-veicular-formulario',
  templateUrl: './modelo-veicular-formulario.component.html',
  styleUrls: ['./modelo-veicular-formulario.component.scss']
})
export class ModeloVeicularFormularioComponent {

  constructor(private apiService: ApiService, private toastService: ToastService, private location: Location) { }

  veiculo = new ModeloVeicular();

  salvar = (data: any) => {
    delete data.id;         
    this.apiService.post(TrasportadorUrls.ObterTodos + 'veiculo/ModeloVeicular', JSON.stringify(data)
    ).pipe(
    ).subscribe({
      next: data => {
        if (data.sucesso) {
          this.toastService.exibirMensagemSucesso('Sucesso', 'Veículo salvo com sucesso');
          this.location.back();
        } else
          this.toastService.exibirMensagemErro('Erro', 'Erro ao salvar veículo');
      },
      error: () => this.toastService.exibirMensagemErro('Erro', 'Erro ao salvar veículo')
    });
  }
}