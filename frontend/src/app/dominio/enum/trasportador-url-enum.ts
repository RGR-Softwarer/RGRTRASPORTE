import { environment } from '../../../environments/environment';

const baseUrl = environment.apiTrasportador;

export const TrasportadorUrlEnum = {
    // Veículos
    VEICULOS: `${baseUrl}veiculo`,
    VEICULO_POR_ID: (id: number) => `${baseUrl}veiculo/${id}`,
    
    // Passageiros
    PASSAGEIROS: `${baseUrl}passageiro`,
    PASSAGEIRO_POR_ID: (id: number) => `${baseUrl}passageiro/${id}`,
    
    // Motoristas
    MOTORISTAS: `${baseUrl}motorista`,
    MOTORISTA_POR_ID: (id: number) => `${baseUrl}motorista/${id}`,
    
    // Localidades
    LOCALIDADES: `${baseUrl}localidade`,
    LOCALIDADE_POR_ID: (id: number) => `${baseUrl}localidade/${id}`,
    
    // Modelos Veiculares
    MODELOS_VEICULARES: `${baseUrl}modelo-veicular`,
    MODELO_VEICULAR_POR_ID: (id: number) => `${baseUrl}modelo-veicular/${id}`,
    
    // Viagens
    VIAGENS: `${baseUrl}viagem`,
    VIAGEM_POR_ID: (id: number) => `${baseUrl}viagem/${id}`,
    VIAGEM_INICIAR: (id: number) => `${baseUrl}viagem/${id}/iniciar`,
    VIAGEM_FINALIZAR: (id: number) => `${baseUrl}viagem/${id}/finalizar`,
    VIAGEM_CANCELAR: (id: number) => `${baseUrl}viagem/${id}/cancelar`,
    VIAGEM_PASSAGEIROS: (id: number) => `${baseUrl}viagem/${id}/passageiro`,
    VIAGEM_PASSAGEIRO_REMOVER: (viagemId: number, passageiroId: number) => `${baseUrl}viagem/${viagemId}/passageiro/${passageiroId}`,
    VIAGEM_POSICOES: (id: number) => `${baseUrl}viagem/${id}/posicao`,
    
    // Gatilhos de Viagem
    GATILHOS_VIAGEM: `${baseUrl}gatilho-viagem`,
    GATILHO_VIAGEM_POR_ID: (id: number) => `${baseUrl}gatilho-viagem/${id}`,
    
    // Dashboard
    DASHBOARD_ESTATISTICAS: `${baseUrl}dashboard/estatisticas`,
    DASHBOARD_VIAGENS_HOJE: `${baseUrl}dashboard/viagens-hoje`,
    DASHBOARD_VEICULOS_EM_VIAGEM: `${baseUrl}dashboard/veiculos-em-viagem`,
    
    // Auditoria
    AUDITORIA: `${baseUrl}auditoria`,
} as const;

// Manter compatibilidade com código antigo
export const TrasportadorUrls = {
    ObterTodos: `${environment.apiTrasportador}`,
} as const;
