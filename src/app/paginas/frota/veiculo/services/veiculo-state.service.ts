import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { Veiculo } from '../../../../dominio/entidade/veiculo';

export interface VeiculoState {
  veiculos: Veiculo[];
  isLoading: boolean;
  error: any | null;
}

const initialState: VeiculoState = {
  veiculos: [],
  isLoading: false,
  error: null
};

@Injectable({
  providedIn: 'root'
})
export class VeiculoStateService {
  private readonly state$ = new BehaviorSubject<VeiculoState>(initialState);

  // Selectors - expõem o estado como Observables
  veiculos$ = this.state$.asObservable().pipe(map(state => state.veiculos));
  isLoading$ = this.state$.asObservable().pipe(map(state => state.isLoading));
  error$ = this.state$.asObservable().pipe(map(state => state.error));

  // Actions - métodos para alterar o estado
  setLoading(isLoading: boolean) {
    this.state$.next({ ...this.state$.value, isLoading });
  }

  setVeiculos(veiculos: Veiculo[]) {
    this.state$.next({ ...this.state$.value, veiculos, isLoading: false, error: null });
  }

  setError(error: any) {
    this.state$.next({ ...this.state$.value, error, isLoading: false });
  }

  addVeiculo(veiculo: Veiculo) {
      const veiculos = [...this.state$.value.veiculos, veiculo];
      this.state$.next({ ...this.state$.value, veiculos });
  }

  updateVeiculo(veiculoAtualizado: Veiculo) {
      const veiculos = this.state$.value.veiculos.map(v => v.id === veiculoAtualizado.id ? veiculoAtualizado : v);
      this.state$.next({ ...this.state$.value, veiculos });
  }

  removeVeiculo(id: number) {
      const veiculos = this.state$.value.veiculos.filter(v => v.id !== id);
      this.state$.next({ ...this.state$.value, veiculos });
  }
} 