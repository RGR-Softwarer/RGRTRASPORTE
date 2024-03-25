import { Component } from '@angular/core';
import { TrasportadorUrls } from '../../../dominio/enum/trasportador-url-enum';

@Component({
  selector: 'app-cadastro-veiculo',
  templateUrl: './veiculo.component.html',
  styleUrl: './veiculo.component.scss'
})

export class CadastroVeiculoComponent {
  adicionarUrl: string = '/cadastro/veiculo/adicionar';
  buscarTodosUrl: string = TrasportadorUrls.ObterTodos + 'veiculo';  
}