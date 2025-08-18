import { FormCampo, FiltroGrid, FormCampoEntidade } from '../../services/decorator/formulario-decorator';

export class Viagem {
    @FormCampo('ID', 'numero', false, false, undefined, 4, true)
    public id?: number;

    @FormCampo('Data da Viagem', 'data', true, true, undefined, 4)
    @FiltroGrid('Data da Viagem', 'data', true)
    public dataViagem?: Date;

    @FormCampo('Horário de Saída', 'texto', true, true, undefined, 4)
    public horarioSaida?: string; // TimeSpan será representado como string no frontend

    @FormCampo('Horário de Chegada', 'texto', true, true, undefined, 4)
    public horarioChegada?: string; // TimeSpan será representado como string no frontend

    @FormCampoEntidade('Veículo', true, undefined, 6)
    public veiculoId?: number;

    @FormCampo('Nome do Veículo', 'texto', false, true, undefined, 6, true)
    public veiculoNome?: string;

    @FormCampo('Placa do Veículo', 'texto', false, true, undefined, 4, true)
    public veiculoPlaca?: string;

    @FormCampoEntidade('Motorista', true, undefined, 6)
    public motoristaId?: number;

    @FormCampo('Nome do Motorista', 'texto', false, true, undefined, 6, true)
    public motoristaNome?: string;

    @FormCampoEntidade('Localidade de Origem', true, undefined, 6)
    public localidadeOrigemId?: number;

    @FormCampo('Nome da Localidade de Origem', 'texto', false, true, undefined, 6, true)
    public localidadeOrigemNome?: string;

    @FormCampoEntidade('Localidade de Destino', true, undefined, 6)
    public localidadeDestinoId?: number;

    @FormCampo('Nome da Localidade de Destino', 'texto', false, true, undefined, 6, true)
    public localidadeDestinoNome?: string;

    @FormCampo('Quantidade de Vagas', 'numero', true, true, undefined, 4, false, { min: 1, max: 200 })
    public quantidadeVagas?: number;

    @FormCampo('Vagas Ocupadas', 'numero', false, false, undefined, 4, true)
    public vagasOcupadas?: number;

    @FormCampo('Vagas Disponíveis', 'numero', false, false, undefined, 4, true)
    public vagasDisponiveis?: number;

    @FormCampo('Distância (km)', 'numero', true, false, undefined, 4, false, { min: 0, step: 0.1 })
    public distancia?: number;

    @FormCampo('Descrição da Viagem', 'textarea', false, false, undefined, 12, false, { rows: 3, maxLength: 500, placeholder: 'Descrição ou observações sobre a viagem' })
    public descricaoViagem?: string;

    @FormCampo('Polilinha da Rota', 'textarea', false, false, undefined, 12, false, { rows: 2, placeholder: 'Dados da rota (Google Maps Polyline)' })
    public polilinhaRota?: string;

    @FormCampo('Status da Viagem', 'texto', false, false, undefined, 4, true)
    public statusViagem?: string;

    @FormCampo('Ativo', 'bool', true, false, undefined, 2)
    @FiltroGrid('Ativo', 'bool', true)
    public ativo?: boolean = true;

    @FormCampoEntidade('Gatilho de Viagem', false, undefined, 6)
    public gatilhoViagemId?: number;

    @FormCampo('Nome do Gatilho', 'texto', false, true, undefined, 6, true)
    public gatilhoViagemNome?: string;

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