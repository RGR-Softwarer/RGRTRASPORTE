import { Component } from '@angular/core';
import { TrasportadorUrls } from '../../../dominio/enum/trasportador-url-enum';

@Component({
  selector: 'app-pacote',
  //standalone: false,
  // imports: [],
  templateUrl: './pacote.component.html',
  styleUrl: './pacote.component.scss'
})
export class PacoteComponent {
  adicionarUrl: string = '/cadastro/pacote';
  buscarTodosUrl: string = TrasportadorUrls.ObterTodos + 'veiculo';  
}
