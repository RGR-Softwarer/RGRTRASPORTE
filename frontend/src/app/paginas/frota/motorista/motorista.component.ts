import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GridComponent } from '../../../componentes/grid/grid.component';
import { Motorista } from '../../../dominio/entidade/motorista';
import { TrasportadorUrlEnum } from '../../../dominio/enum/trasportador-url-enum';

@Component({
    selector: 'app-motorista',
    standalone: true,
    imports: [CommonModule, GridComponent],
    templateUrl: './motorista.component.html',
    styleUrls: ['./motorista.component.scss']
})
export class MotoristaComponent {
    buscarTodosUrl = TrasportadorUrlEnum.MOTORISTAS;
    adicionarUrl = 'frota/motorista/adicionar';
    entidade = Motorista;
}







