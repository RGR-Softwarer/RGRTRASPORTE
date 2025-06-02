import 'reflect-metadata';

export interface FormCamposMetadata {
  key: string;
  label: string;
  type: string;
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
}

export interface FormCampoOption {
  label: string;
  value: any;
}

export interface FormCampoConstrutor {
  new (...args: any[]): {}; 
  formFields?: FormCamposMetadata[]; 
  filterFields?: FiltroMetadata[];
}

// Interfaces para filtros
export interface FiltroMetadata {
  key: string;
  label: string;
  type: 'texto' | 'numero' | 'enum' | 'bool' | 'data';
  visible: boolean;
  options: FormCampoOption[];
  placeholder?: string;
  operador?: 'contains' | 'equals' | 'in' | 'between';
}

export interface FiltroConstrutor {
  new (...args: any[]): {};
  filterFields?: FiltroMetadata[];
}

export function FormCampo(
  label: string, 
  type: 'texto' | 'numero' | 'enum' | 'entidade' | 'bool' | 'data' | 'textarea' | 'email' | 'telefone', 
  visible: boolean = true, 
  required: boolean = false, 
  enumType?: any, 
  colSpan16: number = 4,
  readonly: boolean = false
) {
  return function(target: any, propertyName: string) {
    const nzSpan = colSpan16 / 16 * 24;

    let options: FormCampoOption[] = [];

    if(type === 'enum' && enumType) {
      options = Object.entries(enumType).map(([key, label]) => ({ 
        label: label as string, 
        value: label 
      })) as FormCampoOption[];
    }

    const constructor = target.constructor as FormCampoConstrutor;
    if (!constructor.formFields) {
      constructor.formFields = [];
    }

    constructor.formFields.push({ 
      key: propertyName, 
      label, 
      type, 
      required, 
      readonly,
      nzSpan, 
      visible, 
      options 
    });
  };
}

// Decorator para filtros
export function FiltroGrid(
  label: string,
  type: 'texto' | 'numero' | 'enum' | 'bool' | 'data',
  visible: boolean = true,
  enumType?: any,
  operador: 'contains' | 'equals' | 'in' | 'between' = 'contains'
) {
  return function(target: any, propertyName: string) {
    let options: FormCampoOption[] = [];

    if(type === 'enum' && enumType) {
      options = Object.entries(enumType).map(([key, label]) => ({ 
        label: label as string, 
        value: label 
      })) as FormCampoOption[];
    }

    const constructor = target.constructor as FiltroConstrutor;
    if (!constructor.filterFields) {
      constructor.filterFields = [];
    }

    constructor.filterFields.push({ 
      key: propertyName, 
      label, 
      type, 
      visible, 
      options,
      operador,
      placeholder: `Filtrar por ${label.toLowerCase()}`
    });
  };
}
