import { FormCampo } from '../../services/decorator/formulario-decorator';


export class Viagem {

    @FormCampo('Hora de Embarque', 'text', true, true)
    public embarque?: string;

    @FormCampo('Hora de Saída', 'text', true, false)
    public saida?: string;

    @FormCampo('Quant. passageiros', 'text', true, false)
    public passageiros?: string;

    @FormCampo('Embarque', 'text', true, false)
    public Embarque?: string;

    @FormCampo('Desembarque', 'text', true, false)
    public Desembarque?: string;
}