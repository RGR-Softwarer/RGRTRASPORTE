import { Injectable, ComponentRef, Type, Injector } from '@angular/core';
import { Subject, Observable } from 'rxjs';

export interface ModalConfig {
  title?: string;
  width?: string | number;
  height?: string | number;
  closable?: boolean;
  maskClosable?: boolean;
  data?: any;
  footer?: boolean;
  confirmText?: string;
  cancelText?: string;
  centered?: boolean;
  zIndex?: number;
  className?: string;
  style?: { [key: string]: string };
}

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
}

export interface ModalRef {
  close: (result?: any) => void;
  afterClose: () => Observable<any>;
  afterOpen: () => Observable<void>;
  getContentComponent: () => any;
}

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  private modalSubject = new Subject<{ component: Type<any>, config: ModalConfig, injector?: Injector }>();
  private modalRefs: ModalRef[] = [];
  private confirmSubject = new Subject<ConfirmConfig>();

  constructor() {}

  // Abrir modal com componente
  open<T>(component: Type<T>, config: ModalConfig = {}, injector?: Injector): ModalRef {
    const modalRef: ModalRef = {
      close: (result?: any) => this.closeModal(modalRef, result),
      afterClose: () => new Subject<any>().asObservable(),
      afterOpen: () => new Subject<void>().asObservable(),
      getContentComponent: () => null
    };

    this.modalRefs.push(modalRef);
    this.modalSubject.next({ component, config, injector });

    return modalRef;
  }

  // Abrir modal de confirmação
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
      maskClosable: config.maskClosable ?? true
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
      okType: 'danger'
    });
  }

  confirmSave(content: string = 'Deseja salvar as alterações?'): Observable<boolean> {
    return this.confirm({
      title: 'Confirmar Salvamento',
      content,
      okText: 'Salvar',
      cancelText: 'Cancelar',
      okType: 'primary'
    });
  }

  confirmAction(title: string, content: string): Observable<boolean> {
    return this.confirm({
      title,
      content,
      okText: 'Confirmar',
      cancelText: 'Cancelar',
      okType: 'primary'
    });
  }

  // Informação
  info(title: string, content: string): Observable<boolean> {
    return this.confirm({
      title,
      content,
      okText: 'OK',
      cancelText: undefined,
      okType: 'primary'
    });
  }

  // Sucesso
  success(title: string, content: string): Observable<boolean> {
    return this.confirm({
      title,
      content,
      okText: 'OK',
      cancelText: undefined,
      okType: 'primary'
    });
  }

  // Erro
  error(title: string, content: string): Observable<boolean> {
    return this.confirm({
      title,
      content,
      okText: 'OK',
      cancelText: undefined,
      okType: 'primary'
    });
  }

  // Aviso
  warning(title: string, content: string): Observable<boolean> {
    return this.confirm({
      title,
      content,
      okText: 'OK',
      cancelText: undefined,
      okType: 'primary'
    });
  }

  // Streams para componentes
  getModalStream(): Observable<{ component: Type<any>, config: ModalConfig, injector?: Injector }> {
    return this.modalSubject.asObservable();
  }

  getConfirmStream(): Observable<ConfirmConfig> {
    return this.confirmSubject.asObservable();
  }

  private closeModal(modalRef: ModalRef, result?: any) {
    const index = this.modalRefs.indexOf(modalRef);
    if (index > -1) {
      this.modalRefs.splice(index, 1);
    }
  }

  closeAll() {
    this.modalRefs.forEach(ref => ref.close());
  }

  // Método para obter modal ativo
  getActiveModal(): ModalRef | null {
    return this.modalRefs.length > 0 ? this.modalRefs[this.modalRefs.length - 1] : null;
  }

  // Método para verificar se há modais abertos
  hasOpenModals(): boolean {
    return this.modalRefs.length > 0;
  }
} 