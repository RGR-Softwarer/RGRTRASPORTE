import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-cadastro-veiculo',
  templateUrl: './veiculo.component.html',
  styleUrl: './veiculo.component.scss'
})

export class CadastroVeiculoComponent implements OnInit {
  dados: readonly any[] = [];
  adicionarUrl : string = '/cadastro/veiculo/adicionar';

  ngOnInit() {
    this.dados = this.gerarDados();
  }

  gerarDados(): readonly any[] {
    const dados = [];
    for (let i = 1; i <= 100; i++) {
      dados.push({
        Marca: 'Toyota ' + i,
        Modelo: 'Corolla',
        Ano: 2020,
        Placa: `ABC-1234`,
        Descricao: `Marca: Toyota, Modelo: Corolla, Ano: 2020, Placa: ABC-1234`,
      });
      
    }
    return dados;
  }

}