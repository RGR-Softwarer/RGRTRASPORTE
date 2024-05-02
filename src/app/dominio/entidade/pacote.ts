import { FormCampo } from '../../services/decorator/formulario-decorator';

export class Pacote {
    @FormCampo('Veiculo', 'text', true, false)
    public veiculo?: number;

    @FormCampo('Valor', 'text', true, true)
    public valor?: string;

    @FormCampo('Viagem', 'text', true, false)
    public viagem?: string;

    @FormCampo('Observação', 'text', true, false)
    public observacao?: string; 
}
