import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastService } from '../../../services/utils/notificacao/toast.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent {
  forgotPasswordForm: FormGroup;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private toastService: ToastService
  ) {
    this.forgotPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  onSubmit(): void {
    if (this.forgotPasswordForm.valid) {
      this.isLoading = true;
      
      // Simular envio de email de recuperação
      setTimeout(() => {
        this.isLoading = false;
        this.toastService.exibirMensagemSucesso(
          'Recuperação de Senha', 
          'Se o email existir em nossa base, você receberá as instruções para redefinir sua senha.'
        );
        this.router.navigate(['/auth/login']);
      }, 2000);
    } else {
      this.toastService.exibirMensagemErro('Erro', 'Por favor, preencha um email válido');
    }
  }

  voltarParaLogin(): void {
    this.router.navigate(['/auth/login']);
  }
} 