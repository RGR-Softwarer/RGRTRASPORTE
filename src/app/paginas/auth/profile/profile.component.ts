import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NotificationService } from '../../../shared/services/notification.service';
import { AppContextService } from '../../../services/context/app.context';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  profileForm: FormGroup;
  isLoading = false;
  user: any;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private notificationService: NotificationService,
    private appContextService: AppContextService
  ) {
    this.profileForm = this.fb.group({
      nome: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      telefone: ['', [Validators.required]],
      senhaAtual: [''],
      novaSenha: ['', [Validators.minLength(6)]],
      confirmarNovaSenha: ['']
    }, { validators: this.passwordMatchValidator });
  }

  ngOnInit(): void {
    this.carregarDadosUsuario();
  }

  private carregarDadosUsuario(): void {
    this.user = this.appContextService.obterUsuarioLogado();
    
    if (this.user) {
      this.profileForm.patchValue({
        nome: this.user.nome || '',
        email: this.user.email || '',
        telefone: this.user.telefone || ''
      });
    } else {
      this.router.navigate(['/auth/login']);
    }
  }

  passwordMatchValidator(form: FormGroup) {
    const novaSenha = form.get('novaSenha');
    const confirmarNovaSenha = form.get('confirmarNovaSenha');
    
    if (novaSenha && confirmarNovaSenha && novaSenha.value && confirmarNovaSenha.value && novaSenha.value !== confirmarNovaSenha.value) {
      confirmarNovaSenha.setErrors({ passwordMismatch: true });
      return { passwordMismatch: true };
    }
    
    return null;
  }

  onSubmit(): void {
    if (this.profileForm.valid) {
      this.isLoading = true;
      
      // Simular atualização
      setTimeout(() => {
        this.isLoading = false;
        this.notificationService.success('Sucesso', 'Perfil atualizado com sucesso!');
        
        // Atualizar dados no contexto
        if (this.user) {
          const dadosAtualizados = {
            ...this.user,
            nome: this.profileForm.get('nome')?.value,
            email: this.profileForm.get('email')?.value,
            telefone: this.profileForm.get('telefone')?.value
          };
          this.appContextService.definirUsuarioLogado(dadosAtualizados);
        }
      }, 2000);
    } else {
      this.notificationService.error('Erro', 'Por favor, preencha todos os campos corretamente');
    }
  }

  voltarParaHome(): void {
    this.router.navigate(['/']);
  }
} 