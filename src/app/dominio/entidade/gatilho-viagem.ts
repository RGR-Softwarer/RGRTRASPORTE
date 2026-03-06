import { FormCampo, FiltroGrid, FormCampoEntidade } from '../../services/decorator/formulario-decorator';
import { Localidade } from './localidade';
import { DiaSemanaEnum } from '../enum/dia-semana-enum';

export class GatilhoViagem {
    @FormCampo('ID', 'numero', false, false, undefined, 4, true)
    public id?: number;

    @FormCampo('Nome do Gatilho', 'texto', true, true, undefined, 8, false, { placeholder: 'Ex: Viagem Manhã - Segunda a Sexta', maxLength: 100 })
    @FiltroGrid('Nome', 'texto', true)
    public nome?: string;

    @FormCampoEntidade('Localidade de Origem', true, {
        url: 'localidade',
        displayField: 'nome',
        valueField: 'id',
        searchFields: ['nome', 'cidade', 'estado'],
        modalTitle: 'Selecionar Localidade de Origem',
        modalWidth: 900,
        entidade: Localidade
    }, 6)
    public localidadeOrigemId?: number;

    @FormCampo('Origem', 'texto', false, true, undefined, 6, true)
    public localidadeOrigemNome?: string;

    @FormCampoEntidade('Localidade de Destino', true, {
        url: 'localidade',
        displayField: 'nome',
        valueField: 'id',
        searchFields: ['nome', 'cidade', 'estado'],
        modalTitle: 'Selecionar Localidade de Destino',
        modalWidth: 900,
        entidade: Localidade
    }, 6)
    public localidadeDestinoId?: number;

    @FormCampo('Destino', 'texto', false, true, undefined, 6, true)
    public localidadeDestinoNome?: string;

    @FormCampo('Horário de Saída', 'texto', true, true, undefined, 4, false, { placeholder: 'HH:mm' })
    @FiltroGrid('Horário Saída', 'texto', true)
    public horarioSaida?: string;

    @FormCampo('Horário de Chegada', 'texto', true, true, undefined, 4, false, { placeholder: 'HH:mm' })
    public horarioChegada?: string;

    @FormCampo('Segunda-feira', 'bool', true, false, undefined, 2)
    public segunda?: boolean = true;

    @FormCampo('Terça-feira', 'bool', true, false, undefined, 2)
    public terca?: boolean = true;

    @FormCampo('Quarta-feira', 'bool', true, false, undefined, 2)
    public quarta?: boolean = true;

    @FormCampo('Quinta-feira', 'bool', true, false, undefined, 2)
    public quinta?: boolean = true;

    @FormCampo('Sexta-feira', 'bool', true, false, undefined, 2)
    public sexta?: boolean = true;

    @FormCampo('Sábado', 'bool', true, false, undefined, 2)
    public sabado?: boolean = false;

    @FormCampo('Domingo', 'bool', true, false, undefined, 2)
    public domingo?: boolean = false;

    @FormCampo('Quantidade de Vagas', 'numero', true, true, undefined, 4, false, { min: 1, max: 200, placeholder: 'Número de vagas' })
    public quantidadeVagas?: number;

    @FormCampo('Descrição', 'textarea', true, false, undefined, 12, false, { rows: 3, maxLength: 500, placeholder: 'Descrição ou observações sobre este gatilho de viagem' })
    public descricao?: string;

    @FormCampo('Ativo', 'bool', true, false, undefined, 2)
    @FiltroGrid('Ativo', 'bool', true)
    public ativo?: boolean = true;

    @FormCampo('Data Criação', 'texto', false, false, undefined, 4, true)
    public createdAt?: Date;

    @FormCampo('Data Atualização', 'texto', false, false, undefined, 4, true)
    public updatedAt?: Date;
}







