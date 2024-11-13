import { Component } from '@angular/core';
import { TrasportadorUrls } from '../../../dominio/enum/trasportador-url-enum';
import { Action } from '../../../dominio/interface/grid/action-grid';

@Component({
  selector: 'app-cadastro-veiculo',
  templateUrl: './veiculo.component.html',
  styleUrl: './veiculo.component.scss'
})

export class CadastroVeiculoComponent {
  adicionarUrl: string = '/frota/veiculo/';
  buscarTodosUrl: string = TrasportadorUrls.ObterTodos + 'veiculo';  
  acoes: Action[] = [{ label: 'Editar', acao: () => {} }];
}