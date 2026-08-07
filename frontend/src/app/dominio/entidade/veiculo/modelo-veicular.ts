import { FormCampo, FiltroGrid } from '../../../services/decorator/formulario-decorator';
import { TipoModeloVeiculoEnum } from '../../enum/veiculo/tipo-modelo-veiculo-enum';

export class ModeloVeicular {
    @FormCampo('ID', 'numero', false)
    public id?: number;

    @FormCampo('Situação', 'bool', true, true)
    @FiltroGrid('Situação', 'bool', true)
    public situacao?: boolean;

    @FormCampo('Situação Descrição', 'texto', false, true)
    public situacaoDescricao?: string;

    @FormCampo('Descrição', 'texto', true, true)
    public descricao?: string;

    @FormCampo('Descrição do Modelo', 'texto', true, true)
    @FiltroGrid('Descrição do Modelo', 'texto', true)
    public descricaoModelo?: string;

    @FormCampo('Tipo', 'enum', true, false, TipoModeloVeiculoEnum)
    @FiltroGrid('Tipo', 'enum', true, TipoModeloVeiculoEnum, 'equals')
    public tipo?: TipoModeloVeiculoEnum;

    @FormCampo('Tipo Descrição', 'texto', false, true)
    public tipoDescricao?: string;

    @FormCampo('Quantidade de Assentos', 'numero', true, false)
    public quantidadeAssento?: number;

    @FormCampo('Quantidade de Eixos', 'numero', true, false)
    public quantidadeEixo?: number;

    @FormCampo('Capacidade Máxima', 'numero', true, false)
    public capacidadeMaxima?: number;

    @FormCampo('Passageiros em Pé', 'numero', true, false)
    public passageirosEmPe?: number;

    @FormCampo('Possui Banheiro', 'bool', true, false)
    public possuiBanheiro?: boolean;

    @FormCampo('Possui Banheiro Descrição', 'texto', false, true)
    public possuiBanheiroDescricao?: string;

    @FormCampo('Possui Climatizador', 'bool', true, false)
    public possuiClimatizador?: boolean;

    @FormCampo('Possui Climatizador Descrição', 'texto', false, true)
    public possuiClimatizadorDescricao?: string;

    @FormCampo('Data Criação', 'texto', false, true)
    public createdAt?: Date;

    @FormCampo('Data Atualização', 'texto', false, true)
    public updatedAt?: Date;
}
