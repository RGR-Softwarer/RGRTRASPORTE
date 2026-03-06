import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { MotoristaApiService } from './motorista-api.service';
import { MotoristaStateService } from './motorista-state.service';
import { Motorista } from '../../../../dominio/entidade/motorista';

@Injectable({ providedIn: 'root' })
export class MotoristaFacade {
    // Expor observables do state
    motoristas$ = this.stateService.motoristas$;
    loading$ = this.stateService.loading$;
    motoristaSelecionado$ = this.stateService.motoristaSelecionado$;

    constructor(
        private apiService: MotoristaApiService,
        private stateService: MotoristaStateService
    ) {}

    async carregarTodos(): Promise<Motorista[]> {
        this.stateService.setLoading(true);
        try {
            const response = await firstValueFrom(this.apiService.obterTodos());
            const motoristas = this.extrairDados(response);
            this.stateService.setMotoristas(motoristas);
            return motoristas;
        } catch (error) {
            console.error('Erro ao carregar motoristas:', error);
            throw error;
        } finally {
            this.stateService.setLoading(false);
        }
    }

    async obterPorId(id: number): Promise<Motorista> {
        this.stateService.setLoading(true);
        try {
            const response = await firstValueFrom(this.apiService.obterPorId(id));
            const motorista = this.extrairDados(response);
            this.stateService.setMotoristaSelecionado(motorista);
            return motorista;
        } catch (error) {
            console.error('Erro ao obter motorista:', error);
            throw error;
        } finally {
            this.stateService.setLoading(false);
        }
    }

    async criar(motorista: Partial<Motorista>): Promise<Motorista> {
        this.stateService.setLoading(true);
        try {
            const response = await firstValueFrom(this.apiService.criar(motorista));
            const novoMotorista = this.extrairDados(response);
            this.stateService.adicionarMotorista(novoMotorista);
            return novoMotorista;
        } catch (error) {
            console.error('Erro ao criar motorista:', error);
            throw error;
        } finally {
            this.stateService.setLoading(false);
        }
    }

    async atualizar(id: number, motorista: Partial<Motorista>): Promise<Motorista> {
        this.stateService.setLoading(true);
        try {
            const response = await firstValueFrom(this.apiService.atualizar(id, motorista));
            const motoristaAtualizado = this.extrairDados(response);
            this.stateService.atualizarMotorista(motoristaAtualizado);
            return motoristaAtualizado;
        } catch (error) {
            console.error('Erro ao atualizar motorista:', error);
            throw error;
        } finally {
            this.stateService.setLoading(false);
        }
    }

    async remover(id: number): Promise<void> {
        this.stateService.setLoading(true);
        try {
            await firstValueFrom(this.apiService.remover(id));
            this.stateService.removerMotorista(id);
        } catch (error) {
            console.error('Erro ao remover motorista:', error);
            throw error;
        } finally {
            this.stateService.setLoading(false);
        }
    }

    selecionarMotorista(motorista: Motorista | null): void {
        this.stateService.setMotoristaSelecionado(motorista);
    }

    limparSelecao(): void {
        this.stateService.setMotoristaSelecionado(null);
    }

    private extrairDados(response: any): any {
        if (!response) return null;
        
        // Tentar diferentes formatos de resposta
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
        
        // Se não encontrar estrutura conhecida, retornar a resposta diretamente
        return response;
    }
}







