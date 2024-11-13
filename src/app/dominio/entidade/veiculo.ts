import { FormCampo } from '../../services/decorator/formulario-decorator';
import { CategoriaCNHEnum } from '../enum/veiculo/categoria-cnh-enum';
import { StatusVeiculoEnum } from '../enum/veiculo/status-veiculo-enum';
import { TipoCombustivelEnum } from '../enum/veiculo/tipo-combustivel-enum';
import { TipoVeiculoEnum } from '../enum/veiculo/tipo-veiculo-enum';

export class Veiculo {
    @FormCampo('ID', 'numero', false)
    public id?: number;

    @FormCampo('Placa', 'texto', true, true)
    public placa?: string;

    @FormCampo('Modelo', 'texto', true, false)
    public modelo?: string;

    @FormCampo('Marca', 'texto', true, false)
    public marca?: string;

    @FormCampo('Ano', 'texto', true, true)
    public ano?: string;

    @FormCampo('Cor', 'texto', true, false)
    public cor?: string;

    @FormCampo('Renavam', 'numero', true, false)
    public renavam?: string;

    @FormCampo('Tipo de Combustível', 'enum', true, false, TipoCombustivelEnum)
    public tipoCombustivel?: TipoCombustivelEnum;

    @FormCampo('Tipo de Veículo', 'enum', true, false, TipoVeiculoEnum)
    public tipoVeiculo?: TipoVeiculoEnum;

    @FormCampo('Categoria', 'texto', true, false)
    public categoria?: string;

    @FormCampo('Capacidade', 'texto', true, false)
    public capacidade?: string;

    @FormCampo('Categoria CNH', 'enum', true, false, CategoriaCNHEnum)
    public categoriaCNH?: CategoriaCNHEnum;

    @FormCampo('Status', 'enum', true, false, StatusVeiculoEnum)
    public status?: StatusVeiculoEnum;

    @FormCampo('Observação', 'texto', true, false)
    public observacao?: string;
}
