import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GridComponent } from '../../../componentes/grid/grid.component';
import { GatilhoViagem } from '../../../dominio/entidade/gatilho-viagem';
import { TrasportadorUrlEnum } from '../../../dominio/enum/trasportador-url-enum';

@Component({
    selector: 'app-gatilho-viagem',
    standalone: true,
    imports: [CommonModule, GridComponent],
    templateUrl: './gatilho-viagem.component.html',
    styleUrls: ['./gatilho-viagem.component.scss']
})
export class GatilhoViagemComponent {
    buscarTodosUrl = TrasportadorUrlEnum.GATILHOS_VIAGEM;
    adicionarUrl = 'frota/gatilho-viagem/adicionar';
    entidade = GatilhoViagem;
}







