import 'reflect-metadata';

// Tipos específicos para melhor tipagem
export type FormFieldType = 'texto' | 'numero' | 'enum' | 'entidade' | 'bool' | 'data' | 'textarea' | 'email' | 'telefone';
export type FilterFieldType = 'texto' | 'numero' | 'enum' | 'bool' | 'data';
export type FilterOperator = 'contains' | 'equals' | 'in' | 'between';

export interface FormCamposMetadata {
  key: string;
  label: string;
  type: FormFieldType;
  required: boolean;
  readonly?: boolean;
  nzSpan: number;
  visible: boolean;
  options: FormCampoOption[];
  
  // Propriedades de responsividade
  nzXs?: number;
  nzSm?: number;
  nzMd?: number;
  nzLg?: number;
  nzXl?: number;
  nzXXl?: number;
  
  // Propriedades de validação e UX
  description?: string;
  placeholder?: string;
  maxLength?: number;
  minLength?: number;
  min?: number;
  max?: number;
  step?: number;
  rows?: number; // Para textarea
  
  // Propriedades específicas para campos de entidade
  entidadeConfig?: {
    url: string;
    displayField: string;
    valueField: string;
    searchFields: string[];
    modalTitle: string;
    modalWidth?: number;
    entidade?: any;
  };
}

export interface FormCampoOption {
  label: string;
  value: any;
}

export interface FormCampoConstrutor {
  new (...args: any[]): any; 
  formFields?: FormCamposMetadata[]; 
  filterFields?: FiltroMetadata[];
}

// Interface mais flexível para objetos que têm metadados
export interface FormCampoObject {
  constructor: FormCampoConstrutor;
}

// Interfaces para filtros
export interface FiltroMetadata {
  key: string;
  label: string;
  type: FilterFieldType;
  visible: boolean;
  options: FormCampoOption[];
  placeholder?: string;
  operador?: FilterOperator;
}

export interface FiltroConstrutor {
  new (...args: any[]): {};
  filterFields?: FiltroMetadata[];
}

export function FormCampo(
  label: string, 
  type: FormFieldType, 
  visible: boolean = true, 
  required: boolean = false, 
  enumType?: any, 
  colSpan16: number = 4,
  readonly: boolean = false,
  options?: Partial<Pick<FormCamposMetadata, 'description' | 'placeholder' | 'maxLength' | 'minLength' | 'min' | 'max' | 'step' | 'rows' | 'nzXs' | 'nzSm' | 'nzMd' | 'nzLg' | 'nzXl' | 'nzXXl' | 'entidadeConfig'>>
) {
  return function(target: any, propertyName: string) {
    // Validações
    if (!label || label.trim() === '') {
      console.warn(`FormCampo: Label é obrigatório para a propriedade ${propertyName}`);
      return;
    }

    if (colSpan16 < 1 || colSpan16 > 16) {
      console.warn(`FormCampo: colSpan16 deve estar entre 1 e 16 para ${propertyName}, usando valor padrão 4`);
      colSpan16 = 4;
    }

    const nzSpan = Math.round((colSpan16 / 16) * 24);

    let fieldOptions: FormCampoOption[] = [];

    if(type === 'enum' && enumType) {
      fieldOptions = Object.entries(enumType).map(([key, label]) => ({ 
        label: label as string, 
        value: label 
      })) as FormCampoOption[];
    }

    const constructor = target.constructor as FormCampoConstrutor;
    if (!constructor.formFields) {
      constructor.formFields = [];
    }

    // Verificar se o campo já existe
    const existingFieldIndex = constructor.formFields.findIndex(field => field.key === propertyName);
    const fieldMetadata: FormCamposMetadata = { 
      key: propertyName, 
      label, 
      type, 
      required, 
      readonly,
      nzSpan, 
      visible, 
      options: fieldOptions,
      ...options // Spread das opções adicionais
    };

    if (existingFieldIndex >= 0) {
      // Atualizar campo existente
      constructor.formFields[existingFieldIndex] = fieldMetadata;
    } else {
      // Adicionar novo campo
      constructor.formFields.push(fieldMetadata);
    }
  };
}

export function FormCampoEntidade(
  label: string,
  required: boolean = false,
  entidadeConfig: FormCamposMetadata['entidadeConfig'],
  colSpan16: number = 8,
  readonly: boolean = false,
  visible: boolean = true,
  options?: Partial<Omit<FormCamposMetadata, 'key' | 'label' | 'type' | 'required' | 'entidadeConfig' | 'nzSpan' | 'visible'>>
) {
  return FormCampo(
    label,
    'entidade',
    visible,
    required,
    undefined,
    colSpan16,
    readonly,
    { ...options, entidadeConfig }
  );
}

// Decorator para filtros
export function FiltroGrid(
  label: string,
  type: FilterFieldType,
  visible: boolean = true,
  enumType?: any,
  operador: FilterOperator = 'contains',
  options?: Partial<Pick<FiltroMetadata, 'placeholder'>>
) {
  return function(target: any, propertyName: string) {
    // Validações
    if (!label || label.trim() === '') {
      console.warn(`FiltroGrid: Label é obrigatório para a propriedade ${propertyName}`);
      return;
    }

    let fieldOptions: FormCampoOption[] = [];

    if(type === 'enum' && enumType) {
      fieldOptions = Object.entries(enumType).map(([key, label]) => ({ 
        label: label as string, 
        value: label 
      })) as FormCampoOption[];
    }

    const constructor = target.constructor as FiltroConstrutor;
    if (!constructor.filterFields) {
      constructor.filterFields = [];
    }

    // Verificar se o filtro já existe
    const existingFilterIndex = constructor.filterFields.findIndex(filter => filter.key === propertyName);
    const filterMetadata: FiltroMetadata = { 
      key: propertyName, 
      label, 
      type, 
      visible, 
      options: fieldOptions,
      operador,
      placeholder: options?.placeholder || `Filtrar por ${label.toLowerCase()}`,
      ...options
    };

    if (existingFilterIndex >= 0) {
      // Atualizar filtro existente
      constructor.filterFields[existingFilterIndex] = filterMetadata;
    } else {
      // Adicionar novo filtro
      constructor.filterFields.push(filterMetadata);
}
  };
}

// Funções utilitárias para trabalhar com os decorators
export class DecoratorUtils {
  
  /**
   * Obtém todos os campos de formulário de uma classe
   */
  static getFormFields(target: any): FormCamposMetadata[] {
    const constructor = target?.constructor as FormCampoConstrutor;
    return constructor?.formFields || [];
  }

  /**
   * Obtém todos os filtros de uma classe
   */
  static getFilterFields(target: any): FiltroMetadata[] {
    const constructor = target?.constructor as FiltroConstrutor;
    return constructor?.filterFields || [];
  }

  /**
   * Obtém um campo específico por chave
   */
  static getFormField(target: any, key: string): FormCamposMetadata | undefined {
    return this.getFormFields(target).find(field => field.key === key);
  }

  /**
   * Obtém um filtro específico por chave
   */
  static getFilterField(target: any, key: string): FiltroMetadata | undefined {
    return this.getFilterFields(target).find(filter => filter.key === key);
  }

  /**
   * Valida se uma classe tem campos de formulário definidos
   */
  static hasFormFields(target: any): boolean {
    return this.getFormFields(target).length > 0;
  }

  /**
   * Valida se uma classe tem filtros definidos
   */
  static hasFilterFields(target: any): boolean {
    return this.getFilterFields(target).length > 0;
  }

  /**
   * Obtém campos visíveis de formulário
   */
  static getVisibleFormFields(target: any): FormCamposMetadata[] {
    return this.getFormFields(target).filter(field => field.visible);
  }

  /**
   * Obtém filtros visíveis
   */
  static getVisibleFilterFields(target: any): FiltroMetadata[] {
    return this.getFilterFields(target).filter(filter => filter.visible);
  }
}

/*
EXEMPLOS DE USO:

// 1. Campo básico
@FormCampo('Nome', 'texto', true, true)
public nome?: string;

// 2. Campo com validações e configurações avançadas
@FormCampo('Email', 'email', true, true, undefined, 8, false, {
  placeholder: 'Digite seu email',
  maxLength: 100,
  description: 'Email será usado para login',
  nzMd: 12,
  nzLg: 8
})
public email?: string;

// 3. Campo enum com opções
@FormCampo('Status', 'enum', true, true, StatusEnum)
@FiltroGrid('Status', 'enum', true, StatusEnum, 'equals')
public status?: StatusEnum;

// 4. Campo numérico com validações
@FormCampo('Idade', 'numero', true, true, undefined, 4, false, {
  min: 0,
  max: 120,
  step: 1,
  placeholder: 'Digite a idade'
})
public idade?: number;

// 5. Campo de data
@FormCampo('Data Nascimento', 'data', true, false, undefined, 6)
public dataNascimento?: Date;

// 6. Campo readonly
@FormCampo('ID', 'numero', false, false, undefined, 4, true)
public id?: number;

// 7. Campo textarea
@FormCampo('Observações', 'textarea', true, false, undefined, 12, false, {
  rows: 4,
  maxLength: 500,
  placeholder: 'Digite observações adicionais'
})
public observacoes?: string;

// 8. Campo booleano
@FormCampo('Ativo', 'bool', true, false)
@FiltroGrid('Ativo', 'bool', true)
public ativo?: boolean;

// 9. Campo de telefone
@FormCampo('Telefone', 'telefone', true, false, undefined, 6, false, {
  placeholder: '(00) 00000-0000'
})
public telefone?: string;

// 10. Campo de entidade (busca)
@FormCampo('Cliente', 'entidade', true, true, undefined, 8, false, {
  placeholder: 'Selecione o cliente'
})
public clienteId?: number;

// USO DAS FUNÇÕES UTILITÁRIAS:

const veiculo = new Veiculo();

// Obter todos os campos
const campos = DecoratorUtils.getFormFields(veiculo);
const filtros = DecoratorUtils.getFilterFields(veiculo);

// Obter campos visíveis
const camposVisiveis = DecoratorUtils.getVisibleFormFields(veiculo);
const filtrosVisiveis = DecoratorUtils.getVisibleFilterFields(veiculo);

// Obter campo específico
const campoNome = DecoratorUtils.getFormField(veiculo, 'nome');
const filtroStatus = DecoratorUtils.getFilterField(veiculo, 'status');

// Verificar se tem campos
const temCampos = DecoratorUtils.hasFormFields(veiculo);
const temFiltros = DecoratorUtils.hasFilterFields(veiculo);
*/
