import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, finalize, tap } from 'rxjs/operators';
import { ViagemApiService, ViagemFilter, ViagemSearchParams } from './viagem-api.service';
import { ViagemStateService } from './viagem-state.service';
import { Viagem } from '../../../../dominio/entidade/viagem';
import { NotificationService } from '../../../../shared/services/notification.service';

@Injectable({
  providedIn: 'root'
})
export class ViagemFacade {
  
  // Expor observables do state service
  public viagens$ = this.stateService.viagens$;
  public isLoading$ = this.stateService.isLoading$;
  public error$ = this.stateService.error$;

  constructor(
    private apiService: ViagemApiService,
    private stateService: ViagemStateService,
    private notificationService: NotificationService
  ) {}

  /**
   * Carrega todas as viagens
   */
  carregarViagens(params?: ViagemSearchParams): void {
    this.stateService.setLoading(true);
    
    this.apiService.buscarTodos(params).pipe(
      tap(viagens => {
        this.stateService.setViagens(viagens);
      }),
      catchError(error => {
        this.stateService.setError('Erro ao carregar viagens');
        this.notificationService.error('Erro', 'Falha ao carregar viagens');
        console.error('Erro ao carregar viagens:', error);
        return of([]);
      }),
      finalize(() => this.stateService.setLoading(false))
    ).subscribe();
  }

  /**
   * Busca uma viagem específica por ID
   * @param id ID da viagem
   * @param auditado Se deve incluir dados de auditoria
   */
  buscarViagemPorId(id: number, auditado: boolean = false): Observable<Viagem> {
    this.stateService.setLoading(true);
    
    return this.apiService.buscarPorId(id, auditado).pipe(
      catchError(error => {
        this.stateService.setError('Erro ao buscar viagem');
        this.notificationService.error('Erro', 'Falha ao buscar viagem');
        console.error('Erro ao buscar viagem:', error);
        throw error;
      }),
      finalize(() => this.stateService.setLoading(false))
    );
  }

  /**
   * Salva uma viagem (cria se nova, atualiza se existente)
   * @param viagem Dados da viagem
   */
  salvarViagem(viagem: Viagem): Observable<Viagem> {
    this.stateService.setLoading(true);
    
    return this.apiService.salvar(viagem).pipe(
      tap(viagemSalva => {
        if (viagem.id) {
          this.stateService.updateViagem(viagemSalva);
          this.notificationService.success('Sucesso', 'Viagem atualizada com sucesso!');
        } else {
          this.stateService.addViagem(viagemSalva);
          this.notificationService.success('Sucesso', 'Viagem criada com sucesso!');
        }
      }),
      catchError(error => {
        this.stateService.setError('Erro ao salvar viagem');
        this.notificationService.error('Erro', 'Falha ao salvar viagem');
        console.error('Erro ao salvar viagem:', error);
        throw error;
      }),
      finalize(() => this.stateService.setLoading(false))
    );
  }

  /**
   * Cancela uma viagem
   * @param id ID da viagem
   * @param motivo Motivo do cancelamento
   */
  cancelarViagem(id: number, motivo?: string): void {
    if (!confirm('Tem certeza que deseja cancelar esta viagem?')) {
      return;
    }

    this.stateService.setLoading(true);
    
    this.apiService.cancelar(id, motivo).pipe(
      tap(() => {
        // Recarrega a viagem para obter o status atualizado
        this.buscarViagemPorId(id).subscribe(viagemAtualizada => {
          this.stateService.updateViagem(viagemAtualizada);
        });
        this.notificationService.success('Sucesso', 'Viagem cancelada com sucesso!');
      }),
      catchError(error => {
        this.stateService.setError('Erro ao cancelar viagem');
        this.notificationService.error('Erro', 'Falha ao cancelar viagem');
        console.error('Erro ao cancelar viagem:', error);
        return of(null);
      }),
      finalize(() => this.stateService.setLoading(false))
    ).subscribe();
  }

  /**
   * Inicia uma viagem
   * @param id ID da viagem
   */
  iniciarViagem(id: number): void {
    if (!confirm('Tem certeza que deseja iniciar esta viagem?')) {
      return;
    }

    this.stateService.setLoading(true);
    
    this.apiService.iniciar(id).pipe(
      tap(() => {
        // Recarrega a viagem para obter o status atualizado
        this.buscarViagemPorId(id).subscribe(viagemAtualizada => {
          this.stateService.updateViagem(viagemAtualizada);
        });
        this.notificationService.success('Sucesso', 'Viagem iniciada com sucesso!');
      }),
      catchError(error => {
        this.stateService.setError('Erro ao iniciar viagem');
        this.notificationService.error('Erro', 'Falha ao iniciar viagem');
        console.error('Erro ao iniciar viagem:', error);
        return of(null);
      }),
      finalize(() => this.stateService.setLoading(false))
    ).subscribe();
  }

  /**
   * Finaliza uma viagem
   * @param id ID da viagem
   */
  finalizarViagem(id: number): void {
    if (!confirm('Tem certeza que deseja finalizar esta viagem?')) {
      return;
    }

    this.stateService.setLoading(true);
    
    this.apiService.finalizar(id).pipe(
      tap(() => {
        // Recarrega a viagem para obter o status atualizado
        this.buscarViagemPorId(id).subscribe(viagemAtualizada => {
          this.stateService.updateViagem(viagemAtualizada);
        });
        this.notificationService.success('Sucesso', 'Viagem finalizada com sucesso!');
      }),
      catchError(error => {
        this.stateService.setError('Erro ao finalizar viagem');
        this.notificationService.error('Erro', 'Falha ao finalizar viagem');
        console.error('Erro ao finalizar viagem:', error);
        return of(null);
      }),
      finalize(() => this.stateService.setLoading(false))
    ).subscribe();
  }

  /**
   * Obtém a rota de uma viagem
   * @param id ID da viagem
   */
  obterRotaViagem(id: number): Observable<any> {
    this.stateService.setLoading(true);
    
    return this.apiService.obterRotaViagem(id).pipe(
      catchError(error => {
        this.stateService.setError('Erro ao obter rota da viagem');
        this.notificationService.error('Erro', 'Falha ao obter rota da viagem');
        console.error('Erro ao obter rota da viagem:', error);
        return of(null);
      }),
      finalize(() => this.stateService.setLoading(false))
    );
  }

  /**
   * Busca viagens com filtros
   * @param filters Filtros de busca
   */
  buscarComFiltros(filters: ViagemFilter): void {
    this.stateService.setLoading(true);
    
    this.apiService.buscarComFiltros(filters).pipe(
      tap(viagens => {
        this.stateService.setViagens(viagens);
      }),
      catchError(error => {
        this.stateService.setError('Erro ao buscar viagens');
        this.notificationService.error('Erro', 'Falha ao buscar viagens');
        console.error('Erro ao buscar viagens:', error);
        return of([]);
      }),
      finalize(() => this.stateService.setLoading(false))
    ).subscribe();
  }

  /**
   * Limpa o estado e recarrega os dados
   */
  refresh(): void {
    this.stateService.reset();
    this.carregarViagens();
  }
}