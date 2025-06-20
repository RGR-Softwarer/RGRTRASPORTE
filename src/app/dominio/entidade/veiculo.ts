import { FormCampo, FiltroGrid, FormCampoEntidade } from '../../services/decorator/formulario-decorator';
import { CategoriaCNHEnum } from '../enum/veiculo/categoria-cnh-enum';
import { StatusVeiculoEnum } from '../enum/veiculo/status-veiculo-enum';
import { TipoCombustivelEnum } from '../enum/veiculo/tipo-combustivel-enum';
import { TipoVeiculoEnum } from '../enum/veiculo/tipo-veiculo-enum';
import { modeloVeicularBuscaConfig } from '../config/busca-entidade/modelo-veicular-busca.config';

export class Veiculo {
    @FormCampo('ID', 'numero', false, false, undefined, 4, true)
    public id?: number;

    @FormCampo('Placa', 'texto', true, true, undefined, 4, false, { placeholder: 'AAA-0A00', maxLength: 8, minLength: 7 })
    @FiltroGrid('Placa', 'texto', true)
    public placa?: string;

    @FormCampo('Placa Formatada', 'texto', false, false, undefined, 4, true)
    public placaFormatada?: string;

    @FormCampo('Modelo', 'texto', true, true, undefined, 8, false, { placeholder: 'Ex: Onix, Gol, etc.' })
    @FiltroGrid('Modelo', 'texto', true)
    public modelo?: string;

    @FormCampo('Marca', 'texto', true, true, undefined, 8, false, { placeholder: 'Ex: Chevrolet, Volkswagen, etc.' })
    @FiltroGrid('Marca', 'texto', true)
    public marca?: string;

    @FormCampo('Número do Chassi', 'texto', true, true, undefined, 8, false, { maxLength: 17, minLength: 17, placeholder: '17 caracteres alfanuméricos' })
    @FiltroGrid('Chassi', 'texto', true)
    public numeroChassi?: string;

    @FormCampo('Ano Modelo', 'numero', true, true, undefined, 4, false, { min: 1950, max: new Date().getFullYear() + 1, placeholder: 'Ex: 2023' })
    @FiltroGrid('Ano Modelo', 'numero', true)
    public anoModelo?: number;

    @FormCampo('Ano Fabricação', 'numero', true, true, undefined, 4, false, { min: 1950, max: new Date().getFullYear(), placeholder: 'Ex: 2022' })
    @FiltroGrid('Ano Fabricação', 'numero', true)
    public anoFabricacao?: number;

    @FormCampo('Cor', 'texto', true, true, undefined, 4, false, { placeholder: 'Ex: Preto, Branco, etc.' })
    @FiltroGrid('Cor', 'texto', true)
    public cor?: string;

    @FormCampo('Renavam', 'texto', true, true, undefined, 4, false, { maxLength: 11, minLength: 9, placeholder: '9 ou 11 dígitos' })
    public renavam?: string;

    @FormCampo('Vencimento Licenciamento', 'data', true, false, undefined, 4)
    public vencimentoLicenciamento?: Date;

    @FormCampo('Tipo de Combustível', 'enum', true, true, TipoCombustivelEnum, 6)
    @FiltroGrid('Tipo de Combustível', 'enum', true, TipoCombustivelEnum, 'in')
    public tipoCombustivel?: TipoCombustivelEnum;

    @FormCampo('Descrição Combustível', 'texto', false, false, undefined, 4, true)
    public tipoCombustivelDescricao?: string;

    @FormCampo('Status', 'enum', true, true, StatusVeiculoEnum, 6)
    @FiltroGrid('Status', 'enum', true, StatusVeiculoEnum, 'in')
    public status?: StatusVeiculoEnum;

    @FormCampo('Descrição Status', 'texto', false, false, undefined, 4, true)
    public statusDescricao?: string;
    
    @FormCampoEntidade('Modelo do Veículo', true, modeloVeicularBuscaConfig, 8)
    public modeloVeiculoId?: number;

    @FormCampo('Nome do Modelo', 'texto', false, true, undefined, 8, true)
    public modeloVeiculoNome?: string;

    @FormCampo('Data Criação', 'texto', false, false, undefined, 4, true)
    public createdAt?: Date;

    @FormCampo('Data Atualização', 'texto', false, false, undefined, 4, true)
    public updatedAt?: Date;

    @FormCampo('Ano', 'texto', false, false, undefined, 4, true)
    public ano?: string;

    @FormCampo('Capacidade', 'texto', false, false)
    public capacidade?: string;

    @FormCampo('Chassi', 'texto', false, false, undefined, 4, true)
    public chassi?: string;

    @FormCampo('Ativo', 'bool', true, false, undefined, 2)
    @FiltroGrid('Ativo', 'bool', true)
    public ativo?: boolean;

    @FormCampo('Data Cadastro', 'texto', false, false, undefined, 4, true)
    public dataCadastro?: Date;

    @FormCampo('Tipo de Veículo', 'enum', true, false, TipoVeiculoEnum, 6)
    @FiltroGrid('Tipo de Veículo', 'enum', true, TipoVeiculoEnum, 'in')
    public tipoVeiculo?: TipoVeiculoEnum;

    @FormCampo('Categoria', 'texto', true, false, undefined, 4, false, { placeholder: 'Ex: Passeio, Utilitário' })
    public categoria?: string;

    @FormCampo('Categoria CNH', 'enum', true, false, CategoriaCNHEnum, 6)
    @FiltroGrid('Categoria CNH', 'enum', true, CategoriaCNHEnum, 'in')
    public categoriaCNH?: CategoriaCNHEnum;

    @FormCampo('Observação', 'textarea', true, false, undefined, 12, false, { rows: 4, maxLength: 500, placeholder: 'Informações adicionais sobre o veículo' })
    public observacao?: string;
}
