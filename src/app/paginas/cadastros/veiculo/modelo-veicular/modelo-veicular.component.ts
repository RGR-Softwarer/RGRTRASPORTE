import { Component } from '@angular/core';
import { TrasportadorUrls } from '../../../../dominio/enum/trasportador-url-enum';
import { Action } from '../../../../dominio/interface/grid/action-grid';
import { ModeloVeicular } from '../../../../dominio/entidade/veiculo/modelo-veicular';

@Component({
  selector: 'app-cadastro-modelo-veicular',
  templateUrl: './modelo-veicular.component.html',
  styleUrl: './modelo-veicular.component.scss'
})

export class CadastroModeloVeicularComponent {
  adicionarUrl: string = '/frota/modeloveicular/';
  buscarTodosUrl: string = TrasportadorUrls.ObterTodos + 'veiculo/ModeloVeicular';  
  acoes: Action[] = [{ label: 'Editar', acao: () => {} }];
  modeloVeicular = new ModeloVeicular();
}