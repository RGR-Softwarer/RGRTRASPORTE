import { Component, ElementRef, ViewChild, AfterViewInit } from '@angular/core';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { PasswordValidator } from '../../../validators/PasswordValidator';
import { LoginService } from '../../../services/login/login.service';
import { ToastService } from '../../../services/utils/notificacao/toast.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements AfterViewInit {

  constructor(
    private loginService: LoginService,
  ) { }

  @ViewChild('emailInput') emailInputRef!: ElementRef<HTMLInputElement>;
  emailRegex: string = '^[a-zA-Z0-9._%+-]+@[a-zA-Z0.9.-]+\\.[a-zA-Z]{2,}$';

  loginForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email, Validators.pattern(this.emailRegex), Validators.minLength(10)]),
    senha: new FormControl('', [Validators.required, PasswordValidator.validate])
  });

  ngAfterViewInit(): void {
    if (this.emailInputRef?.nativeElement) {
      this.emailInputRef.nativeElement.focus();
    }
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      this.loginService.authenticate(this.loginForm.value.email!, this.loginForm.value.senha!)
      console.log('Form Data: ', this.loginForm.value);
    } else {
      console.log('Formulário inválido');
    }
  }

  getSenhaErrorMessage(): string {
    const senhaControl = this.loginForm.get('senha');
    if (senhaControl?.hasError('required')) {
      return 'A senha é obrigatória.';
    } else if (senhaControl?.hasError('passwordValidator')) {
      return senhaControl.getError('passwordValidator').message;
    }
    return '';
  }
}
