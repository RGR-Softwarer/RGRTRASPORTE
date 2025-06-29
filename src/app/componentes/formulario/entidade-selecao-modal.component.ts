import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { GridService } from '../../services/grid/grid.service';
import { FormCamposMetadata, DecoratorUtils } from '../../services/decorator/formulario-decorator';
import { ConfiguracaoGrid } from '../../dominio/interface/grid/configuracao-grid';
import { NonNullableFormBuilder, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { GridComponent } from '../grid/grid.component';

export interface EntidadeSelecaoConfig {
  url: string;
  displayField: string;
  valueField: string;
  searchFields: string[];
  modalTitle: string;
  modalWidth?: number;
  entidade?: any; // Entidade para configurar a grid
  filtroInicial?: string; // Filtro inicial para aplicar na grid
}

@Component({
  selector: 'app-entidade-selecao-modal',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    GridComponent
  ],
  template: `
    <div class="modal-backdrop" (click)="fechar()"></div>
    <div class="modal show d-block" tabindex="-1">
      <div class="modal-dialog modal-dialog-centered modal-xl">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">{{ getModalTitle() }}</h5>
            <button type="button" class="btn-close" aria-label="Fechar" (click)="fechar()"></button>
          </div>
          <div class="modal-body">
            <!-- Componente Grid -->
            <app-grid 
              [buscarTodosUrl]="config.url"
              [entidade]="config.entidade"
              [identificador]="'entidade-selecao-grid'"
              [acoes]="gridActions"
              [formularioConfiguracao]="gridConfig"
              [filtroInicial]="config.filtroInicial">
            </app-grid>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" (click)="fechar()">Cancelar</button>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .modal-backdrop {
      position: fixed;
      top: 0; left: 0; right: 0; bottom: 0;
      background: rgba(0,0,0,0.5);
      z-index: 1040;
    }
    .modal {
      z-index: 1050;
      position: fixed;
      top: 0; left: 0; right: 0; bottom: 0;
      display: flex;
      align-items: center;
      justify-content: center;
    }
    .modal-content {
      border-radius: 8px;
      box-shadow: 0 4px 20px rgba(0,0,0,0.15);
      max-height: 90vh;
      width: 95vw;
      max-width: 1200px;
      margin: 24px auto;
      padding: 16px 24px;
      background: #fff;
    }
    .modal-body {
      max-height: calc(90vh - 120px);
      overflow: auto;
      padding: 24px;
      background: #fff;
      border-radius: 8px;
    }
    .modal-body app-grid {
      height: 100%;
    }
  `]
})
export class EntidadeSelecaoModalComponent implements OnInit {
  @Input() config!: EntidadeSelecaoConfig;
  @Input() show = false;
  @Output() close = new EventEmitter<void>();
  @Output() select = new EventEmitter<any>();

  gridActions: any[] = [];
  gridConfig: any;

  constructor(
    private gridService: GridService,
    private formBuilder: NonNullableFormBuilder
  ) {}

  ngOnInit(): void {
    this.configurarGrid();
  }

  configurarGrid(): void {
    // Configurar ações da grid (apenas selecionar)
    this.gridActions = [
      {
        label: 'Selecionar',
        acao: (item: any) => this.selecionarItem(item)
      }
    ];

    // Configurar a grid para seleção de entidade
    this.gridConfig = this.formBuilder.group({
      comBorda: [true],
      carregando: [false],
      paginacao: [true],
      alteradorTamanho: [false],
      titulo: [false],
      cabecalho: [true],
      rodape: [false],
      expansivel: [false],
      caixaSelecao: [false],
      cabecalhoFixo: [false],
      semResultado: [false],
      elipse: [false],
      simples: [false],
      mostrarOpcoes: [false],
      tamanho: ['small'],
      tipoPaginacao: ['default'],
      rolagemTabela: ['scroll'],
      layoutTabela: ['auto'],
      posicao: ['bottom'],
      tituloTabela: [''],
      rodapeTabela: [''],
      adicionar: [false],
      action: [true]
    });
  }

  getModalTitle(): string {
    return this.config && this.config.modalTitle ? this.config.modalTitle : 'Selecionar Entidade';
  }

  selecionarItem(item: any): void {
    this.select.emit(item);
    this.fechar();
  }

  fechar(): void {
    this.close.emit();
  }
} 