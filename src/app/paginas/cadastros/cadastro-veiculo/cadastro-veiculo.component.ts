import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-cadastro-veiculo',
  templateUrl: './cadastro-veiculo.component.html',
  styleUrl: './cadastro-veiculo.component.scss'
})

export class CadastroVeiculoComponent implements OnInit {
  dados: readonly any[] = [];



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