import { Component } from '@angular/core';
import { TrasportadorUrls } from '../../../dominio/enum/trasportador-url-enum';

@Component({
  selector: 'app-viagem',
  // standalone: true,
  // imports: [],
  templateUrl: './viagem.component.html',
  styleUrl: './viagem.component.scss'
})
export class ViagemComponent {
  adicionarUrl: string = '/cadastro/viagem/adicionar';
  buscarTodosUrl: string = TrasportadorUrls.ObterTodos + 'viagem';

}
