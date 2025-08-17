import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, finalize, tap } from 'rxjs/operators';
import { LocalidadeApiService, LocalidadeFilter } from './localidade-api.service';
import { LocalidadeStateService } from './localidade-state.service';
import { Localidade } from '../../../../dominio/entidade/localidade';
import { NotificationService } from '../../../../shared/services/notification.service';

@Injectable({
  providedIn: 'root'
})
export class LocalidadeFacade {
  
  // Expor observables do state service
  public localidades$ = this.stateService.localidades$;
  public isLoading$ = this.stateService.isLoading$;
  public error$ = this.stateService.error$;

  constructor(
    private apiService: LocalidadeApiService,
    private stateService: LocalidadeStateService,
    private notificationService: NotificationService
  ) {}

  /**
   * Carrega todas as localidades
   */
  carregarLocalidades(): void {
    this.stateService.setLoading(true);
    
    this.apiService.buscarTodos().pipe(
      tap(localidades => {
        this.stateService.setLocalidades(localidades);
      }),
      catchError(error => {
        this.stateService.setError('Erro ao carregar localidades');
        this.notificationService.error('Erro', 'Falha ao carregar localidades');
        console.error('Erro ao carregar localidades:', error);
        return of([]);
      }),
      finalize(() => this.stateService.setLoading(false))
    ).subscribe();
  }

  /**
   * Busca uma localidade específica por ID
   * @param id ID da localidade
   */
  buscarLocalidadePorId(id: number): Observable<Localidade> {
    this.stateService.setLoading(true);
    
    return this.apiService.buscarPorId(id).pipe(
      catchError(error => {
        this.stateService.setError('Erro ao buscar localidade');
        this.notificationService.error('Erro', 'Falha ao buscar localidade');
        console.error('Erro ao buscar localidade:', error);
        throw error;
      }),
      finalize(() => this.stateService.setLoading(false))
    );
  }

  /**
   * Cria uma nova localidade
   * @param localidade Dados da localidade
   */
  criarLocalidade(localidade: Omit<Localidade, 'id'>): Observable<Localidade> {
    this.stateService.setLoading(true);
    
    return this.apiService.criar(localidade).pipe(
      tap(novaLocalidade => {
        this.stateService.addLocalidade(novaLocalidade);
        this.notificationService.success('Sucesso', 'Localidade criada com sucesso!');
      }),
      catchError(error => {
        this.stateService.setError('Erro ao criar localidade');
        this.notificationService.error('Erro', 'Falha ao criar localidade');
        console.error('Erro ao criar localidade:', error);
        throw error;
      }),
      finalize(() => this.stateService.setLoading(false))
    );
  }

  /**
   * Atualiza uma localidade existente
   * @param id ID da localidade
   * @param localidade Dados atualizados
   */
  atualizarLocalidade(id: number, localidade: Partial<Localidade>): Observable<Localidade> {
    this.stateService.setLoading(true);
    
    return this.apiService.atualizar(id, localidade).pipe(
      tap(localidadeAtualizada => {
        this.stateService.updateLocalidade(localidadeAtualizada);
        this.notificationService.success('Sucesso', 'Localidade atualizada com sucesso!');
      }),
      catchError(error => {
        this.stateService.setError('Erro ao atualizar localidade');
        this.notificationService.error('Erro', 'Falha ao atualizar localidade');
        console.error('Erro ao atualizar localidade:', error);
        throw error;
      }),
      finalize(() => this.stateService.setLoading(false))
    );
  }

  /**
   * Salva uma localidade (cria se nova, atualiza se existente)
   * @param localidade Dados da localidade
   */
  salvarLocalidade(localidade: Localidade): Observable<Localidade> {
    if (localidade.id) {
      return this.atualizarLocalidade(localidade.id, localidade);
    }
    return this.criarLocalidade(localidade);
  }

  /**
   * Deleta uma localidade
   * @param id ID da localidade
   */
  deletarLocalidade(id: number): void {
    if (!confirm('Tem certeza que deseja deletar esta localidade?')) {
      return;
    }

    this.stateService.setLoading(true);
    
    this.apiService.deletar(id).pipe(
      tap(() => {
        this.stateService.removeLocalidade(id);
        this.notificationService.success('Sucesso', 'Localidade deletada com sucesso!');
      }),
      catchError(error => {
        this.stateService.setError('Erro ao deletar localidade');
        this.notificationService.error('Erro', 'Falha ao deletar localidade');
        console.error('Erro ao deletar localidade:', error);
        return of(null);
      }),
      finalize(() => this.stateService.setLoading(false))
    ).subscribe();
  }

  /**
   * Busca localidades com filtros
   * @param filters Filtros de busca
   */
  buscarComFiltros(filters: LocalidadeFilter): void {
    this.stateService.setLoading(true);
    
    this.apiService.buscarComFiltros(filters).pipe(
      tap(localidades => {
        this.stateService.setLocalidades(localidades);
      }),
      catchError(error => {
        this.stateService.setError('Erro ao buscar localidades');
        this.notificationService.error('Erro', 'Falha ao buscar localidades');
        console.error('Erro ao buscar localidades:', error);
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
    this.carregarLocalidades();
  }
}