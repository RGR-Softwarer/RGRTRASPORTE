import { Injectable } from '@angular/core';
import { FormGroup, AbstractControl } from '@angular/forms';
import { FormCamposMetadata } from '../decorator/formulario-decorator';
import { NotificationService } from '../../shared/services/notification.service';
import { LoggingService } from '../utils/log/logging.service';

@Injectable({
  providedIn: 'root'
})
export class ValidacaoService {

  constructor(
    private notificationService: NotificationService,
    private loggingService: LoggingService
  ) {}

  /**
   * Verifica se um campo específico é inválido
   */
  isFieldInvalid(form: FormGroup, fieldKey: string, formFields: FormCamposMetadata[], formSubmitted: boolean): boolean {
    const control = form.get(fieldKey);
    const field = formFields.find(f => f.key === fieldKey);
    
    if (!control || field?.readonly) {
      return false;
    }
    
    // Exibir erro se o campo é inválido E (foi tocado/está sujo OU o formulário foi submetido)
    return control.invalid && (control.dirty || control.touched || formSubmitted);
  }

  /**
   * Obtém mensagem de erro para um campo específico
   */
  getFieldErrorMessage(form: FormGroup, fieldKey: string, formFields: FormCamposMetadata[]): string {
    const control = form.get(fieldKey);
    if (!control || !control.errors) {
      return '';
    }

    const field = formFields.find(f => f.key === fieldKey);
    const fieldLabel = field?.label || fieldKey;

    if (control.errors['required']) {
      return `${fieldLabel} é obrigatório`;
    }
    if (control.errors['email']) {
      return `${fieldLabel} deve ser um email válido`;
    }
    if (control.errors['minlength']) {
      return `${fieldLabel} deve ter pelo menos ${control.errors['minlength'].requiredLength} caracteres`;
    }
    if (control.errors['maxlength']) {
      return `${fieldLabel} deve ter no máximo ${control.errors['maxlength'].requiredLength} caracteres`;
    }
    if (control.errors['min']) {
      return `${fieldLabel} deve ser maior ou igual a ${control.errors['min'].min}`;
    }
    if (control.errors['max']) {
      return `${fieldLabel} deve ser menor ou igual a ${control.errors['max'].max}`;
    }

    return `${fieldLabel} é inválido`;
  }

  /**
   * Exibe erros de validação do formulário
   */
  exibirErrosValidacao(form: FormGroup, formFields: FormCamposMetadata[]): void {
    // Coletar informações sobre campos com erro
    const camposComErro: string[] = [];
    const errosDetalhados: string[] = [];
    
    Object.keys(form.controls).forEach(key => {
      const control = form.get(key);
      const field = formFields.find(f => f.key === key);
      
      // Ignorar campos readonly na validação
      if (field?.readonly) {
        return;
      }
      
      if (control && control.invalid) {
        control.markAsDirty();
        control.markAsTouched();
        control.updateValueAndValidity({ onlySelf: true });
        
        const fieldLabel = field?.label || key;
        camposComErro.push(fieldLabel);
        
        // Obter erro específico
        const errorMessage = this.getFieldErrorMessage(form, key, formFields);
        if (errorMessage) {
          errosDetalhados.push(errorMessage);
        }
      }
    });

    // Mensagem mais detalhada
    if (camposComErro.length > 0) {
      const mensagemDetalhada = `Os seguintes campos contêm erros:\n• ${errosDetalhados.join('\n• ')}`;
      this.notificationService.error('Formulário Inválido', mensagemDetalhada);
      
      // Log detalhado para depuração
      this.loggingService.error('Campos com erro:', {
        campos: camposComErro,
        erros: errosDetalhados,
        formStatus: form.status,
        formErrors: form.errors,
        controlsStatus: Object.keys(form.controls).map(key => ({
          campo: key,
          valor: form.get(key)?.value,
          erro: form.get(key)?.errors,
          valido: form.get(key)?.valid,
          readonly: formFields.find(f => f.key === key)?.readonly
        }))
      });
    } else {
      this.notificationService.error('Erro', 'Formulário inválido. Verifique os dados informados.');
    }
  }

  /**
   * Faz scroll para o primeiro campo com erro
   */
  scrollParaPrimeiroErro(): void {
    setTimeout(() => {
      const firstErrorElement = document.querySelector('.ant-form-item-has-error input, .ant-form-item-has-error nz-select, .ant-form-item-has-error textarea, .ant-form-item-has-error nz-date-picker, .ant-form-item-has-error .ant-picker');
      if (firstErrorElement) {
        firstErrorElement.scrollIntoView({ behavior: 'smooth', block: 'center' });
        (firstErrorElement as HTMLElement).focus();
        
        // Log para depuração
        this.loggingService.log('Rolando para o primeiro campo com erro:', firstErrorElement);
      } else {
        console.warn('Nenhum campo com erro foi encontrado para scroll');
      }
    }, 200);
  }

  /**
   * Valida se o formulário está válido e exibe erros se necessário
   */
  validarFormulario(form: FormGroup, formFields: FormCamposMetadata[]): boolean {
    if (form.valid) {
      return true;
    }

    this.exibirErrosValidacao(form, formFields);
    this.scrollParaPrimeiroErro();
    return false;
  }

  /**
   * Marca todos os campos como tocados para exibir erros
   */
  marcarCamposComoTocados(form: FormGroup): void {
    Object.keys(form.controls).forEach(key => {
      const control = form.get(key);
      if (control) {
        control.markAsTouched();
        control.updateValueAndValidity();
      }
    });
  }

  /**
   * Limpa todos os erros do formulário
   */
  limparErros(form: FormGroup): void {
    Object.keys(form.controls).forEach(key => {
      const control = form.get(key);
      if (control) {
        control.markAsUntouched();
        control.markAsPristine();
        control.setErrors(null);
      }
    });
  }
} 