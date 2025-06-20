import { environment } from '../../../../environments/environment';

const URL_MODELO_VEICULAR = `${environment.apiBaseUrl}/ModeloVeicular`;

export const modeloVeicularBuscaConfig = {
    url: URL_MODELO_VEICULAR,
    displayField: 'descricaoModelo',
    valueField: 'id',
    searchFields: ['tipo', 'descricaoModelo'],
    modalTitle: 'Selecionar Modelo de Veículo'
}; 