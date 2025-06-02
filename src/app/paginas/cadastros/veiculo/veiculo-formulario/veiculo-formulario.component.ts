import { Component, OnInit } from '@angular/core';
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
export class VeiculoFormularioComponent implements OnInit {

  constructor(private apiService: ApiService, private toastService: ToastService, private location: Location) { }

  veiculo = new Veiculo();
  isEditMode = false;

  ngOnInit(): void {
    this.processarDadosNavegacao();
  }

  private processarDadosNavegacao(): void {
    const state = window.history.state;
    
    if (state?.isEditMode && state?.objeto) {
      this.isEditMode = true;
      try {
        // Processar o objeto recebido
        let dadosVeiculo = state.objeto;
        
        // Se for string, fazer parse
        if (typeof dadosVeiculo === 'string') {
          dadosVeiculo = JSON.parse(dadosVeiculo);
        }
        
        // Carregar dados no objeto veiculo
        Object.assign(this.veiculo, dadosVeiculo);
        
        console.log('=== DADOS CARREGADOS PARA EDIÇÃO ===');
        console.log('Dados do veículo:', this.veiculo);
        console.log('Modo de edição:', this.isEditMode);
        
      } catch (error) {
        console.error('Erro ao processar dados da navegação:', error);
        this.toastService.exibirMensagemErro('Erro', 'Erro ao carregar dados para edição');
        this.isEditMode = false;
      }
    } else {
      this.isEditMode = false;
      console.log('Modo de adição - novo veículo');
    }
  }

  salvar = (data: any) => {
    console.log('=== DADOS RECEBIDOS DO FORMULÁRIO ===');
    console.log('Dados originais:', data);
    console.log('Modo de edição:', this.isEditMode);
    console.log('ID do veículo:', data.id);
    console.log('Tipo tipoCombustivel:', typeof data.tipoCombustivel, '- Valor:', data.tipoCombustivel);
    console.log('Tipo status:', typeof data.status, '- Valor:', data.status);

    // Preparar dados apenas com os campos esperados pelo endpoint
    const dadosParaEnvio = {
      placa: data.placa || "",
      modelo: data.modelo || "",
      marca: data.marca || "",
      numeroChassi: data.numeroChassi || "",
      anoModelo: Number(data.anoModelo) || 0,
      anoFabricacao: Number(data.anoFabricacao) || 0,
      cor: data.cor || "",
      renavam: data.renavam || "",
      vencimentoLicenciamento: data.vencimentoLicenciamento ? new Date(data.vencimentoLicenciamento).toISOString() : null,
      tipoCombustivel: data.tipoCombustivel || "", // Enviar como string
      status: data.status || "", // Enviar como string
      observacao: data.observacao || null,
      modeloVeiculoId: Number(data.modeloVeiculoId) || 0
    };

    // Se for edição, incluir o ID
    if (this.isEditMode && data.id) {
      (dadosParaEnvio as any).id = Number(data.id);
    }

    console.log('=== DADOS PREPARADOS PARA ENVIO ===');
    console.log('Dados para envio:', dadosParaEnvio);
    console.log('JSON que será enviado:', JSON.stringify(dadosParaEnvio, null, 2));

    // Definir URL e método baseado no modo
    const url = this.isEditMode 
      ? `${TrasportadorUrls.ObterTodos}veiculo/${data.id}`
      : `${TrasportadorUrls.ObterTodos}veiculo`;
    
    const request = this.isEditMode 
      ? this.apiService.put(url, JSON.stringify(dadosParaEnvio))
      : this.apiService.post(url, JSON.stringify(dadosParaEnvio));

    request.subscribe({
      next: response => {
        console.log('Resposta do servidor:', response);
        if (response.sucesso) {
          const mensagem = this.isEditMode ? 'Veículo atualizado com sucesso' : 'Veículo salvo com sucesso';
          this.toastService.exibirMensagemSucesso('Sucesso', mensagem);
          this.location.back();
        } else {
          this.toastService.exibirMensagemErro('Erro', 'Erro ao salvar veículo');
        }
      },
      error: (error) => {
        console.error('Erro completo:', error);
        console.error('Status:', error.status);
        console.error('Mensagem:', error.error);
        this.toastService.exibirMensagemErro('Erro', 'Erro ao salvar veículo: ' + (error.error?.title || error.message));
      }
    });
  }
}