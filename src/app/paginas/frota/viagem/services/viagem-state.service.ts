import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Viagem } from '../../../../dominio/entidade/viagem';

@Injectable({
  providedIn: 'root'
})
export class ViagemStateService {
  private viagensSubject = new BehaviorSubject<Viagem[]>([]);
  private loadingSubject = new BehaviorSubject<boolean>(false);
  private errorSubject = new BehaviorSubject<string | null>(null);

  // Observables públicos
  public viagens$ = this.viagensSubject.asObservable();
  public isLoading$ = this.loadingSubject.asObservable();
  public error$ = this.errorSubject.asObservable();

  // Getters para valores atuais
  get viagens(): Viagem[] {
    return this.viagensSubject.value;
  }

  get isLoading(): boolean {
    return this.loadingSubject.value;
  }

  get error(): string | null {
    return this.errorSubject.value;
  }

  // Setters para atualizar o estado
  setViagens(viagens: Viagem[]): void {
    this.viagensSubject.next(viagens);
    this.clearError();
  }

  addViagem(viagem: Viagem): void {
    const currentViagens = [...this.viagens, viagem];
    this.viagensSubject.next(currentViagens);
    this.clearError();
  }

  updateViagem(viagem: Viagem): void {
    const currentViagens = this.viagens.map(v => 
      v.id === viagem.id ? viagem : v
    );
    this.viagensSubject.next(currentViagens);
    this.clearError();
  }

  removeViagem(id: number): void {
    const currentViagens = this.viagens.filter(v => v.id !== id);
    this.viagensSubject.next(currentViagens);
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
    this.viagensSubject.next([]);
    this.loadingSubject.next(false);
    this.errorSubject.next(null);
  }

  // Buscar viagem por ID no estado atual
  getViagemById(id: number): Viagem | undefined {
    return this.viagens.find(viagem => viagem.id === id);
  }
}