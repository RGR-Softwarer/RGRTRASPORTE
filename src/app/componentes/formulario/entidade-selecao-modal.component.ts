import { Component, Input, OnInit, Inject } from '@angular/core';
import { NzModalRef, NZ_MODAL_DATA } from 'ng-zorro-antd/modal';
import { GridService } from '../../services/grid/grid.service';
import { FormCamposMetadata } from '../../services/decorator/formulario-decorator';

export interface EntidadeSelecaoConfig {
  url: string;
  displayField: string;
  valueField: string;
  searchFields: string[];
  modalTitle: string;
  modalWidth?: number;
}

@Component({
  selector: 'app-entidade-selecao-modal',
  template: `
    <div class="entidade-selecao-container">
      <!-- Campo de busca -->
      <div class="search-container" style="margin-bottom: 16px;">
        <nz-input-group nzSearch [nzAddOnAfter]="suffixIconButton">
          <input 
            type="text" 
            nz-input 
            placeholder="Buscar..."
            [(ngModel)]="searchTerm"
            (input)="onSearchChange($event)"
          />
        </nz-input-group>
        <ng-template #suffixIconButton>
          <button nz-button nzType="primary" nzSearch type="button" (click)="buscarDados()">
            <span nz-icon nzType="search"></span>
          </button>
        </ng-template>
      </div>

      <!-- Tabela de dados -->
      <nz-table
        #entidadeTable
        [nzData]="dadosFiltrados"
        [nzLoading]="carregando"
        nzSize="small"
        [nzFrontPagination]="false"
        [nzShowPagination]="false"
        [nzScroll]="{ y: '400px' }"
      >
        <thead>
          <tr>
            <th>{{ config.valueField | uppercase }}</th>
            <th>{{ config.displayField | uppercase }}</th>
            <th *ngFor="let field of filteredSearchFields">{{ field | uppercase }}</th>
          </tr>
        </thead>
        <tbody>
          <tr 
            *ngFor="let item of dadosFiltrados" 
            (click)="onSelect(item)" 
            [class.selected-row]="selectedItem?.[config.valueField] === item[config.valueField]"
            style="cursor: pointer;"
          >
            <td>{{ item[config.valueField] }}</td>
            <td>{{ item[config.displayField] }}</td>
            <td *ngFor="let field of filteredSearchFields">{{ item[field] }}</td>
          </tr>
        </tbody>
      </nz-table>

      <!-- Botões de ação -->
      <div style="text-align: right; margin-top: 16px;">
        <button nz-button nzType="default" (click)="fechar()">Cancelar</button>
        <button nz-button nzType="primary" [disabled]="!selectedItem" (click)="confirmarSelecao()">
          Selecionar
        </button>
      </div>
    </div>
  `,
  styles: [`
    .entidade-selecao-container {
      padding: 16px;
    }
    
    .search-container {
      display: flex;
      gap: 8px;
    }
    
    .selected-row {
      background: #e6f7ff;
    }
    
    .selected-row:hover {
      background: #bae7ff !important;
    }

    ::ng-deep .ant-table-placeholder {
        text-align: center;
    }
  `]
})
export class EntidadeSelecaoModalComponent implements OnInit {
  config!: EntidadeSelecaoConfig;
  
  dados: any[] = [];
  dadosFiltrados: any[] = [];
  carregando = false;
  selectedItem: any = null;
  searchTerm = '';

  get filteredSearchFields(): string[] {
    if (!this.config || !this.config.searchFields) {
        return [];
    }
    // Remove o displayField da lista de busca para não repetir a coluna
    return this.config.searchFields.filter(field => field !== this.config.displayField);
  }

  constructor(
    private gridService: GridService,
    private modalRef: NzModalRef,
    @Inject(NZ_MODAL_DATA) private modalData: any
  ) {
    this.config = modalData.config;
  }

  ngOnInit(): void {
    this.buscarDados();
  }

  buscarDados(): void {
    this.carregando = true;
    this.gridService.buscarDados(this.config.url).then(dados => {
      this.dados = dados;
      this.dadosFiltrados = dados;
      this.carregando = false;
    }).catch(() => {
      this.carregando = false;
    });
  }

  onSearchChange(event: any): void {
    const term = event.target.value;
    this.searchTerm = term;
    if (!term.trim()) {
      this.dadosFiltrados = this.dados;
      return;
    }

    this.dadosFiltrados = this.dados.filter(item => {
      const searchFields = [this.config.displayField, ...this.config.searchFields];
      return searchFields.some(field => {
        const value = item[field];
        return value && value.toString().toLowerCase().includes(term.toLowerCase());
      });
    });
  }

  onSelect(item: any): void {
    this.selectedItem = item;
  }

  confirmarSelecao(): void {
    if (this.selectedItem) {
      this.modalRef.close(this.selectedItem);
    }
  }

  fechar(): void {
    this.modalRef.close();
  }
} 