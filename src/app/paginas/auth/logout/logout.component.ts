import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AppContextService } from '../../../services/context/app.context';
import { ToastService } from '../../../services/utils/notificacao/toast.service';

@Component({
  selector: 'app-logout',
  template: `
    <div class="logout-container">
      <div class="logout-content">
        <h2>Saindo do sistema...</h2>
        <p>Você será redirecionado para a página de login.</p>
      </div>
    </div>
  `,
  styles: [`
    .logout-container {
      display: flex;
      justify-content: center;
      align-items: center;
      height: 100vh;
      background-color: #f5f5f5;
    }
    .logout-content {
      text-align: center;
      padding: 2rem;
      background: white;
      border-radius: 8px;
      box-shadow: 0 2px 8px rgba(0,0,0,0.1);
    }
    h2 {
      color: #1890ff;
      margin-bottom: 1rem;
    }
    p {
      color: #666;
    }
  `]
})
export class LogoutComponent implements OnInit {

  constructor(
    private appContextService: AppContextService,
    private router: Router,
    private toastService: ToastService
  ) { }

  ngOnInit(): void {
    this.realizarLogout();
  }

  private realizarLogout(): void {
    // Limpar dados do usuário
    this.appContextService.logout();
    
    // Exibir mensagem de sucesso
    this.toastService.exibirMensagemSucesso('Logout', 'Você foi desconectado com sucesso');
    
    // Aguardar um momento e redirecionar
    setTimeout(() => {
      this.router.navigate(['/auth/login']);
    }, 1500);
  }
} 