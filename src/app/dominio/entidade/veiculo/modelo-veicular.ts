import { FormCampo } from '../../../services/decorator/formulario-decorator';
import { TipoVeiculoEnum } from '../../enum/veiculo/tipo-veiculo-enum';

export class ModeloVeicular {
    @FormCampo('ID', 'numero', false)
    public id?: number;

    @FormCampo('Situacao', 'bool', true)
    public situacao?: string;

    @FormCampo('Descricao do Modelo', 'texto', true)
    public modelo?: string;

    @FormCampo('Quantidade de Assentos', 'numero', true, false)
    public Assento?: string;

    @FormCampo('Quantidade de Eixos', 'numero', true, false)
    public Eixo?: string;

    @FormCampo('Capacidade Máxima', 'numero', true, false)
    public Max?: string;

    @FormCampo('Passageiros em Pé', 'numero', true, false)
    public passageiros?: string;

    @FormCampo('Possui Banheiro', 'bool', true)
    public banheiro?: string;

    @FormCampo('Possui Climatizador', 'bool', true)
    public climatizador?: string;

    @FormCampo('Tipo', 'enum', true, false, TipoVeiculoEnum)
    public TipoVeiculoEnum?: TipoVeiculoEnum;

}
