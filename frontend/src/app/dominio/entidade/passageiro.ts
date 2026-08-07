import { FormCampo, FiltroGrid, FormCampoEntidade } from '../../services/decorator/formulario-decorator';
import { SexoEnum } from '../enum/sexo-enum';

export class Passageiro {
    @FormCampo('ID', 'numero', false, false, undefined, 4, true)
    public id?: number;

    @FormCampo('Nome', 'texto', true, true, undefined, 8, false, { placeholder: 'Nome completo do passageiro', maxLength: 150 })
    @FiltroGrid('Nome', 'texto', true)
    public nome?: string;

    @FormCampo('CPF', 'texto', true, true, undefined, 4, false, { placeholder: '000.000.000-00', maxLength: 14, minLength: 11 })
    @FiltroGrid('CPF', 'texto', true)
    public cpf?: string;

    @FormCampo('Telefone', 'texto', true, true, undefined, 4, false, { placeholder: '(00) 00000-0000', maxLength: 15 })
    public telefone?: string;

    @FormCampo('E-mail', 'email', true, false, undefined, 8, false, { placeholder: 'exemplo@email.com', maxLength: 100 })
    public email?: string;

    @FormCampo('Sexo', 'enum', true, true, SexoEnum, 4)
    @FiltroGrid('Sexo', 'enum', true, SexoEnum, 'in')
    public sexo?: SexoEnum;

    @FormCampo('Descrição Sexo', 'texto', false, false, undefined, 4, true)
    public sexoDescricao?: string;

    @FormCampoEntidade('Localidade de Residência', true, undefined, 8)
    public localidadeId?: number;

    @FormCampo('Nome da Localidade', 'texto', false, true, undefined, 8, true)
    public localidadeNome?: string;

    @FormCampoEntidade('Localidade de Embarque', false, undefined, 6)
    public localidadeEmbarqueId?: number;

    @FormCampo('Nome da Localidade de Embarque', 'texto', false, true, undefined, 6, true)
    public localidadeEmbarqueNome?: string;

    @FormCampoEntidade('Localidade de Desembarque', false, undefined, 6)
    public localidadeDesembarqueId?: number;

    @FormCampo('Nome da Localidade de Desembarque', 'texto', false, true, undefined, 6, true)
    public localidadeDesembarqueNome?: string;

    @FormCampo('Observação', 'textarea', false, false, undefined, 12, false, { rows: 3, maxLength: 500, placeholder: 'Informações adicionais sobre o passageiro' })
    public observacao?: string;

    @FormCampo('Situação', 'bool', true, false, undefined, 2)
    @FiltroGrid('Situação', 'bool', true)
    public situacao?: boolean = true;

    @FormCampo('Data Criação', 'texto', false, false, undefined, 4, true)
    public createdAt?: Date;

    @FormCampo('Data Atualização', 'texto', false, false, undefined, 4, true)
    public updatedAt?: Date;
}