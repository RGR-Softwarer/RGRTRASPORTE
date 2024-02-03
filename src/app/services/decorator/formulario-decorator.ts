export interface FormCamposMetadata {
  key: string;
  label: string;
  type: string;
  required: boolean;
  nzSpan : number;
  visible: boolean;
}

export interface FormCampoConstrutor {
  new (...args: any[]): {}; 
  formFields?: FormCamposMetadata[]; 
}

export function FormCampo(label: string, type: string, visible: boolean = true, required: boolean = false, colSpan16: number = 4) {
  
  const nzSpan = colSpan16 / 16 * 24; 

  return function(target: any, key: string) {
    const constructor = target.constructor as FormCampoConstrutor;
    if (!constructor.formFields) {
      constructor.formFields = [];
    }
    constructor.formFields.push({ key, label, type, required, nzSpan, visible });
  };
}
