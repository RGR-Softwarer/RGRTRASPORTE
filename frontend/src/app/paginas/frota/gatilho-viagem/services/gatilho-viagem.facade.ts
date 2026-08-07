import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { GatilhoViagemApiService } from './gatilho-viagem-api.service';
import { GatilhoViagemStateService } from './gatilho-viagem-state.service';
import { GatilhoViagem } from '../../../../dominio/entidade/gatilho-viagem';

@Injectable({ providedIn: 'root' })
export class GatilhoViagemFacade {
    // Expor observables do state
    gatilhos$ = this.stateService.gatilhos$;
    loading$ = this.stateService.loading$;
    gatilhoSelecionado$ = this.stateService.gatilhoSelecionado$;

    constructor(
        private apiService: GatilhoViagemApiService,
        private stateService: GatilhoViagemStateService
    ) {}

    async carregarTodos(): Promise<GatilhoViagem[]> {
        this.stateService.setLoading(true);
        try {
            const response = await firstValueFrom(this.apiService.obterTodos());
            const gatilhos = this.extrairDados(response);
            this.stateService.setGatilhos(gatilhos);
            return gatilhos;
        } catch (error) {
            console.error('Erro ao carregar gatilhos de viagem:', error);
            throw error;
        } finally {
            this.stateService.setLoading(false);
        }
    }

    async obterPorId(id: number): Promise<GatilhoViagem> {
        this.stateService.setLoading(true);
        try {
            const response = await firstValueFrom(this.apiService.obterPorId(id));
            const gatilho = this.extrairDados(response);
            this.stateService.setGatilhoSelecionado(gatilho);
            return gatilho;
        } catch (error) {
            console.error('Erro ao obter gatilho de viagem:', error);
            throw error;
        } finally {
            this.stateService.setLoading(false);
        }
    }

    async criar(gatilho: any): Promise<GatilhoViagem> {
        this.stateService.setLoading(true);
        try {
            const response = await firstValueFrom(this.apiService.criar(gatilho));
            const novoGatilho = this.extrairDados(response);
            this.stateService.adicionarGatilho(novoGatilho);
            return novoGatilho;
        } catch (error) {
            console.error('Erro ao criar gatilho de viagem:', error);
            throw error;
        } finally {
            this.stateService.setLoading(false);
        }
    }

    async atualizar(id: number, gatilho: any): Promise<GatilhoViagem> {
        this.stateService.setLoading(true);
        try {
            const response = await firstValueFrom(this.apiService.atualizar(id, gatilho));
            const gatilhoAtualizado = this.extrairDados(response);
            this.stateService.atualizarGatilho(gatilhoAtualizado);
            return gatilhoAtualizado;
        } catch (error) {
            console.error('Erro ao atualizar gatilho de viagem:', error);
            throw error;
        } finally {
            this.stateService.setLoading(false);
        }
    }

    async remover(id: number): Promise<void> {
        this.stateService.setLoading(true);
        try {
            await firstValueFrom(this.apiService.remover(id));
            this.stateService.removerGatilho(id);
        } catch (error) {
            console.error('Erro ao remover gatilho de viagem:', error);
            throw error;
        } finally {
            this.stateService.setLoading(false);
        }
    }

    selecionarGatilho(gatilho: GatilhoViagem | null): void {
        this.stateService.setGatilhoSelecionado(gatilho);
    }

    limparSelecao(): void {
        this.stateService.setGatilhoSelecionado(null);
    }

    private extrairDados(response: any): any {
        if (!response) return null;
        
        if (response.success && response.data !== undefined) {
            return response.data;
        }
        if (response.sucesso && response.dados !== undefined) {
            return response.dados;
        }
        if (response.data !== undefined) {
            return response.data;
        }
        if (response.dados !== undefined) {
            return response.dados;
        }
        
        return response;
    }
}







