import { Injectable, ComponentRef, Type } from '@angular/core';
import { Subject } from 'rxjs';

export interface ModalConfig {
  title?: string;
  width?: string;
  closable?: boolean;
  data?: any;
}

export interface ModalRef {
  close: () => void;
  afterClose: () => Subject<any>;
}

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  private modalSubject = new Subject<{ component: Type<any>, config: ModalConfig }>();
  private modalRefs: ModalRef[] = [];

  constructor() {}

  open<T>(component: Type<T>, config: ModalConfig = {}): ModalRef {
    const modalRef: ModalRef = {
      close: () => this.closeModal(modalRef),
      afterClose: () => new Subject<any>()
    };

    this.modalRefs.push(modalRef);
    this.modalSubject.next({ component, config });

    return modalRef;
  }

  getModalStream() {
    return this.modalSubject.asObservable();
  }

  private closeModal(modalRef: ModalRef) {
    const index = this.modalRefs.indexOf(modalRef);
    if (index > -1) {
      this.modalRefs.splice(index, 1);
    }
  }

  closeAll() {
    this.modalRefs.forEach(ref => ref.close());
  }
} 