import { Component, Input, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router'; // Importação do Router
import { Location } from '@angular/common'; // Importação do Location
import { FormCampoConstrutor, FormCamposMetadata } from '../../services/decorator/formulario-decorator';
import { ToastService } from '../../services/utils/notificacao/toast.service';
import { ChangeDetectorRef } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

// Interfaces para melhor tipagem
interface FormularioData {
  [key: string]: any;
}

interface FormularioState {
  objeto?: FormularioData;
}

interface FormularioConfig {
  fields: FormCamposMetadata[];
  data?: FormularioData;
  isEditMode: boolean;
}

@Component({
  selector: 'app-formulario',
  templateUrl: './formulario.component.html',
  styleUrls: ['./formulario.component.scss']
})
export class FormularioComponent implements OnInit, AfterViewInit, OnDestroy {
  @Input() salvarCallback: (data: FormularioData) => void = () => {};
  @Input() objeto: any; // Mantido como any por ser interface externa

  form: FormGroup = new FormGroup({});
  formFields: FormCamposMetadata[] = [];
  dados: FormularioData | null = null;
  isEditMode: boolean = false;
  private formSubmitted: boolean = false;
  
  private destroy$ = new Subject<void>();
  private formConfig: FormularioConfig | null = null;

  constructor(
    private fb: FormBuilder, 
    private toastService: ToastService,
    private route: ActivatedRoute,
    private cd: ChangeDetectorRef,
    private router: Router, // Injeção do Router
    private location: Location // Injeção do Location
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
    const objetoConstructor = this.objeto?.constructor as FormCampoConstrutor;
    if (!objetoConstructor?.formFields) {
      console.warn('Nenhum campo de formulário definido no objeto');
      this.formFields = [];
      return;
    }

    this.formFields = [...objetoConstructor.formFields];
    console.log('Campos definidos no formFields:', this.formFields.map(f => f.key));
  }

  private criarFormularioReativo(): void {
    if (this.formFields.length === 0) {
      this.form = this.fb.group({});
      return;
    }

    const formControls = this.formFields.reduce((acc, field) => {
      const validators = this.buildValidators(field);
      let valorInicial = this.obterValorInicialPorTipo(field);
      
      // Se temos dados e estamos em modo de edição, usar os dados reais
      if (this.dados && this.dados.hasOwnProperty(field.key)) {
        valorInicial = this.processarValorPorTipo(this.dados[field.key], field);
        console.log(`Campo ${field.key} inicializado com valor:`, valorInicial);
      }
      
      acc[field.key] = [valorInicial, validators];
      return acc;
    }, {} as { [key: string]: any[] });

    this.form = this.fb.group(formControls);
    
    console.log('FormGroup criado:', {
      enabled: this.form.enabled,
      value: this.form.value,
      controls: Object.keys(this.form.controls)
    });
  }

  private buildValidators(field: FormCamposMetadata): any[] {
    const validators: any[] = [];
    
    // Não adicionar validação required em campos readonly
    if (field.required && !field.readonly) {
      validators.push(Validators.required);
    }
    
    if (field.minLength) {
      validators.push(Validators.minLength(field.minLength));
    }
    
    if (field.maxLength) {
      validators.push(Validators.maxLength(field.maxLength));
    }
    
    if (field.min !== undefined) {
      validators.push(Validators.min(field.min));
    }
    
    if (field.max !== undefined) {
      validators.push(Validators.max(field.max));
    }

    return validators;
  }

  private obterValorInicialPorTipo(field: FormCamposMetadata): any {
    switch (field.type) {
      case 'numero':
        return null;
      case 'bool':
        return false;
      case 'enum':
        return null;
      case 'texto':
      case 'textarea':
      case 'email':
      case 'telefone':
        return '';
      case 'data':
        return '';
      case 'entidade':
        return '';
      default:
        return null;
    }
  }

  private extrairDadosNavegacao(): void {
    const state = window.history.state as FormularioState;
    
    if (!state?.objeto) {
      this.isEditMode = false;
      return;
    }

    try {
      this.dados = this.processarDadosEntrada(state.objeto);
      this.isEditMode = this.verificarModoEdicao(this.dados);
      
      console.log('Dados extraídos da navegação:', this.dados);
      console.log('Modo de edição:', this.isEditMode);
    } catch (error) {
      console.error('Erro ao processar dados da navegação:', error);
      this.toastService.exibirMensagemErro('Erro', 'Dados inválidos recebidos');
    }
  }

  private processarDadosEntrada(objeto: any): FormularioData {
    let dadosProcessados = objeto;

    // Tratar string JSON
    if (typeof dadosProcessados === 'string') {
      dadosProcessados = JSON.parse(dadosProcessados);
    }

    // Tratar array (pegar primeiro elemento)
    if (Array.isArray(dadosProcessados)) {
      if (dadosProcessados.length === 0) {
        throw new Error('Array de dados está vazio');
      }
      dadosProcessados = dadosProcessados[0];
    }

    // Validar se é um objeto válido
    if (!dadosProcessados || typeof dadosProcessados !== 'object') {
      throw new Error('Dados inválidos: deve ser um objeto');
    }

    return dadosProcessados;
  }

  private verificarModoEdicao(dados: FormularioData): boolean {
    // Verifica múltiplas propriedades que podem indicar edição
    return !!(dados['id'] || dados['ID'] || dados['guid'] || dados['GUID'] || dados['_id']);
  }

  ngAfterViewInit(): void {
    // Apenas detectar mudanças para garantir que a view esteja atualizada
    this.cd.detectChanges();
    
    // Log para depuração
    console.log('AfterViewInit - Estado do formulário:', {
      enabled: this.form.enabled,
      value: this.form.value,
      isEditMode: this.isEditMode,
      dados: this.dados
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private preencherFormulario(): void {
    if (!this.dados || !this.form) {
      console.log('Preenchimento cancelado:', { dados: !!this.dados, form: !!this.form });
      return;
    }

    console.log('Preenchendo o formulário com:', this.dados);
    console.log('Estado inicial do formulário:', {
      enabled: this.form.enabled,
      disabled: this.form.disabled,
      controls: Object.keys(this.form.controls)
    });

    const mappedData: FormularioData = {};

    this.formFields.forEach(field => {
      if (this.dados && this.dados.hasOwnProperty(field.key)) {
        const valorOriginal = this.dados[field.key];
        const valorProcessado = this.processarValorPorTipo(valorOriginal, field);
        
        mappedData[field.key] = valorProcessado;
        
        console.log(`Campo ${field.key} (${field.type}):`, {
          original: valorOriginal,
          processado: valorProcessado
        });
      } else {
        // Definir valores padrão baseados no tipo
        mappedData[field.key] = this.obterValorPadrao(field);
        console.warn(`Campo não encontrado em dados: ${field.key}, usando valor padrão`);
      }
    });

    console.log('Dados mapeados para patchValue:', mappedData);

    // Garantir que o formulário esteja habilitado antes do patchValue
    if (this.form.disabled) {
      this.form.enable();
      console.log('Formulário foi habilitado');
    }

    this.form.patchValue(mappedData);

    // Verificar estado após patchValue
    console.log('Estado do formulário após patchValue:', {
      enabled: this.form.enabled,
      disabled: this.form.disabled,
      value: this.form.value
    });

    // Verificar estado individual dos controles
    Object.keys(this.form.controls).forEach(key => {
      const control = this.form.get(key);
      if (control?.disabled) {
        console.warn(`Controle '${key}' está desabilitado`, control);
        control.enable();
      }
    });

    this.cd.detectChanges();
  }

  private processarValorPorTipo(valor: any, field: FormCamposMetadata): any {
    if (valor === null || valor === undefined) {
      return this.obterValorPadrao(field);
    }

    switch (field.type) {
      case 'numero':
        return this.processarValorNumerico(valor);
        
      case 'bool':
        return this.processarValorBooleano(valor);
        
      case 'enum':
        return this.processarValorEnum(valor, field);
        
      case 'texto':
      case 'textarea':
      case 'email':
      case 'telefone':
        return this.processarValorTexto(valor);
        
      case 'data':
        return this.processarValorData(valor);
        
      case 'entidade':
        return this.processarValorEntidade(valor);
        
      default:
        console.warn(`Tipo de campo não reconhecido: ${field.type}`);
        return valor;
    }
  }

  private processarValorNumerico(valor: any): number | null {
    if (typeof valor === 'number') {
      return valor;
    }
    
    if (typeof valor === 'string') {
      const numeroConvertido = parseFloat(valor);
      return isNaN(numeroConvertido) ? null : numeroConvertido;
    }
    
    return null;
  }

  private processarValorBooleano(valor: any): boolean {
    if (typeof valor === 'boolean') {
      return valor;
    }
    
    if (typeof valor === 'string') {
      const valorMinusculo = valor.toLowerCase().trim();
      return valorMinusculo === 'true' || valorMinusculo === '1' || valorMinusculo === 'sim';
    }
    
    if (typeof valor === 'number') {
      return valor === 1;
    }
    
    return false;
  }

  private processarValorEnum(valor: any, field: FormCamposMetadata): any {
    if (!valor) {
      return null;
    }

    // Se o valor é uma string e corresponde a uma das opções, retornar como está
    if (typeof valor === 'string') {
      const opcaoEncontrada = field.options?.find(opt => 
        opt.value === valor || opt.label === valor
      );
      return opcaoEncontrada ? opcaoEncontrada.value : valor;
    }
    
    return valor;
  }

  private processarValorTexto(valor: any): string {
    if (valor === null || valor === undefined) {
      return '';
    }
    
    return String(valor);
  }

  private processarValorEntidade(valor: any): any {
    // Para campos de entidade, geralmente queremos o valor como está
    // ou extrair um campo específico como 'nome' ou 'descricao'
    if (valor && typeof valor === 'object') {
      return valor.nome || valor.descricao || valor.label || valor.toString();
    }
    
    return valor || '';
  }

  private processarValorData(valor: any): string {
    if (!valor) {
      return '';
    }

    // Se já é uma string no formato correto, retornar
    if (typeof valor === 'string') {
      // Verificar se está no formato ISO (yyyy-mm-dd) ou converter
      if (valor.includes('T')) {
        // É uma data ISO, extrair apenas a parte da data
        return valor.split('T')[0];
      }
      
      // Se está no formato dd/mm/yyyy, converter para yyyy-mm-dd
      if (valor.includes('/')) {
        const partes = valor.split('/');
        if (partes.length === 3) {
          return `${partes[2]}-${partes[1].padStart(2, '0')}-${partes[0].padStart(2, '0')}`;
        }
      }
      
      return valor;
    }

    // Se é um objeto Date
    if (valor instanceof Date) {
      return valor.toISOString().split('T')[0];
    }

    return '';
  }

  private obterValorPadrao(field: FormCamposMetadata): any {
    switch (field.type) {
      case 'numero':
        return null;
      case 'bool':
        return false;
      case 'enum':
        return null;
      case 'texto':
      case 'textarea':
      case 'email':
      case 'telefone':
        return '';
      case 'data':
        return '';
      case 'entidade':
        return '';
      default:
        return null;
    }
  }

  // Função para retornar à última rota
  voltar(): void {
    this.location.back();
  }

  onSubmit(): void {
    this.formSubmitted = true;
    
    if (this.form.valid) {
      try {
        const formData = this.prepararDadosSubmissao();
        this.salvarCallback(formData);
      } catch (error) {
        console.error('Erro ao processar dados do formulário:', error);
        this.toastService.exibirMensagemErro('Erro', 'Falha ao processar os dados do formulário');
      }
    } else {
      this.exibirErrosValidacao();
    }
  }

  private prepararDadosSubmissao(): FormularioData {
    const formValue = { ...this.form.value };
    
    console.log('Valores do formulário antes do processamento:', formValue);
    
    // Se está em modo de edição, preservar o ID
    if (this.isEditMode && this.dados) {
      const idField = this.identificarCampoId();
      if (idField && this.dados[idField]) {
        formValue[idField] = this.dados[idField];
      }
    }

    // Log específico para campos de enum
    this.formFields.forEach(field => {
      if (field.type === 'enum' && formValue[field.key]) {
        console.log(`Campo enum ${field.key}:`, {
          valorFormulario: formValue[field.key],
          opcoes: field.options
        });
      }
    });

    console.log('Valores finais para submissão:', formValue);
    return formValue;
  }

  private identificarCampoId(): string | null {
    const possiveisIds = ['id', 'ID', 'guid', 'GUID', '_id'];
    return possiveisIds.find(id => this.dados && this.dados[id]) || null;
  }

  private exibirErrosValidacao(): void {
    // Coletar informações sobre campos com erro
    const camposComErro: string[] = [];
    const errosDetalhados: string[] = [];
    
    Object.keys(this.form.controls).forEach(key => {
      const control = this.form.get(key);
      const field = this.formFields.find(f => f.key === key);
      
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
        const errorMessage = this.getFieldErrorMessage(key);
        if (errorMessage) {
          errosDetalhados.push(errorMessage);
        }
      }
    });

    // Forçar detecção de mudanças para exibir erros visuais
    this.cd.detectChanges();

    // Mensagem mais detalhada
    if (camposComErro.length > 0) {
      const mensagemDetalhada = `Os seguintes campos contêm erros:\n• ${errosDetalhados.join('\n• ')}`;
      this.toastService.exibirMensagemErro('Formulário Inválido', mensagemDetalhada);
      
      // Log detalhado para depuração
      console.error('Campos com erro:', {
        campos: camposComErro,
        erros: errosDetalhados,
        formStatus: this.form.status,
        formErrors: this.form.errors,
        controlsStatus: Object.keys(this.form.controls).map(key => ({
          campo: key,
          valor: this.form.get(key)?.value,
          erro: this.form.get(key)?.errors,
          valido: this.form.get(key)?.valid,
          readonly: this.formFields.find(f => f.key === key)?.readonly
        }))
      });
    } else {
      this.toastService.exibirMensagemErro('Erro', 'Formulário inválido. Verifique os dados informados.');
    }

    // Scroll para o primeiro campo com erro
    this.scrollParaPrimeiroErro();
  }

  private scrollParaPrimeiroErro(): void {
    setTimeout(() => {
      const firstErrorElement = document.querySelector('.ant-form-item-has-error input, .ant-form-item-has-error nz-select, .ant-form-item-has-error textarea, .ant-form-item-has-error nz-date-picker');
      if (firstErrorElement) {
        firstErrorElement.scrollIntoView({ behavior: 'smooth', block: 'center' });
        (firstErrorElement as HTMLElement).focus();
        
        // Log para depuração
        console.log('Rolando para o primeiro campo com erro:', firstErrorElement);
      } else {
        console.warn('Nenhum campo com erro foi encontrado para scroll');
      }
    }, 200); // Aumento o timeout para 200ms para garantir que o DOM foi atualizado
  }

  // Métodos utilitários para o template
  isFieldInvalid(fieldKey: string): boolean {
    const control = this.form.get(fieldKey);
    const field = this.formFields.find(f => f.key === fieldKey);
    
    if (!control || field?.readonly) {
      return false;
    }
    
    // Exibir erro se o campo é inválido E (foi tocado/está sujo OU o formulário foi submetido)
    return control.invalid && (control.dirty || control.touched || this.formSubmitted);
  }

  getFieldErrorMessage(fieldKey: string): string {
    const control = this.form.get(fieldKey);
    if (!control || !control.errors) {
      return '';
    }

    const field = this.formFields.find(f => f.key === fieldKey);
    const fieldLabel = field?.label || fieldKey;

    if (control.errors['required']) {
      return `${fieldLabel} é obrigatório`;
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

  // Métodos de depuração para campos
  onFieldFocus(fieldKey: string): void {
    const control = this.form.get(fieldKey);
    console.log(`Campo '${fieldKey}' recebeu foco:`, {
      enabled: control?.enabled,
      disabled: control?.disabled,
      value: control?.value
    });
  }

  onFieldBlur(fieldKey: string): void {
    const control = this.form.get(fieldKey);
    console.log(`Campo '${fieldKey}' perdeu foco:`, {
      enabled: control?.enabled,
      disabled: control?.disabled,
      value: control?.value,
      valid: control?.valid,
      errors: control?.errors
    });
  }
}
