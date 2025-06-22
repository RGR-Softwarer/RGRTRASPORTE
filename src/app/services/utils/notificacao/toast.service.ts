import { Injectable } from '@angular/core';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzNotificationService } from 'ng-zorro-antd/notification';

export interface ToastConfig {
  title?: string;
  message: string;
  type: 'success' | 'error' | 'warning' | 'info';
  duration?: number;
  showIcon?: boolean;
  closable?: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  constructor(
    private messageService: NzMessageService,
    private notificationService: NzNotificationService
  ) {}

  // Métodos simplificados para mensagens rápidas (mantendo compatibilidade)
  exibirMensagemSucesso(titulo: string, conteudo: string): void {
    this.exibirNotificacao({
      message: conteudo,
      title: titulo,
      type: 'success',
      duration: 4000,
      showIcon: true,
      closable: true
    });
  }

  exibirMensagemErro(titulo: string, conteudo: string): void {
    this.exibirNotificacao({
      message: conteudo,
      title: titulo,
      type: 'error',
      duration: 6000,
      showIcon: true,
      closable: true
    });
  }

  exibirMensagemInfo(titulo: string, conteudo: string): void {
    this.exibirNotificacao({
      message: conteudo,
      title: titulo,
      type: 'info',
      duration: 4000,
      showIcon: true,
      closable: true
    });
  }

  exibirMensagemAviso(titulo: string, conteudo: string): void {
    this.exibirNotificacao({
      message: conteudo,
      title: titulo,
      type: 'warning',
      duration: 5000,
      showIcon: true,
      closable: true
    });
  }

  // Novos métodos com parâmetros opcionais
  exibirMensagemSucessoSimples(mensagem: string, titulo?: string): void {
    this.exibirNotificacao({
      message: mensagem,
      title: titulo || 'Sucesso',
      type: 'success',
      duration: 4000,
      showIcon: true,
      closable: true
    });
  }

  exibirMensagemErroSimples(mensagem: string, titulo?: string): void {
    this.exibirNotificacao({
      message: mensagem,
      title: titulo || 'Erro',
      type: 'error',
      duration: 6000,
      showIcon: true,
      closable: true
    });
  }

  exibirMensagemAvisoSimples(mensagem: string, titulo?: string): void {
    this.exibirNotificacao({
      message: mensagem,
      title: titulo || 'Aviso',
      type: 'warning',
      duration: 5000,
      showIcon: true,
      closable: true
    });
  }

  exibirMensagemInfoSimples(mensagem: string, titulo?: string): void {
    this.exibirNotificacao({
      message: mensagem,
      title: titulo || 'Informação',
      type: 'info',
      duration: 4000,
      showIcon: true,
      closable: true
    });
  }

  // Método principal para notificações avançadas
  exibirNotificacao(config: ToastConfig): void {
    const iconMap = {
      success: 'check-circle',
      error: 'close-circle',
      warning: 'exclamation-circle',
      info: 'info-circle'
    };

    const colorMap = {
      success: '#10b981',
      error: '#ef4444',
      warning: '#f59e0b',
      info: '#06b6d4'
    };

    this.notificationService.create(
      config.type,
      config.title || '',
      config.message,
      {
        nzDuration: config.duration,
        nzPlacement: 'topRight',
        nzStyle: {
          borderRadius: '12px',
          boxShadow: '0 4px 6px rgba(0, 0, 0, 0.07), 0 2px 4px rgba(0, 0, 0, 0.06)',
          border: `1px solid ${colorMap[config.type]}20`,
          background: `${colorMap[config.type]}02`
        },
        nzAnimate: true
      }
    );
  }

  // Método para mensagens simples (sem título)
  exibirMensagemSimples(mensagem: string, tipo: 'success' | 'error' | 'warning' | 'info' = 'info'): void {
    this.messageService.create(tipo, mensagem, {
      nzDuration: 3000
    });
  }

  // Método para notificações persistentes (não fecham automaticamente)
  exibirNotificacaoPersistente(config: ToastConfig): void {
    this.exibirNotificacao({
      ...config,
      duration: 0, // Não fecha automaticamente
      closable: true
    });
  }

  // Método para limpar todas as notificações
  limparTodasNotificacoes(): void {
    this.notificationService.remove();
    this.messageService.remove();
  }

  // Método para notificação de loading
  exibirLoading(mensagem: string = 'Carregando...'): string {
    return this.messageService.loading(mensagem, {
      nzDuration: 0
    }).messageId;
  }

  // Método para remover loading específico
  removerLoading(messageId: string): void {
    this.messageService.remove(messageId);
  }
}
