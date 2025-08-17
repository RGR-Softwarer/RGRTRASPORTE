import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Passageiro } from '../../../../dominio/entidade/passageiro';

@Injectable({
  providedIn: 'root'
})
export class PassageiroStateService {
  private passageirosSubject = new BehaviorSubject<Passageiro[]>([]);
  private loadingSubject = new BehaviorSubject<boolean>(false);
  private errorSubject = new BehaviorSubject<string | null>(null);

  // Observables públicos
  public passageiros$ = this.passageirosSubject.asObservable();
  public isLoading$ = this.loadingSubject.asObservable();
  public error$ = this.errorSubject.asObservable();

  // Getters para valores atuais
  get passageiros(): Passageiro[] {
    return this.passageirosSubject.value;
  }

  get isLoading(): boolean {
    return this.loadingSubject.value;
  }

  get error(): string | null {
    return this.errorSubject.value;
  }

  // Setters para atualizar o estado
  setPassageiros(passageiros: Passageiro[]): void {
    this.passageirosSubject.next(passageiros);
    this.clearError();
  }

  addPassageiro(passageiro: Passageiro): void {
    const currentPassageiros = [...this.passageiros, passageiro];
    this.passageirosSubject.next(currentPassageiros);
    this.clearError();
  }

  updatePassageiro(passageiro: Passageiro): void {
    const currentPassageiros = this.passageiros.map(p => 
      p.id === passageiro.id ? passageiro : p
    );
    this.passageirosSubject.next(currentPassageiros);
    this.clearError();
  }

  removePassageiro(id: number): void {
    const currentPassageiros = this.passageiros.filter(p => p.id !== id);
    this.passageirosSubject.next(currentPassageiros);
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
    this.passageirosSubject.next([]);
    this.loadingSubject.next(false);
    this.errorSubject.next(null);
  }

  // Buscar passageiro por ID no estado atual
  getPassageiroById(id: number): Passageiro | undefined {
    return this.passageiros.find(passageiro => passageiro.id === id);
  }
}