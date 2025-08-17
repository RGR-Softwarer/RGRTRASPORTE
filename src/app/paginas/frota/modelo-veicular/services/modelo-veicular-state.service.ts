import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { ModeloVeicular } from '../../../../dominio/entidade/veiculo/modelo-veicular';

@Injectable({
  providedIn: 'root'
})
export class ModeloVeicularStateService {
  private modelosSubject = new BehaviorSubject<ModeloVeicular[]>([]);
  private loadingSubject = new BehaviorSubject<boolean>(false);
  private errorSubject = new BehaviorSubject<string | null>(null);

  // Observables públicos
  public modelos$ = this.modelosSubject.asObservable();
  public isLoading$ = this.loadingSubject.asObservable();
  public error$ = this.errorSubject.asObservable();

  // Getters para valores atuais
  get modelos(): ModeloVeicular[] {
    return this.modelosSubject.value;
  }

  get isLoading(): boolean {
    return this.loadingSubject.value;
  }

  get error(): string | null {
    return this.errorSubject.value;
  }

  // Setters para atualizar o estado
  setModelos(modelos: ModeloVeicular[]): void {
    this.modelosSubject.next(modelos);
    this.clearError();
  }

  addModelo(modelo: ModeloVeicular): void {
    const currentModelos = [...this.modelos, modelo];
    this.modelosSubject.next(currentModelos);
    this.clearError();
  }

  updateModelo(modelo: ModeloVeicular): void {
    const currentModelos = this.modelos.map(m => 
      m.id === modelo.id ? modelo : m
    );
    this.modelosSubject.next(currentModelos);
    this.clearError();
  }

  removeModelo(id: number): void {
    const currentModelos = this.modelos.filter(m => m.id !== id);
    this.modelosSubject.next(currentModelos);
    this.clearError();
  }

  setLoading(loading: boolean): void {
    this.loadingSubject.next(loading);
  }

  setError(error: string | null): void {
    this.errorSubject.next(error);
    this.setLoading(false);
  }

  clearError(): void {
    this.errorSubject.next(null);
  }

  // Limpar todo o estado
  reset(): void {
    this.modelosSubject.next([]);
    this.loadingSubject.next(false);
    this.errorSubject.next(null);
  }

  // Buscar modelo por ID no estado atual
  getModeloById(id: number): ModeloVeicular | undefined {
    return this.modelos.find(modelo => modelo.id === id);
  }
}