import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subject, takeUntil, forkJoin, of, catchError } from 'rxjs';
import { AppContext } from '../../dominio/entidade/app.context';
import { AppContextService } from '../../services/context/app.context';
import { ApiService } from '../../services/http/api.service';
import { TrasportadorUrlEnum } from '../../dominio/enum/trasportador-url-enum';
import { NotificationService } from '../../shared/services/notification.service';

interface DashboardStats {
    totalViagens: number;
    viagensHoje: number;
    viagensEmAndamento: number;
    viagensAgendadas: number;
    totalPassageiros: number;
    passageirosAtivos: number;
    totalVeiculos: number;
    veiculosDisponiveis: number;
    veiculosEmViagem: number;
    totalMotoristas: number;
    motoristasAtivos: number;
    totalLocalidades: number;
}

interface ViagemRecente {
    id: number;
    codigo?: string;
    dataViagem: Date;
    localidadeOrigemNome: string;
    localidadeDestinoNome: string;
    situacao: string;
    vagasDisponiveis: number;
    quantidadeVagas: number;
}

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss'],
    standalone: true,
    imports: [CommonModule]
})
export class DashboardComponent implements OnInit, OnDestroy {
    private destroy$ = new Subject<void>();
    
    userContext: AppContext | null = null;
    loading = true;
    
    stats: DashboardStats = {
        totalViagens: 0,
        viagensHoje: 0,
        viagensEmAndamento: 0,
        viagensAgendadas: 0,
        totalPassageiros: 0,
        passageirosAtivos: 0,
        totalVeiculos: 0,
        veiculosDisponiveis: 0,
        veiculosEmViagem: 0,
        totalMotoristas: 0,
        motoristasAtivos: 0,
        totalLocalidades: 0
    };
    
    viagensRecentes: ViagemRecente[] = [];
    ultimaAtualizacao: Date = new Date();

    constructor(
        private appContextService: AppContextService,
        private apiService: ApiService,
        private router: Router,
        private notificationService: NotificationService
    ) {}

    ngOnInit(): void {
        this.userContext = this.appContextService.obterUsuarioLogado();
        this.carregarEstatisticas();
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    carregarEstatisticas(): void {
        this.loading = true;
        
        forkJoin({
            estatisticas: this.apiService.get<any>(TrasportadorUrlEnum.DASHBOARD_ESTATISTICAS).pipe(catchError(() => of({ data: null }))),
            viagensHoje: this.apiService.get<any>(TrasportadorUrlEnum.DASHBOARD_VIAGENS_HOJE).pipe(catchError(() => of({ data: [] }))),
            veiculosEmViagem: this.apiService.get<any>(TrasportadorUrlEnum.DASHBOARD_VEICULOS_EM_VIAGEM).pipe(catchError(() => of({ data: [] })))
        }).pipe(takeUntil(this.destroy$)).subscribe({
            next: (result) => {
                this.processarEstatisticas(result);
                this.loading = false;
                this.ultimaAtualizacao = new Date();
            },
            error: (error) => {
                console.error('Erro ao carregar estatísticas:', error);
                this.loading = false;
                this.notificationService.error('Erro', 'Falha ao carregar estatísticas do dashboard');
            }
        });
    }

    private processarEstatisticas(result: any): void {
        // Processar estatísticas do endpoint de dashboard
        const estatisticas = this.extrairObjeto(result.estatisticas);
        const viagensHoje = this.extrairDados(result.viagensHoje);
        
        if (estatisticas) {
            this.stats.totalViagens = estatisticas.totalViagens || 0;
            this.stats.viagensHoje = estatisticas.viagensHoje || 0;
            this.stats.viagensEmAndamento = estatisticas.viagensEmAndamento || 0;
            this.stats.viagensAgendadas = estatisticas.viagensAgendadas || 0;
            this.stats.totalPassageiros = estatisticas.totalPassageiros || 0;
            this.stats.passageirosAtivos = estatisticas.passageirosAtivos || 0;
            this.stats.totalVeiculos = estatisticas.totalVeiculos || 0;
            this.stats.veiculosDisponiveis = estatisticas.veiculosDisponiveis || 0;
            this.stats.veiculosEmViagem = estatisticas.veiculosEmViagem || 0;
            this.stats.totalMotoristas = estatisticas.totalMotoristas || 0;
            this.stats.motoristasAtivos = estatisticas.motoristasAtivos || 0;
            this.stats.totalLocalidades = estatisticas.totalLocalidades || 0;
        }
        
        // Viagens Recentes (últimas 5) - usar viagens de hoje
        this.viagensRecentes = viagensHoje
            .slice(0, 5)
            .map((v: any) => ({
                id: v.id,
                codigo: v.codigo,
                dataViagem: new Date(v.dataViagem || v.data),
                localidadeOrigemNome: v.localidadeOrigemNome || v.origem || 'Não informado',
                localidadeDestinoNome: v.localidadeDestinoNome || v.destino || 'Não informado',
                situacao: v.situacao || v.status || 'Agendada',
                vagasDisponiveis: v.vagasDisponiveis || v.vagasLivres || 0,
                quantidadeVagas: v.quantidadeVagas || v.totalVagas || 0
            }));
    }

    private extrairDados(response: any): any[] {
        if (!response) return [];
        if (response.success && response.data) return Array.isArray(response.data) ? response.data : [];
        if (response.sucesso && response.dados) return Array.isArray(response.dados) ? response.dados : [];
        if (response.data) return Array.isArray(response.data) ? response.data : [];
        if (response.dados) return Array.isArray(response.dados) ? response.dados : [];
        if (Array.isArray(response)) return response;
        return [];
    }

    private extrairObjeto(response: any): any {
        if (!response) return null;
        if (response.success && response.data) return response.data;
        if (response.sucesso && response.dados) return response.dados;
        if (response.data) return response.data;
        if (response.dados) return response.dados;
        if (typeof response === 'object' && !Array.isArray(response)) return response;
        return null;
    }

    atualizarDados(): void {
        this.carregarEstatisticas();
        this.notificationService.success('Sucesso', 'Dados atualizados!');
    }

    navegarPara(rota: string): void {
        this.router.navigate([rota]);
    }

    getSituacaoClass(situacao: string): string {
        switch (situacao?.toLowerCase()) {
            case 'agendada': return 'status-agendada';
            case 'emandamento': case 'em andamento': return 'status-andamento';
            case 'finalizada': return 'status-finalizada';
            case 'cancelada': return 'status-cancelada';
            default: return 'status-agendada';
        }
    }

    formatarData(data: Date): string {
        return data.toLocaleDateString('pt-BR', { 
            day: '2-digit', 
            month: '2-digit', 
            year: 'numeric' 
        });
    }

    getOcupacao(viagem: ViagemRecente): number {
        if (!viagem.quantidadeVagas) return 0;
        const ocupadas = viagem.quantidadeVagas - viagem.vagasDisponiveis;
        return Math.round((ocupadas / viagem.quantidadeVagas) * 100);
    }
}
