import 'reflect-metadata';

export interface FormCamposMetadata {
  key: string;
  label: string;
  type: string;
  required: boolean;
  nzSpan : number;
  visible: boolean;
  options: FormCampoOption[];
}

export interface FormCampoOption {
  label: string;
  value: any;
}

export interface FormCampoConstrutor {
  new (...args: any[]): {}; 
  formFields?: FormCamposMetadata[]; 
}

export function FormCampo(label: string, type: 'texto' | 'numero' | 'enum'| 'entidade', visible: boolean = true, required: boolean = false, enumType?: any, colSpan16: number = 4) {
  return function(target: any, propertyName: string) {
    const nzSpan = colSpan16 / 16 * 24;

    let options: FormCampoOption[] = [];

    if(type === 'enum' && enumType) {
      options = Object.entries(enumType).map(([key, label]) => ({ label, value: key })) as FormCampoOption[];
    }

    const constructor = target.constructor as FormCampoConstrutor;
    if (!constructor.formFields) {
      constructor.formFields = [];
    }

    constructor.formFields.push({ key: propertyName, label, type, required, nzSpan, visible, options });
  };
}
