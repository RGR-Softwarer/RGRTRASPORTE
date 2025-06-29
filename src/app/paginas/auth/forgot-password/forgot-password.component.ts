import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NotificationService } from '../../../shared/services/notification.service';

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
    private notificationService: NotificationService
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
        this.notificationService.success('Sucesso', 'Solicitação enviada com sucesso!');
        this.router.navigate(['/auth/login']);
      }, 2000);
    } else {
      this.notificationService.error('Erro', 'Por favor, preencha um email válido');
    }
  }

  voltarParaLogin(): void {
    this.router.navigate(['/auth/login']);
  }
} 