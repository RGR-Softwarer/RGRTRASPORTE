import { Component, OnInit } from '@angular/core';
import { AppContext } from '../../dominio/entidade/app.context';
import { AppContextService } from '../../services/context/app.context';
import { ConfiguracaoGrid, RolagemTabela } from '../../dominio/interface/configuracao-grid';
import { FormControl, FormGroup, NonNullableFormBuilder } from '@angular/forms';
import { NzTableLayout, NzTablePaginationPosition, NzTablePaginationType, NzTableSize } from 'ng-zorro-antd/table';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  userContext: AppContext | null = null;
  formularioConfiguracao!: FormGroup<{ [K in keyof ConfiguracaoGrid]: FormControl<ConfiguracaoGrid[K]> }>;
  minhaListaDeObjetos:any[] = [ { Nome: 'João', Idade: 20 }, { Nome: 'Maria', Idade: 30 }, { Nome: 'José', Idade: 40 }, { Nome: 'Pedro', Idade: 50 }, { Nome: 'Paulo', Idade: 60 }, { Nome: 'Paula', Idade: 70 }];
  
  constructor(private appContextService: AppContextService, private formBuilder: NonNullableFormBuilder) { 
    this.formularioConfiguracao = this.formBuilder.group({
      comBorda: [false],
      carregando: [false],
      paginacao: [true],
      alteradorTamanho: [false],
      titulo: [true],
      cabecalho: [true],
      rodape: [true],
      expansivel: [true],
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
  }
}