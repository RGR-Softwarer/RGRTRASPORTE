import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Motorista } from '../../../../dominio/entidade/motorista';

@Injectable({ providedIn: 'root' })
export class MotoristaStateService {
    private motoristasSubject = new BehaviorSubject<Motorista[]>([]);
    private loadingSubject = new BehaviorSubject<boolean>(false);
    private motoristaSelecionadoSubject = new BehaviorSubject<Motorista | null>(null);

    motoristas$ = this.motoristasSubject.asObservable();
    loading$ = this.loadingSubject.asObservable();
    motoristaSelecionado$ = this.motoristaSelecionadoSubject.asObservable();

    setMotoristas(motoristas: Motorista[]): void {
        this.motoristasSubject.next(motoristas);
    }

    setLoading(loading: boolean): void {
        this.loadingSubject.next(loading);
    }

    setMotoristaSelecionado(motorista: Motorista | null): void {
        this.motoristaSelecionadoSubject.next(motorista);
    }

    getMotoristas(): Motorista[] {
        return this.motoristasSubject.getValue();
    }

    getMotoristaSelecionado(): Motorista | null {
        return this.motoristaSelecionadoSubject.getValue();
    }

    adicionarMotorista(motorista: Motorista): void {
        const motoristas = this.getMotoristas();
        this.setMotoristas([...motoristas, motorista]);
    }

    atualizarMotorista(motorista: Motorista): void {
        const motoristas = this.getMotoristas();
        const index = motoristas.findIndex(m => m.id === motorista.id);
        if (index !== -1) {
            motoristas[index] = motorista;
            this.setMotoristas([...motoristas]);
        }
    }

    removerMotorista(id: number): void {
        const motoristas = this.getMotoristas();
        this.setMotoristas(motoristas.filter(m => m.id !== id));
    }

    limpar(): void {
        this.motoristasSubject.next([]);
        this.motoristaSelecionadoSubject.next(null);
        this.loadingSubject.next(false);
    }
}







