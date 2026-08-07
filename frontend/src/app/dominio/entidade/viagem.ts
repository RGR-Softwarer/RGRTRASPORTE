import { FormCampo, FiltroGrid, FormCampoEntidade } from '../../services/decorator/formulario-decorator';
import { Veiculo } from './veiculo';
import { Motorista } from './motorista';
import { Localidade } from './localidade';

export class Viagem {
    @FormCampo('ID', 'numero', false, false, undefined, 4, true)
    public id?: number;

    @FormCampo('Data da Viagem', 'data', true, true, undefined, 4)
    @FiltroGrid('Data da Viagem', 'data', true)
    public dataViagem?: Date;

    @FormCampo('Horário de Saída', 'texto', true, true, undefined, 4, false, { placeholder: 'HH:mm' })
    public horarioSaida?: string;

    @FormCampo('Horário de Chegada', 'texto', true, true, undefined, 4, false, { placeholder: 'HH:mm' })
    public horarioChegada?: string;

    @FormCampoEntidade('Veículo', true, {
        url: 'veiculo',
        displayField: 'placa',
        valueField: 'id',
        searchFields: ['placa', 'modelo', 'marca'],
        modalTitle: 'Selecionar Veículo',
        modalWidth: 900,
        entidade: Veiculo
    }, 6)
    public veiculoId?: number;

    @FormCampo('Placa do Veículo', 'texto', false, true, undefined, 6, true)
    public veiculoNome?: string;

    @FormCampoEntidade('Motorista', true, {
        url: 'motorista',
        displayField: 'nome',
        valueField: 'id',
        searchFields: ['nome', 'cpf', 'cnh'],
        modalTitle: 'Selecionar Motorista',
        modalWidth: 900,
        entidade: Motorista
    }, 6)
    public motoristaId?: number;

    @FormCampo('Nome do Motorista', 'texto', false, true, undefined, 6, true)
    public motoristaNome?: string;

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

    @FormCampo('Quantidade de Vagas', 'numero', true, true, undefined, 4, false, { min: 1, max: 200, placeholder: 'Número de vagas' })
    public quantidadeVagas?: number;

    @FormCampo('Vagas Ocupadas', 'numero', false, false, undefined, 4, true)
    public vagasOcupadas?: number;

    @FormCampo('Vagas Disponíveis', 'numero', false, false, undefined, 4, true)
    public vagasDisponiveis?: number;

    @FormCampo('Distância (km)', 'numero', true, false, undefined, 4, false, { min: 0, step: 0.1, placeholder: 'Distância em km' })
    public distancia?: number;

    @FormCampo('Descrição da Viagem', 'textarea', true, false, undefined, 12, false, { rows: 3, maxLength: 500, placeholder: 'Descrição ou observações sobre a viagem' })
    public descricaoViagem?: string;

    @FormCampo('Polilinha da Rota', 'textarea', false, false, undefined, 12, false, { rows: 2, placeholder: 'Dados da rota (Google Maps Polyline)' })
    public polilinhaRota?: string;

    @FormCampo('Status da Viagem', 'texto', false, false, undefined, 4, true)
    public statusViagem?: string;

    @FormCampo('Ativo', 'bool', true, false, undefined, 2)
    @FiltroGrid('Ativo', 'bool', true)
    public ativo?: boolean = true;

    @FormCampo('Data de Início Real', 'data', false, false, undefined, 4, true)
    public dataInicioReal?: Date;

    @FormCampo('Data de Fim Real', 'data', false, false, undefined, 4, true)
    public dataFimReal?: Date;

    @FormCampo('Motivo do Cancelamento', 'textarea', false, false, undefined, 12, true, { rows: 2 })
    public motivoCancelamento?: string;

    @FormCampo('Data Criação', 'texto', false, false, undefined, 4, true)
    public createdAt?: Date;

    @FormCampo('Data Atualização', 'texto', false, false, undefined, 4, true)
    public updatedAt?: Date;
}
