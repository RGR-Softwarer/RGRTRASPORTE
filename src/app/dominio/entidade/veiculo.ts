import { FormCampo } from '../../services/decorator/formulario-decorator';

export class Veiculo {
    @FormCampo('ID', 'number', false)
    public id?: number;

    @FormCampo('Placa', 'text', true, true)
    public placa?: string;

    @FormCampo('Marca', 'text')
    public marca?: string;

    @FormCampo('Modelo', 'text')
    public modelo?: string;

    @FormCampo('Ano', 'number')
    public ano?: number;

    @FormCampo('Cor', 'text')
    public cor?: string;

    @FormCampo('Combustível', 'text')
    public combustivel?: string;

    @FormCampo('Valor', 'number')
    public valor?: number;
}
