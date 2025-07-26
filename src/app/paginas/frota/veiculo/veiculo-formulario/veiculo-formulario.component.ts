import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule, Location } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { Veiculo } from '../../../../dominio/entidade/veiculo';
import { ApiService } from '../../../../services/http/api.service';
import { TrasportadorUrls } from '../../../../dominio/enum/trasportador-url-enum';
import { BreadcrumbService } from '../../../../services/breadcrumb/breadcrumb.service';
import { NotificationService } from '../../../../shared/services/notification.service';
import { FormularioComponent } from '../../../../componentes/formulario/formulario.component';
import { InputFieldComponent } from '../../../../shared/components/form/input-field/input-field.component';
import { SelectFieldComponent } from '../../../../shared/components/form/select-field/select-field.component';
import { ButtonComponent } from '../../../../shared/components/button/button.component';

@Component({
  selector: 'app-veiculo-formulario',
  templateUrl: './veiculo-formulario.component.html',
  styleUrls: ['./veiculo-formulario.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormularioComponent,
    InputFieldComponent,
    SelectFieldComponent,
    ButtonComponent
  ]
})
export class VeiculoFormularioComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private apiService: ApiService, 
    private location: Location,
    private breadcrumbService: BreadcrumbService,
    private notificationService: NotificationService
  ) { }

  veiculo = new Veiculo();
  isEditMode = false;

  ngOnInit(): void {
    // Forçar atualização dos breadcrumbs ao entrar na tela
    this.breadcrumbService.limparCache();
    this.breadcrumbService["atualizarBreadcrumbs"](this.router.url);
    this.processarDadosNavegacao();
  }

  private processarDadosNavegacao(): void {
    // Primeiro, verificar se há parâmetro id na URL
    const id = this.route.snapshot.paramMap.get('id');
    
    if (id) {
      // Modo edição via URL
      this.isEditMode = true;
      this.carregarVeiculoPorId(Number(id));
    } else {
      // Verificar se há dados no state (compatibilidade)
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
          
          console.log('=== DADOS CARREGADOS PARA EDIÇÃO (STATE) ===');
          console.log('Dados do veículo:', this.veiculo);
          console.log('Modo de edição:', this.isEditMode);
          
        } catch (error) {
          console.error('Erro ao processar dados da navegação:', error);
          this.notificationService.error('Erro', 'Erro ao carregar dados para edição');
          this.isEditMode = false;
        }
      } else {
        this.isEditMode = false;
        console.log('Modo de adição - novo veículo');
      }
    }
  }

  private carregarVeiculoPorId(id: number): void {
    const url = `${TrasportadorUrls.ObterTodos}veiculo/${id}`;
    
    this.apiService.get(url).subscribe({
      next: (response: any) => {
        if (response.success && response.data) {
          Object.assign(this.veiculo, response.data);
          console.log('=== DADOS CARREGADOS PARA EDIÇÃO (API) ===');
          console.log('Dados do veículo:', this.veiculo);
          console.log('Modo de edição:', this.isEditMode);
        } else {
          this.notificationService.error('Erro', 'Veículo não encontrado');
          this.location.back();
        }
      },
      error: (error) => {
        console.error('Erro ao carregar veículo:', error);
        this.notificationService.error('Erro', 'Erro ao carregar dados do veículo');
        this.location.back();
      }
    });
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
        if (response.success) {
          const mensagem = this.isEditMode ? 'Veículo atualizado com sucesso' : 'Veículo salvo com sucesso';
          this.notificationService.success('Sucesso', mensagem);
          this.location.back();
        } else {
          this.notificationService.error('Erro', 'Erro ao salvar veículo');
        }
      },
      error: (error) => {
        console.error('Erro completo:', error);
        console.error('Status:', error.status);
        console.error('Mensagem:', error.error);
        this.notificationService.error('Erro', 'Erro ao salvar veículo: ' + (error.error?.title || error.message));
      }
    });
  }
}