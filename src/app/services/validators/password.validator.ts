import { AbstractControl, ValidatorFn } from '@angular/forms';

export class PasswordValidator {
  static validate: ValidatorFn = (control: AbstractControl): { [key: string]: any } | null => {
    const password = control.value as string;

    if (password.length < 6) {
      return { passwordValidator: { message: 'A senha deve ter pelo menos 6 caracteres.' } };
    }   

    if (!/[A-Z]/.test(password)) {
      return { passwordValidator: { message: 'A senha deve conter pelo menos uma letra maiúscula.' } };
    }

    if (!/[!@#$%^&*()_+{}\[\]:;<>,.?~\\-]/.test(password)) {
      return { passwordValidator: { message: 'A senha deve conter pelo menos um caractere especial.' } };
    }

    return null;
  };
}
