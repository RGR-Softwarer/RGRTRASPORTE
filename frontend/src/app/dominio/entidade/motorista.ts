import { FormCampo, FiltroGrid, FormCampoEntidade } from '../../services/decorator/formulario-decorator';
import { SexoEnum } from '../enum/sexo-enum';
import { CategoriaCNHEnum } from '../enum/veiculo/categoria-cnh-enum';

export class Motorista {
    @FormCampo('ID', 'numero', false, false, undefined, 4, true)
    public id?: number;

    @FormCampo('Nome', 'texto', true, true, undefined, 8, false, { placeholder: 'Nome completo do motorista', maxLength: 150 })
    @FiltroGrid('Nome', 'texto', true)
    public nome?: string;

    @FormCampo('CPF', 'texto', true, true, undefined, 4, false, { placeholder: '000.000.000-00', maxLength: 14, minLength: 11 })
    @FiltroGrid('CPF', 'texto', true)
    public cpf?: string;

    @FormCampo('RG', 'texto', true, true, undefined, 4, false, { placeholder: 'RG do motorista', maxLength: 20 })
    public rg?: string;

    @FormCampo('Telefone', 'texto', true, true, undefined, 4, false, { placeholder: '(00) 00000-0000', maxLength: 15 })
    public telefone?: string;

    @FormCampo('E-mail', 'email', true, false, undefined, 8, false, { placeholder: 'exemplo@email.com', maxLength: 100 })
    public email?: string;

    @FormCampo('Sexo', 'enum', true, true, SexoEnum, 4)
    @FiltroGrid('Sexo', 'enum', true, SexoEnum, 'in')
    public sexo?: SexoEnum;

    @FormCampo('Descrição Sexo', 'texto', false, false, undefined, 4, true)
    public sexoDescricao?: string;

    @FormCampo('CNH', 'texto', true, true, undefined, 4, false, { placeholder: '00000000000', maxLength: 11, minLength: 11 })
    @FiltroGrid('CNH', 'texto', true)
    public cnh?: string;

    @FormCampo('Categoria CNH', 'enum', true, true, CategoriaCNHEnum, 4)
    @FiltroGrid('Categoria CNH', 'enum', true, CategoriaCNHEnum, 'in')
    public categoriaCNH?: CategoriaCNHEnum;

    @FormCampo('Validade CNH', 'data', true, true, undefined, 4)
    public validadeCNH?: Date;

    @FormCampo('Observação', 'textarea', false, false, undefined, 12, false, { rows: 3, maxLength: 500, placeholder: 'Informações adicionais sobre o motorista' })
    public observacao?: string;

    @FormCampo('Situação', 'bool', true, false, undefined, 2)
    @FiltroGrid('Situação', 'bool', true)
    public situacao?: boolean = true;

    @FormCampo('Data Criação', 'texto', false, false, undefined, 4, true)
    public createdAt?: Date;

    @FormCampo('Data Atualização', 'texto', false, false, undefined, 4, true)
    public updatedAt?: Date;
}







