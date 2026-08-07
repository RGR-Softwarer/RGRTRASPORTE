import { Component, ElementRef, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import { FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { PasswordValidator } from '../../../services/validators/password.validator';
import { LoginService } from '../../../services/login/login.service';
import { NotificationService } from '../../../shared/services/notification.service';
import { TypedFormGroup } from '../../../dominio/interface/forms/form-types';

interface LoginFormData {
  email: string;
  senha: string;
  lembrar: boolean;
}

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule]
})
export class LoginComponent implements AfterViewInit, OnDestroy {
  private destroy$ = new Subject<void>();
  
  @ViewChild('emailInput') emailInputRef!: ElementRef<HTMLInputElement>;
  
  // Estados do componente
  loading: boolean = false;
  readonly emailRegex: string = '^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$';
  
  // Formulário tipado
  loginForm = new FormGroup({
    email: new FormControl<string>('', {
      nonNullable: true,
      validators: [
        Validators.required, 
        Validators.email, 
        Validators.pattern(this.emailRegex), 
        Validators.minLength(10)
      ]
    }),
    senha: new FormControl<string>('', {
      nonNullable: true,
      validators: [
        Validators.required, 
        PasswordValidator.validate
      ]
    }),
    lembrar: new FormControl<boolean>(false, { nonNullable: true })
  });

  constructor(
    private loginService: LoginService,
    private notificationService: NotificationService
  ) {}

  ngAfterViewInit(): void {
    // Foca no campo email após a view carregar
    setTimeout(() => {
      if (this.emailInputRef?.nativeElement) {
        this.emailInputRef.nativeElement.focus();
      }
    }, 100);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  async onSubmit(): Promise<void> {
    if (this.loading) {
      return;
    }

    if (!this.loginForm.valid) {
      this.marcarCamposInvalidos();
      this.notificationService.error('Erro', 'Por favor, corrija os erros do formulário');
      return;
    }

    this.loading = true;
    
    try {
      const formValue = this.loginForm.value as LoginFormData;
      
      if (!formValue.email || !formValue.senha) {
        throw new Error('Email e senha são obrigatórios');
      }

      await this.loginService.authenticate(formValue.email, formValue.senha);
      this.notificationService.success('Sucesso', 'Login realizado com sucesso!');
      
    } catch (error) {
      console.error('Erro no login:', error);
      this.notificationService.error('Erro', 'Credenciais inválidas ou erro no servidor');
    } finally {
      this.loading = false;
    }
  }

  private marcarCamposInvalidos(): void {
    Object.values(this.loginForm.controls).forEach(control => {
      if (control.invalid) {
        control.markAsDirty();
        control.markAsTouched();
        control.updateValueAndValidity({ onlySelf: true });
      }
    });
  }

  /**
   * Obtém mensagem de erro específica para o campo email
   */
  getEmailErrorMessage(): string {
    const emailControl = this.loginForm.get('email');
    
    if (emailControl?.hasError('required')) {
      return 'O email é obrigatório.';
    }
    if (emailControl?.hasError('email')) {
      return 'Digite um email válido.';
    }
    if (emailControl?.hasError('pattern')) {
      return 'Formato de email inválido.';
    }
    if (emailControl?.hasError('minlength')) {
      return 'Email deve ter pelo menos 10 caracteres.';
    }
    
    return '';
  }

  /**
   * Obtém mensagem de erro específica para o campo senha
   */
  getSenhaErrorMessage(): string {
    const senhaControl = this.loginForm.get('senha');
    
    if (senhaControl?.hasError('required')) {
      return 'A senha é obrigatória.';
    }
    if (senhaControl?.hasError('passwordValidator')) {
      return senhaControl.getError('passwordValidator').message;
    }
    
    return '';
  }

  /**
   * Verifica se um campo específico tem erros
   */
  hasFieldError(fieldName: string): boolean {
    const control = this.loginForm.get(fieldName);
    return !!(control?.errors && (control.dirty || control.touched));
  }

  /**
   * Limpa o formulário
   */
  limparFormulario(): void {
    this.loginForm.reset();
    this.loginForm.get('lembrar')?.setValue(false);
    
    setTimeout(() => {
      if (this.emailInputRef?.nativeElement) {
        this.emailInputRef.nativeElement.focus();
      }
    }, 100);
  }
}
