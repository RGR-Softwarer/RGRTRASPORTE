import { Injectable } from '@angular/core';
import { VeiculoApiService } from './veiculo-api.service';
import { VeiculoStateService } from './veiculo-state.service';
import { Veiculo } from '../../../../dominio/entidade/veiculo';
import { tap, catchError } from 'rxjs/operators';
import { EMPTY, Observable } from 'rxjs';
import { NotificationService } from '../../../../shared/services/notification.service';

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
    private notificationService: NotificationService
  ) {}

  carregarVeiculos() {
    this.state.setLoading(true);
    this.api.buscarTodos().pipe(
      tap(veiculos => this.state.setVeiculos(veiculos)),
      catchError(err => {
        this.state.setError(err);
        this.notificationService.error('Erro', 'Não foi possível carregar os veículos.');
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
          this.notificationService.success('Sucesso', 'Veículo atualizado com sucesso!');
        } else {
          this.state.addVeiculo(veiculoSalvo);
          this.notificationService.success('Sucesso', 'Veículo adicionado com sucesso!');
        }
        this.state.setLoading(false);
      }),
      catchError(err => {
        this.state.setError(err);
        this.notificationService.error('Erro', 'Não foi possível salvar o veículo.');
        return EMPTY;
      })
    );
  }

  deletarVeiculo(id: number) {
    this.state.setLoading(true);
    this.api.deletar(id).pipe(
      tap(() => {
        this.state.removeVeiculo(id);
        this.notificationService.success('Sucesso', 'Veículo deletado com sucesso!');
        this.state.setLoading(false);
      }),
      catchError(err => {
        this.state.setError(err);
        this.notificationService.error('Erro', 'Não foi possível deletar o veículo.');
        return EMPTY;
      })
    ).subscribe();
  }
} 