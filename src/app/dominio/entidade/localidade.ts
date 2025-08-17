import { FormCampo, FiltroGrid } from '../../services/decorator/formulario-decorator';

export class Localidade {
    @FormCampo('ID', 'numero', false, false, undefined, 4, true)
    public id?: number;

    @FormCampo('Nome', 'texto', true, true, undefined, 8, false, { placeholder: 'Nome da localidade', maxLength: 100 })
    @FiltroGrid('Nome', 'texto', true)
    public nome?: string;

    @FormCampo('Estado', 'texto', true, true, undefined, 4, false, { placeholder: 'Ex: SP, RJ, MG', maxLength: 2, minLength: 2 })
    @FiltroGrid('Estado', 'texto', true)
    public estado?: string;

    @FormCampo('Cidade', 'texto', true, true, undefined, 8, false, { placeholder: 'Nome da cidade', maxLength: 100 })
    @FiltroGrid('Cidade', 'texto', true)
    public cidade?: string;

    @FormCampo('CEP', 'texto', true, true, undefined, 4, false, { placeholder: '00000-000', maxLength: 9, minLength: 8 })
    public cep?: string;

    @FormCampo('Bairro', 'texto', true, true, undefined, 6, false, { placeholder: 'Nome do bairro', maxLength: 100 })
    public bairro?: string;

    @FormCampo('Logradouro', 'texto', true, true, undefined, 8, false, { placeholder: 'Rua, Avenida, etc.', maxLength: 200 })
    public logradouro?: string;

    @FormCampo('Número', 'texto', true, false, undefined, 4, false, { placeholder: 'Número do endereço', maxLength: 10 })
    public numero?: string;

    @FormCampo('Complemento', 'texto', false, false, undefined, 8, false, { placeholder: 'Complemento (opcional)', maxLength: 100 })
    public complemento?: string;

    @FormCampo('Latitude', 'numero', true, true, undefined, 6, false, { placeholder: 'Ex: -23.5505', step: 0.000001, min: -90, max: 90 })
    public latitude?: number;

    @FormCampo('Longitude', 'numero', true, true, undefined, 6, false, { placeholder: 'Ex: -46.6333', step: 0.000001, min: -180, max: 180 })
    public longitude?: number;

    @FormCampo('Ativo', 'bool', true, false, undefined, 2)
    @FiltroGrid('Ativo', 'bool', true)
    public ativo?: boolean = true;

    @FormCampo('Data Criação', 'texto', false, false, undefined, 4, true)
    public createdAt?: Date;

    @FormCampo('Data Atualização', 'texto', false, false, undefined, 4, true)
    public updatedAt?: Date;
}