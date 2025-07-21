import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';

export interface ConfirmConfig {
  title?: string;
  content: string;
  okText?: string;
  cancelText?: string;
  okType?: 'primary' | 'danger' | 'default';
  cancelType?: 'primary' | 'danger' | 'default';
  width?: string | number;
  centered?: boolean;
  closable?: boolean;
  maskClosable?: boolean;
  icon?: string;
}

export interface ConfirmResult {
  confirmed: boolean;
  data?: any;
}

@Injectable({
  providedIn: 'root'
})
export class ConfirmService {
  private confirmSubject = new Subject<ConfirmConfig>();
  private resultSubject = new Subject<ConfirmResult>();

  constructor() {}

  // Abrir confirmação
  confirm(config: ConfirmConfig): Observable<boolean> {
    const resultSubject = new Subject<boolean>();
    
    this.confirmSubject.next({
      title: config.title || 'Confirmar',
      content: config.content,
      okText: config.okText || 'OK',
      cancelText: config.cancelText || 'Cancelar',
      okType: config.okType || 'primary',
      cancelType: config.cancelType || 'default',
      width: config.width || 416,
      centered: config.centered ?? true,
      closable: config.closable ?? true,
      maskClosable: config.maskClosable ?? true,
      icon: config.icon || 'question-circle'
    });

    return resultSubject.asObservable();
  }

  // Métodos de conveniência para confirmação
  confirmDelete(content: string = 'Tem certeza que deseja excluir este item?'): Observable<boolean> {
    return this.confirm({
      title: 'Confirmar Exclusão',
      content,
      okText: 'Excluir',
      cancelText: 'Cancelar',
      okType: 'danger',
      icon: 'exclamation-triangle'
    });
  }

  confirmSave(content: string = 'Deseja salvar as alterações?'): Observable<boolean> {
    return this.confirm({
      title: 'Confirmar Salvamento',
      content,
      okText: 'Salvar',
      cancelText: 'Cancelar',
      okType: 'primary',
      icon: 'save'
    });
  }

  confirmAction(title: string, content: string): Observable<boolean> {
    return this.confirm({
      title,
      content,
      okText: 'Confirmar',
      cancelText: 'Cancelar',
      okType: 'primary',
      icon: 'question-circle'
    });
  }

  // Informação
  info(title: string, content: string): Observable<boolean> {
    return this.confirm({
      title,
      content,
      okText: 'OK',
      cancelText: undefined,
      okType: 'primary',
      icon: 'info-circle'
    });
  }

  // Sucesso
  success(title: string, content: string): Observable<boolean> {
    return this.confirm({
      title,
      content,
      okText: 'OK',
      cancelText: undefined,
      okType: 'primary',
      icon: 'check-circle'
    });
  }

  // Erro
  error(title: string, content: string): Observable<boolean> {
    return this.confirm({
      title,
      content,
      okText: 'OK',
      cancelText: undefined,
      okType: 'primary',
      icon: 'exclamation-circle'
    });
  }

  // Aviso
  warning(title: string, content: string): Observable<boolean> {
    return this.confirm({
      title,
      content,
      okText: 'OK',
      cancelText: undefined,
      okType: 'primary',
      icon: 'exclamation-triangle'
    });
  }

  // Stream para componente de confirmação
  getConfirmStream(): Observable<ConfirmConfig> {
    return this.confirmSubject.asObservable();
  }

  // Método para responder à confirmação
  respond(confirmed: boolean, data?: any) {
    this.resultSubject.next({ confirmed, data });
  }

  // Stream para resultados
  getResultStream(): Observable<ConfirmResult> {
    return this.resultSubject.asObservable();
  }
} 