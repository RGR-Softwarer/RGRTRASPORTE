import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, finalize, tap } from 'rxjs/operators';
import { PassageiroApiService, PassageiroFilter, PassageiroPaginatedQuery, ObterPassageirosQuery } from './passageiro-api.service';
import { PassageiroStateService } from './passageiro-state.service';
import { Passageiro } from '../../../../dominio/entidade/passageiro';
import { NotificationService } from '../../../../shared/services/notification.service';

@Injectable({
  providedIn: 'root'
})
export class PassageiroFacade {
  
  // Expor observables do state service
  public passageiros$ = this.stateService.passageiros$;
  public isLoading$ = this.stateService.isLoading$;
  public error$ = this.stateService.error$;

  constructor(
    private apiService: PassageiroApiService,
    private stateService: PassageiroStateService,
    private notificationService: NotificationService
  ) {}

  /**
   * Carrega todos os passageiros
   */
  carregarPassageiros(query?: ObterPassageirosQuery): void {
    this.stateService.setLoading(true);
    
    this.apiService.buscarTodos(query).pipe(
      tap(passageiros => {
        this.stateService.setPassageiros(passageiros);
      }),
      catchError(error => {
        this.stateService.setError('Erro ao carregar passageiros');
        this.notificationService.error('Erro', 'Falha ao carregar passageiros');
        console.error('Erro ao carregar passageiros:', error);
        return of([]);
      }),
      finalize(() => this.stateService.setLoading(false))
    ).subscribe();
  }

  /**
   * Busca um passageiro específico por ID
   * @param id ID do passageiro
   */
  buscarPassageiroPorId(id: number): Observable<Passageiro> {
    this.stateService.setLoading(true);
    
    return this.apiService.buscarPorId(id).pipe(
      catchError(error => {
        this.stateService.setError('Erro ao buscar passageiro');
        this.notificationService.error('Erro', 'Falha ao buscar passageiro');
        console.error('Erro ao buscar passageiro:', error);
        throw error;
      }),
      finalize(() => this.stateService.setLoading(false))
    );
  }

  /**
   * Cria um novo passageiro
   * @param passageiro Dados do passageiro
   */
  criarPassageiro(passageiro: Omit<Passageiro, 'id'>): Observable<Passageiro> {
    this.stateService.setLoading(true);
    
    return this.apiService.criar(passageiro).pipe(
      tap(novoPassageiro => {
        this.stateService.addPassageiro(novoPassageiro);
        this.notificationService.success('Sucesso', 'Passageiro criado com sucesso!');
      }),
      catchError(error => {
        this.stateService.setError('Erro ao criar passageiro');
        this.notificationService.error('Erro', 'Falha ao criar passageiro');
        console.error('Erro ao criar passageiro:', error);
        throw error;
      }),
      finalize(() => this.stateService.setLoading(false))
    );
  }

  /**
   * Atualiza um passageiro existente
   * @param id ID do passageiro
   * @param passageiro Dados atualizados
   */
  atualizarPassageiro(id: number, passageiro: Partial<Passageiro>): Observable<Passageiro> {
    this.stateService.setLoading(true);
    
    return this.apiService.atualizar(id, passageiro).pipe(
      tap(passageiroAtualizado => {
        this.stateService.updatePassageiro(passageiroAtualizado);
        this.notificationService.success('Sucesso', 'Passageiro atualizado com sucesso!');
      }),
      catchError(error => {
        this.stateService.setError('Erro ao atualizar passageiro');
        this.notificationService.error('Erro', 'Falha ao atualizar passageiro');
        console.error('Erro ao atualizar passageiro:', error);
        throw error;
      }),
      finalize(() => this.stateService.setLoading(false))
    );
  }

  /**
   * Salva um passageiro (cria se novo, atualiza se existente)
   * @param passageiro Dados do passageiro
   */
  salvarPassageiro(passageiro: Passageiro): Observable<Passageiro> {
    if (passageiro.id) {
      return this.atualizarPassageiro(passageiro.id, passageiro);
    }
    return this.criarPassageiro(passageiro);
  }

  /**
   * Deleta um passageiro
   * @param id ID do passageiro
   */
  deletarPassageiro(id: number): void {
    if (!confirm('Tem certeza que deseja deletar este passageiro?')) {
      return;
    }

    this.stateService.setLoading(true);
    
    this.apiService.deletar(id).pipe(
      tap(() => {
        this.stateService.removePassageiro(id);
        this.notificationService.success('Sucesso', 'Passageiro deletado com sucesso!');
      }),
      catchError(error => {
        this.stateService.setError('Erro ao deletar passageiro');
        this.notificationService.error('Erro', 'Falha ao deletar passageiro');
        console.error('Erro ao deletar passageiro:', error);
        return of(null);
      }),
      finalize(() => this.stateService.setLoading(false))
    ).subscribe();
  }

  /**
   * Busca passageiros com filtros avançados
   * @param query Query de busca paginada
   */
  buscarComFiltros(query: PassageiroPaginatedQuery): Observable<any> {
    this.stateService.setLoading(true);
    
    return this.apiService.buscarComFiltros(query).pipe(
      catchError(error => {
        this.stateService.setError('Erro ao buscar passageiros');
        this.notificationService.error('Erro', 'Falha ao buscar passageiros');
        console.error('Erro ao buscar passageiros:', error);
        return of({ items: [], totalCount: 0 });
      }),
      finalize(() => this.stateService.setLoading(false))
    );
  }

  /**
   * Busca passageiros com filtros simples
   * @param filters Filtros de busca
   */
  buscarComFiltrosSimples(filters: PassageiroFilter): void {
    this.stateService.setLoading(true);
    
    this.apiService.buscarComFiltrosSimples(filters).pipe(
      tap(passageiros => {
        this.stateService.setPassageiros(passageiros);
      }),
      catchError(error => {
        this.stateService.setError('Erro ao buscar passageiros');
        this.notificationService.error('Erro', 'Falha ao buscar passageiros');
        console.error('Erro ao buscar passageiros:', error);
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
    this.carregarPassageiros();
  }
}