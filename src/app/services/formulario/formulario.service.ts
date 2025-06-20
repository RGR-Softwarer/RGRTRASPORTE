import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormCamposMetadata, DecoratorUtils } from '../decorator/formulario-decorator';
import { LoggingService } from '../utils/log/logging.service';

export interface FormularioData {
  [key: string]: any;
}

export interface FormularioState {
  objeto?: FormularioData;
}

@Injectable({
  providedIn: 'root'
})
export class FormularioService {
  
  constructor(
    private fb: FormBuilder,
    private loggingService: LoggingService
  ) {}

  /**
   * Cria um FormGroup reativo baseado nos metadados da entidade
   */
  criarFormularioReativo(objeto: any, dados?: FormularioData | null): FormGroup {
    const formFields = DecoratorUtils.getFormFields(objeto);
    
    if (formFields.length === 0) {
      this.loggingService.warn('Nenhum campo de formulário definido no objeto');
      return this.fb.group({});
    }

    const formControls = formFields.reduce((acc, field) => {
      const validators = this.buildValidators(field);
      let valorInicial = this.obterValorInicialPorTipo(field);
      
      // Se temos dados, usar os dados reais
      if (dados && dados.hasOwnProperty(field.key)) {
        valorInicial = this.processarValorPorTipo(dados[field.key], field);
        this.loggingService.log(`Campo ${field.key} inicializado com valor:`, valorInicial);
      }
      
      acc[field.key] = [valorInicial, validators];
      return acc;
    }, {} as { [key: string]: any[] });

    const form = this.fb.group(formControls);
    
    this.loggingService.log('FormGroup criado:', {
      enabled: form.enabled,
      value: form.value,
      controls: Object.keys(form.controls)
    });

    return form;
  }

  /**
   * Constrói validadores para um campo específico
   */
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

    // Validação específica para email
    if (field.type === 'email') {
      validators.push(Validators.email);
    }

    return validators;
  }

  /**
   * Obtém valor inicial por tipo de campo
   */
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

  /**
   * Processa valor por tipo de campo
   */
  private processarValorPorTipo(valor: any, field: FormCamposMetadata): any {
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
        return valor;
    }
  }

  private processarValorNumerico(valor: any): number | null {
    if (valor === null || valor === undefined || valor === '') {
      return null;
    }
    
    const numero = Number(valor);
    return isNaN(numero) ? null : numero;
  }

  private processarValorBooleano(valor: any): boolean {
    if (typeof valor === 'boolean') {
      return valor;
    }
    
    if (typeof valor === 'string') {
      return valor.toLowerCase() === 'true' || valor === '1';
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
    
    // Verificar se o valor existe nas opções
    const opcaoEncontrada = field.options.find(opt => 
      opt.value === valor || opt.label === valor
    );
    
    return opcaoEncontrada ? opcaoEncontrada.value : valor;
  }

  private processarValorTexto(valor: any): string {
    return valor ? String(valor).trim() : '';
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

  private processarValorEntidade(valor: any): any {
    if (valor && typeof valor === 'object') {
      return valor.nome || valor.descricao || valor.label || valor.toString();
    }
    
    return valor || '';
  }

  /**
   * Extrai dados da navegação
   */
  extrairDadosNavegacao(): { dados: FormularioData | null; isEditMode: boolean } {
    const state = window.history.state as FormularioState;
    
    if (!state?.objeto) {
      return { dados: null, isEditMode: false };
    }

    try {
      const dados = this.processarDadosEntrada(state.objeto);
      const isEditMode = this.verificarModoEdicao(dados);
      
      this.loggingService.log('Dados extraídos da navegação:', dados);
      this.loggingService.log('Modo de edição:', isEditMode);
      
      return { dados, isEditMode };
    } catch (error) {
      console.error('Erro ao processar dados da navegação:', error);
      return { dados: null, isEditMode: false };
    }
  }

  private processarDadosEntrada(objeto: any): FormularioData | null {
    if (!objeto) return null;
    
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

  private verificarModoEdicao(dados: FormularioData | null): boolean {
    if (!dados) return false;
    const possiveisIds = ['id', 'ID', 'guid', 'GUID', '_id'];
    return possiveisIds.some(id => dados && dados[id]);
  }

  /**
   * Preenche formulário com dados
   */
  preencherFormulario(form: FormGroup, dados: FormularioData | null, formFields: FormCamposMetadata[]): void {
    if (!dados || !form) {
      this.loggingService.log('Preenchimento cancelado:', { dados: !!dados, form: !!form });
      return;
    }

    this.loggingService.log('Preenchendo o formulário com:', dados);

    const mappedData: FormularioData = {};

    formFields.forEach(field => {
      if (dados.hasOwnProperty(field.key)) {
        const valorOriginal = dados[field.key];
        const valorProcessado = this.processarValorPorTipo(valorOriginal, field);
        
        mappedData[field.key] = valorProcessado;
        
        this.loggingService.log(`Campo ${field.key} (${field.type}):`, {
          original: valorOriginal,
          processado: valorProcessado
        });
      } else {
        // Definir valores padrão baseados no tipo
        mappedData[field.key] = this.obterValorPadrao(field);
        console.warn(`Campo não encontrado em dados: ${field.key}, usando valor padrão`);
      }
    });

    this.loggingService.log('Dados mapeados para patchValue:', mappedData);

    // Garantir que o formulário esteja habilitado antes do patchValue
    if (form.disabled) {
      form.enable();
      this.loggingService.log('Formulário foi habilitado');
    }

    try {
      form.patchValue(mappedData);
      this.loggingService.log('patchValue executado com sucesso');
    } catch (error) {
      console.error('Erro ao executar patchValue:', error);
      this.loggingService.log('Erro no patchValue:', error);
    }
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

  /**
   * Prepara dados para submissão
   */
  prepararDadosSubmissao(form: FormGroup, dados: FormularioData | null, isEditMode: boolean): FormularioData {
    const formValue = { ...form.value };
    
    this.loggingService.log('Valores do formulário antes do processamento:', formValue);
    
    // Se está em modo de edição, preservar o ID
    if (isEditMode && dados) {
      const idField = this.identificarCampoId(dados);
      if (idField && dados[idField]) {
        formValue[idField] = dados[idField];
      }
    }

    this.loggingService.log('Valores finais para submissão:', formValue);
    return formValue;
  }

  private identificarCampoId(dados: FormularioData | null): string | null {
    if (!dados) return null;
    const possiveisIds = ['id', 'ID', 'guid', 'GUID', '_id'];
    return possiveisIds.find(id => dados && dados[id]) || null;
  }
} 