import { FormCampo } from '../../services/decorator/formulario-decorator';
import { CategoriaCNHEnum } from '../enum/veiculo/categoria-cnh-enum';
import { StatusVeiculoEnum } from '../enum/veiculo/status-veiculo-enum';
import { TipoCombustivelEnum } from '../enum/veiculo/tipo-combustivel-enum';
import { TipoVeiculoEnum } from '../enum/veiculo/tipo-veiculo-enum';

export class Veiculo {
    @FormCampo('ID', 'number', false)
    public id?: number;

    @FormCampo('Placa', 'text', true, true)
    public placa?: string;

    @FormCampo('Modelo', 'text', true, false)
    public modelo?: string;

    @FormCampo('Marca', 'text', true, false)
    public marca?: string;

    @FormCampo('Ano', 'text', true, false)
    public ano?: string;

    @FormCampo('Cor', 'text', true, false)
    public cor?: string;

    @FormCampo('Renavam', 'number', true, false)
    public renavam?: string;

    @FormCampo('Tipo de Combustível', 'select', true, false, TipoCombustivelEnum)
    public tipoCombustivel?: TipoCombustivelEnum;

    @FormCampo('Tipo de Veículo', 'select', true, false, TipoVeiculoEnum)
    public tipoVeiculo?: TipoVeiculoEnum;

    @FormCampo('Categoria', 'text', true, false)
    public categoria?: string;

    @FormCampo('Capacidade', 'text', true, false)
    public capacidade?: string;

    @FormCampo('Categoria CNH', 'select', true, false, CategoriaCNHEnum)
    public categoriaCNH?: CategoriaCNHEnum;

    @FormCampo('Status', 'select', true, false, StatusVeiculoEnum)
    public status?: StatusVeiculoEnum;

    @FormCampo('Observação', 'text', true, false)
    public observacao?: string;
}
