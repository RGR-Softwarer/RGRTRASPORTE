import { Component, OnInit } from '@angular/core';
import { AppContext } from '../../dominio/entidade/app.context';
import { AppContextService } from '../../services/context/app.context';
import { NzTableLayout, NzTablePaginationPosition, NzTablePaginationType, NzTableSize } from 'ng-zorro-antd/table';
import { ConfiguracaoGrid, RolagemTabela } from '../../dominio/interface/configuracao-grid';
import { FormControl, FormGroup, NonNullableFormBuilder } from '@angular/forms';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  userContext: AppContext | null = null;
  formularioConfiguracao!: FormGroup<{ [K in keyof ConfiguracaoGrid]: FormControl<ConfiguracaoGrid[K]> }>;
  minhaListaDeObjetos: readonly any[] = [];

  constructor(private appContextService: AppContextService, private formBuilder: NonNullableFormBuilder) { 
    this.formularioConfiguracao = this.formBuilder.group({
      comBorda: [false],
      carregando: [false],
      paginacao: [true],
      alteradorTamanho: [false],
      titulo: [true],
      cabecalho: [true],
      rodape: [true],
      expansivel: [false],
      caixaSelecao: [false],
      cabecalhoFixo: [false],
      semResultado: [false],
      elipse: [false],
      simples: [false],
      mostrarOpcoes: [false],
      tamanho: 'small' as NzTableSize,
      tipoPaginacao: 'default' as NzTablePaginationType,
      rolagemTabela: 'unset' as RolagemTabela,
      layoutTabela: 'auto' as NzTableLayout,
      posicao: 'bottom' as NzTablePaginationPosition,
      tituloTabela: 'Título da Tabela',
      rodapeTabela: 'Rodapé da Tabela'
    });
  }   

  ngOnInit() {
    this.userContext = this.appContextService.obterUsuarioLogado();
    console.log(this.userContext);
    this.minhaListaDeObjetos = this.gerarDados();
  }
  
  gerarDados(): readonly any[] {
    const dados = [];
    for (let i = 1; i <= 100; i++) {
      dados.push({
        nome: 'John Brown',
        idade: `${i}2`,
        endereco: `Nova York No. ${i} Lake Park`,
        descricao: `Meu nome é John Brown, tenho ${i}2 anos, morando em Nova York No. ${i} Lake Park.`,
        marcado: false,
        expandido: false
      });
    }
    return dados;
  }
}