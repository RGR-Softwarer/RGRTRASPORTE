import { Component, OnInit } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { FormularioComponent } from '../../../../componentes/formulario/formulario.component';
import { GatilhoViagemFacade } from '../services/gatilho-viagem.facade';
import { GatilhoViagem } from '../../../../dominio/entidade/gatilho-viagem';
import { NotificationService } from '../../../../shared/services/notification.service';
import { FormularioData } from '../../../../services/formulario/formulario.service';
import { firstValueFrom } from 'rxjs';

@Component({
    selector: 'app-gatilho-viagem-formulario',
    standalone: true,
    imports: [CommonModule, FormularioComponent],
    templateUrl: './gatilho-viagem-formulario.component.html',
    styleUrls: ['./gatilho-viagem-formulario.component.scss']
})
export class GatilhoViagemFormularioComponent implements OnInit {
    gatilhoViagem = new GatilhoViagem();

    constructor(
        private gatilhoViagemFacade: GatilhoViagemFacade,
        private notificationService: NotificationService,
        private location: Location
    ) {}

    ngOnInit(): void {}

    salvar = async (data: FormularioData) => {
        try {
            // Verificar se está em modo de edição pelo ID
            const isEditMode = !!(data.id || (data as any).id);
            const gatilhoId = data.id || (data as any).id;
            
            // Montar array de dias da semana
            const diasSemana: string[] = [];
            if (data.segunda) diasSemana.push('Segunda');
            if (data.terca) diasSemana.push('Terca');
            if (data.quarta) diasSemana.push('Quarta');
            if (data.quinta) diasSemana.push('Quinta');
            if (data.sexta) diasSemana.push('Sexta');
            if (data.sabado) diasSemana.push('Sabado');
            if (data.domingo) diasSemana.push('Domingo');

            const gatilhoData: any = {
                nome: data.nome,
                localidadeOrigemId: data.localidadeOrigemId,
                localidadeDestinoId: data.localidadeDestinoId,
                horarioSaida: data.horarioSaida,
                horarioChegada: data.horarioChegada,
                diasSemana: diasSemana,
                quantidadeVagas: data.quantidadeVagas,
                descricao: data.descricao || '',
                ativo: data.ativo !== false
            };

            if (isEditMode && gatilhoId) {
                await firstValueFrom(this.gatilhoViagemFacade.atualizar(gatilhoId, gatilhoData));
                this.notificationService.success('Sucesso', 'Gatilho de viagem atualizado com sucesso');
            } else {
                await firstValueFrom(this.gatilhoViagemFacade.criar(gatilhoData));
                this.notificationService.success('Sucesso', 'Gatilho de viagem criado com sucesso');
            }
            this.location.back();
        } catch (error: any) {
            console.error('Erro ao salvar gatilho de viagem:', error);
            const mensagem = error?.error?.message || error?.message || 'Falha ao salvar gatilho de viagem';
            this.notificationService.error('Erro', mensagem);
        }
    }
}
