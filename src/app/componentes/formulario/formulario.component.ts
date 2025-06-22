import { Component, Input, OnInit, AfterViewInit, OnDestroy, ChangeDetectionStrategy } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Location } from '@angular/common';
import { FormCamposMetadata, DecoratorUtils } from '../../services/decorator/formulario-decorator';
import { FormularioService, FormularioData } from '../../services/formulario/formulario.service';
import { ValidacaoService } from '../../services/formulario/validacao.service';
import { EstadoService } from '../../services/formulario/estado.service';
import { ToastService } from '../../services/utils/notificacao/toast.service';
import { LoggingService } from '../../services/utils/log/logging.service';
import { ChangeDetectorRef } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { NzModalService } from 'ng-zorro-antd/modal';
import { EntidadeSelecaoModalComponent, EntidadeSelecaoConfig } from './entidade-selecao-modal.component';

@Component({
  selector: 'app-formulario',
  templateUrl: './formulario.component.html',
  styleUrls: ['./formulario.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FormularioComponent implements OnInit, AfterViewInit, OnDestroy {
  @Input() salvarCallback: (data: FormularioData) => void = () => {};
  @Input() objeto: any = null;

  form: FormGroup = new FormGroup({});
  formFields: FormCamposMetadata[] = [];
  
  // Estados observáveis
  estado$ = this.estadoService.estado$;
  
  private destroy$ = new Subject<void>();

  constructor(
    private formularioService: FormularioService,
    private validacaoService: ValidacaoService,
    private estadoService: EstadoService,
    private toastService: ToastService,
    private location: Location,
    private cd: ChangeDetectorRef,
    private loggingService: LoggingService,
    private modal: NzModalService,
  ) {}

  ngOnInit(): void {
    this.inicializarFormulario();
  }

  private inicializarFormulario(): void {
    try {
      this.configurarCamposFormulario();
      this.extrairDadosNavegacao();
      this.criarFormularioReativo();
    } catch (error) {
      console.error('Erro ao inicializar formulário:', error);
      this.toastService.exibirMensagemErro('Erro', 'Falha ao inicializar o formulário');
    }
  }

  private configurarCamposFormulario(): void {
    if (!this.objeto) {
      console.warn('Nenhum objeto fornecido para o formulário');
      this.formFields = [];
      return;
    }

    this.formFields = DecoratorUtils.getFormFields(this.objeto);

    if (this.formFields.length === 0) {
      console.warn('Nenhum campo de formulário definido no objeto');
    }

    this.loggingService.log('Campos definidos no formFields:', this.formFields.map(f => f.key));
  }

  private criarFormularioReativo(): void {
    const dados = this.estadoService.getDados();
    this.form = this.formularioService.criarFormularioReativo(this.objeto, dados);
  }

  private extrairDadosNavegacao(): void {
    const { dados, isEditMode } = this.formularioService.extrairDadosNavegacao();
    
    this.estadoService.atualizarEstado({
      dados,
      isEditMode
    });
  }

  ngAfterViewInit(): void {
    // Aguardar um ciclo para garantir que o formulário foi criado
    setTimeout(() => {
      const estado = this.estadoService.getEstado();
      
      // Preencher formulário se houver dados
      if (estado.dados && estado.isEditMode) {
        this.formularioService.preencherFormulario(this.form, estado.dados, this.formFields);
      }
      
    // Apenas detectar mudanças para garantir que a view esteja atualizada
    this.cd.detectChanges();
    
    // Log para depuração
    this.loggingService.log('AfterViewInit - Estado do formulário:', {
      enabled: this.form.enabled,
      value: this.form.value,
        isEditMode: estado.isEditMode,
        dados: estado.dados
    });
    }, 100);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.estadoService.resetarEstado();
  }

  // Função para retornar à última rota
  voltar(): void {
    this.location.back();
  }

  onSubmit(): void {
    this.estadoService.setFormSubmitted(true);
    
    if (this.validacaoService.validarFormulario(this.form, this.formFields)) {
      try {
        const estado = this.estadoService.getEstado();
        const formData = this.formularioService.prepararDadosSubmissao(
          this.form, 
          estado.dados, 
          estado.isEditMode
        );
        
        this.estadoService.setLoading(true);
        this.salvarCallback(formData);
      } catch (error) {
        console.error('Erro ao processar dados do formulário:', error);
        this.toastService.exibirMensagemErro('Erro', 'Falha ao processar os dados do formulário');
      } finally {
        this.estadoService.setLoading(false);
      }
    }
  }

  // Métodos utilitários para o template
  isFieldInvalid(fieldKey: string): boolean {
    const estado = this.estadoService.getEstado();
    return this.validacaoService.isFieldInvalid(
      this.form, 
      fieldKey, 
      this.formFields, 
      estado.formSubmitted
    );
  }

  getFieldErrorMessage(fieldKey: string): string {
    return this.validacaoService.getFieldErrorMessage(this.form, fieldKey, this.formFields);
  }

  // Métodos de depuração para campos
  onFieldFocus(fieldKey: string): void {
    const control = this.form.get(fieldKey);
    this.loggingService.log(`Campo '${fieldKey}' recebeu foco:`, {
      enabled: control?.enabled,
      disabled: control?.disabled,
      value: control?.value
    });
  }

  onFieldBlur(fieldKey: string): void {
    console.log(`Campo ${fieldKey} perdeu o foco`);
    // Aqui você pode adicionar lógica adicional quando o campo perde o foco
  }

  formatarTelefone(event: any): void {
    let value = event.target.value.replace(/\D/g, ''); // Remove tudo que não é dígito
    
    if (value.length <= 11) {
      // Aplica máscara: (00) 00000-0000
      value = value.replace(/^(\d{2})(\d)/g, '($1) $2');
      value = value.replace(/(\d)(\d{4})$/, '$1-$2');
    }
    
    event.target.value = value;
  }

  abrirSelecaoEntidade(field: FormCamposMetadata): void {
    if (!field.entidadeConfig) {
      console.warn('Configuração de entidade não encontrada para o campo:', field.key);
      return;
    }

    const config = field.entidadeConfig;
    if (!config) {
      this.loggingService.error('Configuração da entidade não encontrada para o campo', field.key);
      return;
    }
    
    const modalRef = this.modal.create({
      nzTitle: config.modalTitle,
      nzContent: EntidadeSelecaoModalComponent,
      nzData: { config },
      nzWidth: config.modalWidth || 800,
      nzFooter: null
    });

    modalRef.afterClose.subscribe(result => {
      if (result && config) {
        this.loggingService.log('Entidade selecionada:', result);
        
        const idValue = result[config.valueField];
        const displayValue = result[config.displayField];
        
        // Conveção: o campo de nome correspondente termina com "Nome"
        // Ex: modeloVeiculoId -> modeloVeiculoNome
        const nameFieldKey = field.key.endsWith('Id') 
          ? field.key.slice(0, -2) + 'Nome' 
          : `${field.key}Nome`;

        const formUpdate: { [key: string]: any } = {};
        formUpdate[field.key] = idValue;

        if (this.form.controls[nameFieldKey]) {
            formUpdate[nameFieldKey] = displayValue;
        }

        this.form.patchValue(formUpdate);
      }
    });
  }
}
