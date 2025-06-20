import { Injectable } from '@angular/core';
import { VeiculoApiService } from './veiculo-api.service';
import { VeiculoStateService } from './veiculo-state.service';
import { Veiculo } from '../../../../dominio/entidade/veiculo';
import { tap, catchError } from 'rxjs/operators';
import { EMPTY, Observable } from 'rxjs';
import { ToastService } from '../../../../services/utils/notificacao/toast.service';

@Injectable({
  providedIn: 'root'
})
export class VeiculoFacade {
  // Expondo o estado para os componentes
  veiculos$ = this.state.veiculos$;
  isLoading$ = this.state.isLoading$;

  constructor(
    private api: VeiculoApiService,
    private state: VeiculoStateService,
    private toast: ToastService
  ) {}

  carregarVeiculos() {
    this.state.setLoading(true);
    this.api.buscarTodos().pipe(
      tap(veiculos => this.state.setVeiculos(veiculos)),
      catchError(err => {
        this.state.setError(err);
        this.toast.exibirMensagemErro('Erro', 'Não foi possível carregar os veículos.');
        return EMPTY;
      })
    ).subscribe();
  }

  salvarVeiculo(veiculo: Veiculo): Observable<Veiculo> {
    this.state.setLoading(true);
    return this.api.salvar(veiculo).pipe(
      tap(veiculoSalvo => {
        if (veiculo.id) {
          this.state.updateVeiculo(veiculoSalvo);
          this.toast.exibirMensagemSucesso('Sucesso', 'Veículo atualizado com sucesso!');
        } else {
          this.state.addVeiculo(veiculoSalvo);
          this.toast.exibirMensagemSucesso('Sucesso', 'Veículo adicionado com sucesso!');
        }
        this.state.setLoading(false);
      }),
      catchError(err => {
        this.state.setError(err);
        this.toast.exibirMensagemErro('Erro', 'Não foi possível salvar o veículo.');
        return EMPTY;
      })
    );
  }

  deletarVeiculo(id: number) {
    this.state.setLoading(true);
    this.api.deletar(id).pipe(
      tap(() => {
        this.state.removeVeiculo(id);
        this.toast.exibirMensagemSucesso('Sucesso', 'Veículo deletado com sucesso!');
        this.state.setLoading(false);
      }),
      catchError(err => {
        this.state.setError(err);
        this.toast.exibirMensagemErro('Erro', 'Não foi possível deletar o veículo.');
        return EMPTY;
      })
    ).subscribe();
  }
} 