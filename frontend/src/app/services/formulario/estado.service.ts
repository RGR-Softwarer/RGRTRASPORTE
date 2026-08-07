import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { FormularioData } from './formulario.service';

export interface FormularioEstado {
  isLoading: boolean;
  isEditMode: boolean;
  formSubmitted: boolean;
  dados: FormularioData | null;
}

@Injectable({
  providedIn: 'root'
})
export class EstadoService {
  
  private estadoSubject = new BehaviorSubject<FormularioEstado>({
    isLoading: false,
    isEditMode: false,
    formSubmitted: false,
    dados: null
  });

  public estado$: Observable<FormularioEstado> = this.estadoSubject.asObservable();

  /**
   * Obtém o estado atual
   */
  getEstado(): FormularioEstado {
    return this.estadoSubject.value;
  }

  /**
   * Define o estado de loading
   */
  setLoading(isLoading: boolean): void {
    const estadoAtual = this.estadoSubject.value;
    this.estadoSubject.next({
      ...estadoAtual,
      isLoading
    });
  }

  /**
   * Define o modo de edição
   */
  setEditMode(isEditMode: boolean): void {
    const estadoAtual = this.estadoSubject.value;
    this.estadoSubject.next({
      ...estadoAtual,
      isEditMode
    });
  }

  /**
   * Define se o formulário foi submetido
   */
  setFormSubmitted(formSubmitted: boolean): void {
    const estadoAtual = this.estadoSubject.value;
    this.estadoSubject.next({
      ...estadoAtual,
      formSubmitted
    });
  }

  /**
   * Define os dados do formulário
   */
  setDados(dados: FormularioData | null): void {
    const estadoAtual = this.estadoSubject.value;
    this.estadoSubject.next({
      ...estadoAtual,
      dados
    });
  }

  /**
   * Atualiza múltiplas propriedades do estado
   */
  atualizarEstado(updates: Partial<FormularioEstado>): void {
    const estadoAtual = this.estadoSubject.value;
    this.estadoSubject.next({
      ...estadoAtual,
      ...updates
    });
  }

  /**
   * Reseta o estado para os valores padrão
   */
  resetarEstado(): void {
    this.estadoSubject.next({
      isLoading: false,
      isEditMode: false,
      formSubmitted: false,
      dados: null
    });
  }

  /**
   * Verifica se o formulário está em modo de edição
   */
  isEditMode(): boolean {
    return this.estadoSubject.value.isEditMode;
  }

  /**
   * Verifica se está carregando
   */
  isLoading(): boolean {
    return this.estadoSubject.value.isLoading;
  }

  /**
   * Verifica se o formulário foi submetido
   */
  isFormSubmitted(): boolean {
    return this.estadoSubject.value.formSubmitted;
  }

  /**
   * Obtém os dados atuais
   */
  getDados(): FormularioData | null {
    return this.estadoSubject.value.dados;
  }
} 