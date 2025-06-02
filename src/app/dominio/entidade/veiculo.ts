import { FormCampo, FiltroGrid } from '../../services/decorator/formulario-decorator';
import { CategoriaCNHEnum } from '../enum/veiculo/categoria-cnh-enum';
import { StatusVeiculoEnum } from '../enum/veiculo/status-veiculo-enum';
import { TipoCombustivelEnum } from '../enum/veiculo/tipo-combustivel-enum';
import { TipoVeiculoEnum } from '../enum/veiculo/tipo-veiculo-enum';

export class Veiculo {
    @FormCampo('ID', 'numero', false, false)
    public id?: number;

    @FormCampo('Placa', 'texto', true, true)
    @FiltroGrid('Placa', 'texto', true)
    public placa?: string;

    @FormCampo('Placa Formatada', 'texto', false, false, undefined, 4, true)
    public placaFormatada?: string;

    @FormCampo('Modelo', 'texto', true, true)
    @FiltroGrid('Modelo', 'texto', true)
    public modelo?: string;

    @FormCampo('Marca', 'texto', true, true)
    @FiltroGrid('Marca', 'texto', true)
    public marca?: string;

    @FormCampo('Número do Chassi', 'texto', true, true)
    @FiltroGrid('Chassi', 'texto', true)
    public numeroChassi?: string;

    @FormCampo('Ano Modelo', 'numero', true, true)
    @FiltroGrid('Ano Modelo', 'numero', true)
    public anoModelo?: number;

    @FormCampo('Ano Fabricação', 'numero', true, true)
    @FiltroGrid('Ano Fabricação', 'numero', true)
    public anoFabricacao?: number;

    @FormCampo('Cor', 'texto', true, true)
    @FiltroGrid('Cor', 'texto', true)
    public cor?: string;

    @FormCampo('Renavam', 'texto', true, true)
    public renavam?: string;

    @FormCampo('Vencimento Licenciamento', 'data', true, false)
    public vencimentoLicenciamento?: Date;

    @FormCampo('Tipo de Combustível', 'enum', true, true, TipoCombustivelEnum)
    @FiltroGrid('Tipo de Combustível', 'enum', true, TipoCombustivelEnum)
    public tipoCombustivel?: TipoCombustivelEnum;

    @FormCampo('Descrição Combustível', 'texto', false, false, undefined, 4, true)
    public tipoCombustivelDescricao?: string;

    @FormCampo('Status', 'enum', true, true, StatusVeiculoEnum)
    @FiltroGrid('Status', 'enum', true, StatusVeiculoEnum)
    public status?: StatusVeiculoEnum;

    @FormCampo('Descrição Status', 'texto', false, false, undefined, 4, true)
    public statusDescricao?: string;

    @FormCampo('Observação', 'textarea', true, false)
    public observacao?: string;

    @FormCampo('Modelo Veículo ID', 'numero', true, false)
    public modeloVeiculoId?: number;

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

    @FormCampo('Ativo', 'bool', false, false)
    @FiltroGrid('Ativo', 'bool', true)
    public ativo?: boolean;

    @FormCampo('Data Cadastro', 'texto', false, false, undefined, 4, true)
    public dataCadastro?: Date;

    @FormCampo('Tipo de Veículo', 'enum', false, false, TipoVeiculoEnum)
    @FiltroGrid('Tipo de Veículo', 'enum', true, TipoVeiculoEnum)
    public tipoVeiculo?: TipoVeiculoEnum;

    @FormCampo('Categoria', 'texto', false, false)
    public categoria?: string;

    @FormCampo('Categoria CNH', 'enum', false, false, CategoriaCNHEnum)
    @FiltroGrid('Categoria CNH', 'enum', true, CategoriaCNHEnum)
    public categoriaCNH?: CategoriaCNHEnum;
}
