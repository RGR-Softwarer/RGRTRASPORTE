import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { GatilhoViagem } from '../../../../dominio/entidade/gatilho-viagem';

@Injectable({ providedIn: 'root' })
export class GatilhoViagemStateService {
    private gatilhosSubject = new BehaviorSubject<GatilhoViagem[]>([]);
    private loadingSubject = new BehaviorSubject<boolean>(false);
    private gatilhoSelecionadoSubject = new BehaviorSubject<GatilhoViagem | null>(null);

    gatilhos$ = this.gatilhosSubject.asObservable();
    loading$ = this.loadingSubject.asObservable();
    gatilhoSelecionado$ = this.gatilhoSelecionadoSubject.asObservable();

    setGatilhos(gatilhos: GatilhoViagem[]): void {
        this.gatilhosSubject.next(gatilhos);
    }

    setLoading(loading: boolean): void {
        this.loadingSubject.next(loading);
    }

    setGatilhoSelecionado(gatilho: GatilhoViagem | null): void {
        this.gatilhoSelecionadoSubject.next(gatilho);
    }

    getGatilhos(): GatilhoViagem[] {
        return this.gatilhosSubject.getValue();
    }

    getGatilhoSelecionado(): GatilhoViagem | null {
        return this.gatilhoSelecionadoSubject.getValue();
    }

    adicionarGatilho(gatilho: GatilhoViagem): void {
        const gatilhos = this.getGatilhos();
        this.setGatilhos([...gatilhos, gatilho]);
    }

    atualizarGatilho(gatilho: GatilhoViagem): void {
        const gatilhos = this.getGatilhos();
        const index = gatilhos.findIndex(g => g.id === gatilho.id);
        if (index !== -1) {
            gatilhos[index] = gatilho;
            this.setGatilhos([...gatilhos]);
        }
    }

    removerGatilho(id: number): void {
        const gatilhos = this.getGatilhos();
        this.setGatilhos(gatilhos.filter(g => g.id !== id));
    }

    limpar(): void {
        this.gatilhosSubject.next([]);
        this.gatilhoSelecionadoSubject.next(null);
        this.loadingSubject.next(false);
    }
}







