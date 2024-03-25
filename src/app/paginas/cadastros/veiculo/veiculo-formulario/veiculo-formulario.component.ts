import { Component } from '@angular/core';
import { Veiculo } from '../../../../dominio/entidade/veiculo';
import { ApiService } from '../../../../services/http/api.service';
import { TrasportadorUrls } from '../../../../dominio/enum/trasportador-url-enum';
import { ToastService } from '../../../../services/utils/notificacao/toast.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-veiculo-formulario',
  templateUrl: './veiculo-formulario.component.html',
  styleUrls: ['./veiculo-formulario.component.scss']
})
export class VeiculoFormularioComponent {

  constructor(private apiService: ApiService, private toastService: ToastService, private location: Location) { }

  veiculo = new Veiculo();

  salvar = (data: any) => {
    delete data.id;         
    this.apiService.post(TrasportadorUrls.ObterTodos + 'veiculo', JSON.stringify(data)
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