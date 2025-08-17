import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Localidade } from '../../../../dominio/entidade/localidade';

@Injectable({
  providedIn: 'root'
})
export class LocalidadeStateService {
  private localidadesSubject = new BehaviorSubject<Localidade[]>([]);
  private loadingSubject = new BehaviorSubject<boolean>(false);
  private errorSubject = new BehaviorSubject<string | null>(null);

  // Observables públicos
  public localidades$ = this.localidadesSubject.asObservable();
  public isLoading$ = this.loadingSubject.asObservable();
  public error$ = this.errorSubject.asObservable();

  // Getters para valores atuais
  get localidades(): Localidade[] {
    return this.localidadesSubject.value;
  }

  get isLoading(): boolean {
    return this.loadingSubject.value;
  }

  get error(): string | null {
    return this.errorSubject.value;
  }

  // Setters para atualizar o estado
  setLocalidades(localidades: Localidade[]): void {
    this.localidadesSubject.next(localidades);
    this.clearError();
  }

  addLocalidade(localidade: Localidade): void {
    const currentLocalidades = [...this.localidades, localidade];
    this.localidadesSubject.next(currentLocalidades);
    this.clearError();
  }

  updateLocalidade(localidade: Localidade): void {
    const currentLocalidades = this.localidades.map(v => 
      v.id === localidade.id ? localidade : v
    );
    this.localidadesSubject.next(currentLocalidades);
    this.clearError();
  }

  removeLocalidade(id: number): void {
    const currentLocalidades = this.localidades.filter(v => v.id !== id);
    this.localidadesSubject.next(currentLocalidades);
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
    this.localidadesSubject.next([]);
    this.loadingSubject.next(false);
    this.errorSubject.next(null);
  }

  // Buscar localidade por ID no estado atual
  getLocalidadeById(id: number): Localidade | undefined {
    return this.localidades.find(localidade => localidade.id === id);
  }
}