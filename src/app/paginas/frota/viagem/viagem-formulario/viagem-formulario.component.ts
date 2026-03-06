import { Component, OnInit } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { FormularioComponent } from '../../../../componentes/formulario/formulario.component';
import { ViagemFacade } from '../services/viagem.facade';
import { Viagem } from '../../../../dominio/entidade/viagem';
import { NotificationService } from '../../../../shared/services/notification.service';
import { FormularioData } from '../../../../services/formulario/formulario.service';
import { firstValueFrom } from 'rxjs';

@Component({
    selector: 'app-viagem-formulario',
    standalone: true,
    imports: [CommonModule, FormularioComponent],
    templateUrl: './viagem-formulario.component.html',
    styleUrls: ['./viagem-formulario.component.scss']
})
export class ViagemFormularioComponent implements OnInit {
    viagem = new Viagem();

    constructor(
        private viagemFacade: ViagemFacade,
        private notificationService: NotificationService,
        private location: Location
    ) {}

    ngOnInit(): void {
        // Configurar entidades para seleção
        this.configurarEntidadesSelecao();
    }

    private configurarEntidadesSelecao(): void {
        // As configurações de entidade são definidas via decorators na classe Viagem
        // Mas podemos adicionar configurações dinâmicas aqui se necessário
    }

    salvar = async (data: FormularioData) => {
        try {
            // O FormularioData já vem com isEditMode e id se estiver em modo de edição
            const isEditMode = (data as any).isEditMode || false;
            const viagemId = data.id || (data as any).id;
            
            // Preparar dados para envio
            const viagemData = this.prepararDadosViagem(data);
            
            // Criar instância de Viagem com os dados
            const viagem = Object.assign(new Viagem(), viagemData);
            
            // Se está em modo de edição, preservar o ID
            if (isEditMode && viagemId) {
                viagem.id = viagemId;
            }

            // Usar o método salvarViagem do facade que retorna Observable
            await firstValueFrom(this.viagemFacade.salvarViagem(viagem));
            
            // A notificação já é feita pelo facade, mas podemos adicionar uma aqui também
            this.location.back();
        } catch (error: any) {
            console.error('Erro ao salvar viagem:', error);
            const mensagem = error?.error?.message || error?.message || 'Falha ao salvar viagem';
            this.notificationService.error('Erro', mensagem);
        }
    }

    private prepararDadosViagem(data: FormularioData): Partial<Viagem> {
        // Converter dataViagem para Date se for string
        let dataViagem: Date | undefined;
        if (data.dataViagem) {
            if (typeof data.dataViagem === 'string') {
                dataViagem = new Date(data.dataViagem);
            } else if (data.dataViagem instanceof Date) {
                dataViagem = data.dataViagem;
            }
        }

        return {
            dataViagem,
            horarioSaida: data.horarioSaida as string,
            horarioChegada: data.horarioChegada as string,
            veiculoId: data.veiculoId as number,
            motoristaId: data.motoristaId as number,
            localidadeOrigemId: data.localidadeOrigemId as number,
            localidadeDestinoId: data.localidadeDestinoId as number,
            quantidadeVagas: data.quantidadeVagas as number,
            distancia: (data.distancia as number) || 0,
            descricaoViagem: (data.descricaoViagem as string) || '',
            polilinhaRota: (data.polilinhaRota as string) || '',
            ativo: data.ativo !== false,
            gatilhoViagemId: data.gatilhoViagemId as number | undefined
        };
    }
}
