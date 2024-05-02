import { Component } from '@angular/core';
import { TrasportadorUrls } from '../../../dominio/enum/trasportador-url-enum';

@Component({
  selector: 'app-pacote',
  templateUrl: './pacote.component.html',
  styleUrl: './pacote.component.scss'
})
export class PacoteComponent {
  adicionarUrl: string = '/cadastro/pacote/adicionar';
  buscarTodosUrl: string = TrasportadorUrls.ObterTodos + 'veiculo';  
}
