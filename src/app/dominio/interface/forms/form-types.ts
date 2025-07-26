import { FormControl, FormGroup } from '@angular/forms';

// Tipo genérico para formulários tipados
export type TypedFormGroup<T> = FormGroup<{
  [K in keyof T]: FormControl<T[K] | null>;
}>;

// Tipo para campos de formulário opcionais
export type PartialFormGroup<T> = FormGroup<{
  [K in keyof T]: FormControl<T[K] | null>;
}>;

// Interface para validação de formulários
export interface FormValidationError {
  field: string;
  message: string;
  code: string;
}

// Interface para estado do formulário
export interface FormState {
  isValid: boolean;
  isDirty: boolean;
  isTouched: boolean;
  isSubmitting: boolean;
  errors: FormValidationError[];
}

// Interface para opções de campo select
export interface SelectOption<T = any> {
  value: T;
  label: string;
  disabled?: boolean;
  group?: string;
}

// Interface para configuração de campo
export interface FieldConfig {
  required?: boolean;
  disabled?: boolean;
  placeholder?: string;
  maxLength?: number;
  minLength?: number;
  min?: number;
  max?: number;
  pattern?: string | RegExp;
  rows?: number; // Para textarea
  multiple?: boolean; // Para select
  options?: SelectOption[]; // Para select/radio
}

// Interface para metadados de campo com decorator
export interface FieldMetadata {
  key: string;
  label: string;
  type: 'text' | 'number' | 'email' | 'password' | 'textarea' | 'select' | 'checkbox' | 'radio' | 'date' | 'datetime' | 'time';
  visible: boolean;
  required: boolean;
  config?: FieldConfig;
  validationMessages?: Record<string, string>;
} 