import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, finalize, tap } from 'rxjs/operators';
import { ModeloVeicularApiService, ModeloVeicularFilter, ModeloVeicularPaginatedQuery } from './modelo-veicular-api.service';
import { ModeloVeicularStateService } from './modelo-veicular-state.service';
import { ModeloVeicular } from '../../../../dominio/entidade/veiculo/modelo-veicular';
import { NotificationService } from '../../../../shared/services/notification.service';

@Injectable({
  providedIn: 'root'
})
export class ModeloVeicularFacade {
  
  // Expor observables do state service
  public modelos$ = this.stateService.modelos$;
  public isLoading$ = this.stateService.isLoading$;
  public error$ = this.stateService.error$;

  constructor(
    private apiService: ModeloVeicularApiService,
    private stateService: ModeloVeicularStateService,
    private notificationService: NotificationService
  ) {}

  /**
   * Carrega todos os modelos veiculares
   */
  carregarModelos(): void {
    this.stateService.setLoading(true);
    
    this.apiService.buscarTodos().pipe(
      tap(modelos => {
        this.stateService.setModelos(modelos);
      }),
      catchError(error => {
        this.stateService.setError('Erro ao carregar modelos veiculares');
        this.notificationService.error('Erro', 'Falha ao carregar modelos veiculares');
        console.error('Erro ao carregar modelos veiculares:', error);
        return of([]);
      }),
      finalize(() => this.stateService.setLoading(false))
    ).subscribe();
  }

  /**
   * Busca um modelo veicular específico por ID
   * @param id ID do modelo veicular
   */
  buscarModeloPorId(id: number): Observable<ModeloVeicular> {
    this.stateService.setLoading(true);
    
    return this.apiService.buscarPorId(id).pipe(
      catchError(error => {
        this.stateService.setError('Erro ao buscar modelo veicular');
        this.notificationService.error('Erro', 'Falha ao buscar modelo veicular');
        console.error('Erro ao buscar modelo veicular:', error);
        throw error;
      }),
      finalize(() => this.stateService.setLoading(false))
    );
  }

  /**
   * Cria um novo modelo veicular
   * @param modelo Dados do modelo veicular
   */
  criarModelo(modelo: Omit<ModeloVeicular, 'id'>): Observable<ModeloVeicular> {
    this.stateService.setLoading(true);
    
    return this.apiService.criar(modelo).pipe(
      tap(novoModelo => {
        this.stateService.addModelo(novoModelo);
        this.notificationService.success('Sucesso', 'Modelo veicular criado com sucesso!');
      }),
      catchError(error => {
        this.stateService.setError('Erro ao criar modelo veicular');
        this.notificationService.error('Erro', 'Falha ao criar modelo veicular');
        console.error('Erro ao criar modelo veicular:', error);
        throw error;
      }),
      finalize(() => this.stateService.setLoading(false))
    );
  }

  /**
   * Atualiza um modelo veicular existente
   * @param id ID do modelo veicular
   * @param modelo Dados atualizados
   */
  atualizarModelo(id: number, modelo: Partial<ModeloVeicular>): Observable<ModeloVeicular> {
    this.stateService.setLoading(true);
    
    return this.apiService.atualizar(id, modelo).pipe(
      tap(modeloAtualizado => {
        this.stateService.updateModelo(modeloAtualizado);
        this.notificationService.success('Sucesso', 'Modelo veicular atualizado com sucesso!');
      }),
      catchError(error => {
        this.stateService.setError('Erro ao atualizar modelo veicular');
        this.notificationService.error('Erro', 'Falha ao atualizar modelo veicular');
        console.error('Erro ao atualizar modelo veicular:', error);
        throw error;
      }),
      finalize(() => this.stateService.setLoading(false))
    );
  }

  /**
   * Salva um modelo veicular (cria se novo, atualiza se existente)
   * @param modelo Dados do modelo veicular
   */
  salvarModelo(modelo: ModeloVeicular): Observable<ModeloVeicular> {
    if (modelo.id) {
      return this.atualizarModelo(modelo.id, modelo);
    }
    return this.criarModelo(modelo);
  }

  /**
   * Deleta um modelo veicular
   * @param id ID do modelo veicular
   */
  deletarModelo(id: number): void {
    if (!confirm('Tem certeza que deseja deletar este modelo veicular?')) {
      return;
    }

    this.stateService.setLoading(true);
    
    this.apiService.deletar(id).pipe(
      tap(() => {
        this.stateService.removeModelo(id);
        this.notificationService.success('Sucesso', 'Modelo veicular deletado com sucesso!');
      }),
      catchError(error => {
        this.stateService.setError('Erro ao deletar modelo veicular');
        this.notificationService.error('Erro', 'Falha ao deletar modelo veicular');
        console.error('Erro ao deletar modelo veicular:', error);
        return of(null);
      }),
      finalize(() => this.stateService.setLoading(false))
    ).subscribe();
  }

  /**
   * Busca modelos com filtros avançados
   * @param query Query de busca paginada
   */
  buscarComFiltros(query: ModeloVeicularPaginatedQuery): Observable<any> {
    this.stateService.setLoading(true);
    
    return this.apiService.buscarComFiltros(query).pipe(
      catchError(error => {
        this.stateService.setError('Erro ao buscar modelos veiculares');
        this.notificationService.error('Erro', 'Falha ao buscar modelos veiculares');
        console.error('Erro ao buscar modelos veiculares:', error);
        return of({ items: [], totalCount: 0 });
      }),
      finalize(() => this.stateService.setLoading(false))
    );
  }

  /**
   * Popula dados iniciais (seed)
   * @param forcarRecriacao Se deve forçar a recriação dos dados
   */
  popularDadosIniciais(forcarRecriacao: boolean = false): void {
    this.stateService.setLoading(true);
    
    this.apiService.seed(forcarRecriacao).pipe(
      tap(() => {
        this.notificationService.success('Sucesso', 'Dados iniciais populados com sucesso!');
        this.carregarModelos(); // Recarrega os dados após o seed
      }),
      catchError(error => {
        this.stateService.setError('Erro ao popular dados iniciais');
        this.notificationService.error('Erro', 'Falha ao popular dados iniciais');
        console.error('Erro ao popular dados iniciais:', error);
        return of(null);
      }),
      finalize(() => this.stateService.setLoading(false))
    ).subscribe();
  }

  /**
   * Limpa o estado e recarrega os dados
   */
  refresh(): void {
    this.stateService.reset();
    this.carregarModelos();
  }
}