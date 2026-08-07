import { Injectable } from '@angular/core';
import { ApiService } from '../http/api.service';
import { LoggingService } from '../utils/log/logging.service';
import { FormCamposMetadata, DecoratorUtils } from '../decorator/formulario-decorator';
import { FiltroMetadata } from '../decorator/formulario-decorator';
import { ConfiguracaoGrid } from '../../dominio/interface/grid/configuracao-grid';
import { FiltroGrid, ParametrosBusca } from '../../dominio/interface/grid/filtros-grid';
import { ResponseGrid } from '../../dominio/interface/grid/response-grid';
import { ApiResponse } from '../../dominio/interface/grid/api-response';
import { HttpParams } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import * as XLSX from 'xlsx';

export interface GridData {
  [key: string]: any;
}

export interface GridItem {
  id?: number | string;
  [key: string]: any;
}

@Injectable({
  providedIn: 'root'
})
export class GridService {

  constructor(
    private apiService: ApiService,
    private loggingService: LoggingService
  ) {}

  /**
   * Configura colunas da grid baseado na entidade
   */
  configurarColunas(entidade: any): FormCamposMetadata[] {
    if (!entidade) {
      this.loggingService.warn('Nenhuma entidade fornecida para configurar colunas');
      return [];
    }

    const formFields = DecoratorUtils.getFormFields(entidade);
    
    // Filtrar apenas campos que devem aparecer na grid
    const gridColumns = formFields.filter(field => 
      field.visible !== false && 
      field.type !== 'entidade' && // Campos de entidade são complexos para grid
      !field.readonly // Campos readonly geralmente não são editáveis na grid
    );

    this.loggingService.log('Colunas configuradas para grid:', gridColumns.map(c => c.key));
    return gridColumns;
  }

  /**
   * Configura filtros da grid baseado na entidade
   */
  configurarFiltros(entidade: any): FiltroMetadata[] {
    if (!entidade) {
      this.loggingService.warn('Nenhuma entidade fornecida para configurar filtros');
      return [];
    }

    const filtros = DecoratorUtils.getFilterFields(entidade);
    this.loggingService.log('Filtros configurados para grid:', filtros.map((f: FiltroMetadata) => f.key));
    return filtros;
  }

  /**
   * Busca dados da API
   */
  async buscarDados(url: string, params?: any): Promise<GridItem[]> {
    try {
      this.loggingService.log('Buscando dados da API:', url, params);
      
      const response = await firstValueFrom(this.apiService.get<any>(url, params));
      
      if (response && Array.isArray(response)) {
        this.loggingService.log('Dados recebidos da API:', response.length);
        return response;
      } else if (response && typeof response === 'object') {
        if ('dados' in response && Array.isArray((response as any).dados)) {
            this.loggingService.log('Dados recebidos da API (propriedade "dados"):', (response as any).dados.length);
            return (response as any).dados;
        }
        if ('data' in response && Array.isArray((response as any).data)) {
            this.loggingService.log('Dados recebidos da API (propriedade "data"):', (response as any).data.length);
            return (response as any).data;
        }
      }
      
      this.loggingService.log('Resposta inesperada da API:', response);
      return [];
    } catch (error) {
      this.loggingService.error('Erro ao buscar dados da API:', error);
      return [];
    }
  }

  /**
   * Aplica filtros localmente nos dados
   */
  aplicarFiltroLocal(dados: GridItem[], filtros: FiltroGrid[]): GridItem[] {
    if (!filtros || filtros.length === 0) {
      return dados;
    }

    return dados.filter(item => {
      return filtros.every(filtro => {
        const valor = item[filtro.campo];
        
        if (!valor && filtro.valor !== '') {
          return false;
        }
        
        if (typeof valor === 'string') {
          return valor.toLowerCase().includes(filtro.valor.toLowerCase());
        }
        
        if (typeof valor === 'number') {
          return valor.toString().includes(filtro.valor);
        }
        
        if (typeof valor === 'boolean') {
          const valorBooleano = filtro.valor.toLowerCase();
          return (valor && valorBooleano === 'true') || (!valor && valorBooleano === 'false');
        }
        
        return true;
      });
    });
  }

  /**
   * Formata valor da célula para exibição
   */
  formatCellValue(value: any, columnKey: string, formFields: FormCamposMetadata[]): string {
    if (value === null || value === undefined) {
      return '';
    }

    const field = formFields.find(f => f.key === columnKey);
    if (!field) {
      return String(value);
    }

    switch (field.type) {
      case 'bool':
        return value ? 'Sim' : 'Não';
      
      case 'enum':
        if (field.options) {
          const option = field.options.find(opt => opt.value === value);
          return option ? option.label : String(value);
        }
        return String(value);
      
      case 'data':
        if (value) {
          try {
            const date = new Date(value);
            return date.toLocaleDateString('pt-BR');
          } catch {
            return String(value);
          }
        }
        return '';
      
      case 'numero':
        if (typeof value === 'number') {
          return value.toLocaleString('pt-BR');
        }
        return String(value);
      
      default:
        return String(value);
    }
  }

  /**
   * Verifica se um campo é do tipo booleano
   */
  isBooleanField(columnKey: string, formFields: FormCamposMetadata[]): boolean {
    const field = formFields.find(f => f.key === columnKey);
    return field?.type === 'bool';
  }

  /**
   * Verifica se um campo é do tipo enum
   */
  isEnumField(columnKey: string, formFields: FormCamposMetadata[]): boolean {
    const field = formFields.find(f => f.key === columnKey);
    return field?.type === 'enum';
  }

  /**
   * Obtém opções de um campo enum
   */
  getEnumOptions(columnKey: string, formFields: FormCamposMetadata[]): { value: string; label: string }[] {
    const field = formFields.find(f => f.key === columnKey);
    return field?.options || [];
  }

  /**
   * Exporta dados para Excel
   */
  exportToExcel(dados: GridItem[], colunas: FormCamposMetadata[], nomeArquivo: string = 'dados'): void {
    try {
      // Preparar dados para exportação
      const dadosExportacao = dados.map(item => {
        const linha: any = {};
        colunas.forEach(coluna => {
          const valor = item[coluna.key];
          linha[coluna.label] = this.formatCellValue(valor, coluna.key, colunas);
        });
        return linha;
      });

      // Criar workbook
      const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(dadosExportacao);
      const wb: XLSX.WorkBook = XLSX.utils.book_new();
      XLSX.utils.book_append_sheet(wb, ws, 'Dados');

      // Exportar arquivo
      const nomeCompleto = `${nomeArquivo}_${new Date().toISOString().split('T')[0]}.xlsx`;
      XLSX.writeFile(wb, nomeCompleto);
      
      this.loggingService.log('Arquivo Excel exportado com sucesso:', nomeCompleto);
    } catch (error) {
      this.loggingService.error('Erro ao exportar para Excel:', error);
      throw error;
    }
  }

  /**
   * Carrega configuração da grid do localStorage
   */
  loadGridConfig(gridId: string): Record<string, boolean> | null {
    try {
      const config = localStorage.getItem(`grid_config_${gridId}`);
      return config ? JSON.parse(config) : null;
    } catch (error) {
      this.loggingService.error('Erro ao carregar configuração da grid:', error);
      return null;
    }
  }

  /**
   * Salva configuração da grid no localStorage
   */
  saveGridConfig(gridId: string, config: Record<string, boolean>): void {
    try {
      localStorage.setItem(`grid_config_${gridId}`, JSON.stringify(config));
      this.loggingService.log('Configuração da grid salva:', config);
    } catch (error) {
      this.loggingService.error('Erro ao salvar configuração da grid:', error);
    }
  }

  /**
   * Verifica se dois conjuntos de parâmetros são iguais
   */
  parametrosIguais(params1: ParametrosBusca, params2: ParametrosBusca): boolean {
    if (!params1 && !params2) return true;
    if (!params1 || !params2) return false;
    
    const keys1 = Object.keys(params1);
    const keys2 = Object.keys(params2);
    
    if (keys1.length !== keys2.length) return false;
    
    return keys1.every(key => (params1 as any)[key] === (params2 as any)[key]);
  }
} 