import { Component, OnInit } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { FormularioComponent } from '../../../../componentes/formulario/formulario.component';
import { MotoristaFacade } from '../services/motorista.facade';
import { Motorista } from '../../../../dominio/entidade/motorista';
import { NotificationService } from '../../../../shared/services/notification.service';
import { FormularioData } from '../../../../services/formulario/formulario.service';
import { firstValueFrom } from 'rxjs';

@Component({
    selector: 'app-motorista-formulario',
    standalone: true,
    imports: [CommonModule, FormularioComponent],
    templateUrl: './motorista-formulario.component.html',
    styleUrls: ['./motorista-formulario.component.scss']
})
export class MotoristaFormularioComponent implements OnInit {
    motorista = new Motorista();

    constructor(
        private motoristaFacade: MotoristaFacade,
        private notificationService: NotificationService,
        private location: Location
    ) {}

    ngOnInit(): void {}

    salvar = async (data: FormularioData) => {
        try {
            // Verificar se está em modo de edição pelo ID
            const isEditMode = !!(data.id || (data as any).id);
            const motoristaId = data.id || (data as any).id;
            
            // Criar instância de Motorista com os dados
            const motorista = Object.assign(new Motorista(), data);
            
            if (isEditMode && motoristaId) {
                motorista.id = motoristaId;
                await firstValueFrom(this.motoristaFacade.atualizar(motoristaId, motorista));
                this.notificationService.success('Sucesso', 'Motorista atualizado com sucesso');
            } else {
                await firstValueFrom(this.motoristaFacade.criar(motorista));
                this.notificationService.success('Sucesso', 'Motorista criado com sucesso');
            }
            this.location.back();
        } catch (error: any) {
            console.error('Erro ao salvar motorista:', error);
            const mensagem = error?.error?.message || error?.message || 'Falha ao salvar motorista';
            this.notificationService.error('Erro', mensagem);
        }
    }
}
