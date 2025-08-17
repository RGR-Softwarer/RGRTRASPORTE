# RGR Transportador - Sistema de Gestão de Transporte

## Visão Geral

O RGR Transportador é um sistema completo de gestão de transporte desenvolvido em Angular 18, focado no controle de frota, motoristas, clientes e relatórios gerenciais.

## Funcionalidades Implementadas

### 🚗 Frota
- **Veículos**: Gestão completa da frota com cadastro, edição e visualização de veículos
- Controle de status (Ativo, Manutenção, Inativo)
- Gestão de documentos (CNH, licenciamento, etc.)

### 👥 Cadastros

#### Motoristas
- Listagem de motoristas com filtros avançados
- Controle de CNH e vencimentos
- Status de atividade
- Busca por nome, CPF ou CNH
- Indicadores visuais de vencimento de documentos

#### Clientes
- Cadastro de clientes PJ e PF
- Gestão de documentos (CNPJ/CPF)
- Controle de status ativo/inativo
- Filtros por tipo de pessoa e status
- Observações e informações de contato

#### Outros Cadastros (Estrutura Preparada)
- Fornecedores
- Tipos de Manutenção
- Tipos de Combustível
- Configurações do Sistema

### 📊 Relatórios

#### Dashboard Gerencial
- **Indicadores Principais**:
  - Total de Veículos (45 unidades, +12.5%)
  - Motoristas Ativos (28 pessoas, +8.3%)
  - Clientes Ativos (156 clientes, +15.2%)
  - Faturamento Mensal (R$ 125.450, +22.1%)
  - Viagens Realizadas (342 viagens, -5.8%)
  - Manutenções Pendentes (8 veículos, -25.0%)

- **Gráficos**:
  - Distribuição de veículos por status
  - Evolução do faturamento mensal
  - Quantidade de viagens por mês

- **Resumo Executivo**:
  - Pontos positivos
  - Pontos de atenção
  - Metas para próximo mês

#### Categorias de Relatórios
- **Frota**: Veículos, Manutenções, Combustível, Motoristas
- **Financeiro**: Custos, Faturamento
- **Operacional**: Viagens, Clientes
- **Gerencial**: Dashboard, Performance

## Tecnologias Utilizadas

- **Angular 18**: Framework principal
- **TypeScript**: Linguagem de programação
- **SCSS**: Estilização avançada
- **Font Awesome**: Ícones
- **Bootstrap 5**: Framework CSS (estrutura base)
- **RxJS**: Programação reativa

## Estrutura do Projeto

```
src/app/
├── paginas/
│   ├── auth/                 # Autenticação
│   ├── dashboard/            # Dashboard principal
│   ├── frota/               # Gestão de frota
│   │   └── veiculo/         # Veículos
│   ├── cadastros/           # Cadastros básicos
│   │   ├── motoristas/      # Motoristas
│   │   └── clientes/        # Clientes
│   └── relatorios/          # Relatórios
│       └── gerencial/       # Dashboard gerencial
├── componentes/             # Componentes reutilizáveis
├── services/               # Serviços
├── dominio/                # Entidades e enums
└── shared/                 # Recursos compartilhados
```

## Características do Design

### Interface Moderna
- Design responsivo e adaptável
- Cards com hover effects
- Cores consistentes e acessíveis
- Ícones intuitivos
- Animações suaves

### UX/UI
- Navegação intuitiva com breadcrumbs
- Filtros avançados em listagens
- Estados vazios informativos
- Feedback visual para ações
- Layout adaptativo para mobile

### Componentes Reutilizáveis
- Cards de indicadores
- Tabelas de dados
- Filtros de busca
- Botões e badges
- Gráficos placeholder

## Funcionalidades Técnicas

### Filtros e Busca
- Busca em tempo real
- Filtros múltiplos
- Limpeza de filtros
- Persistência de estado

### Validações
- Formatação automática de documentos
- Validação de vencimentos
- Indicadores visuais de status
- Verificação de dados obrigatórios

### Responsividade
- Layout adaptativo
- Componentes mobile-friendly
- Grid system flexível
- Breakpoints otimizados

## Próximos Passos

### Funcionalidades Pendentes
1. **Formulários de Cadastro**: Implementar telas de adicionar/editar
2. **Autenticação**: Integrar com backend de autenticação
3. **API Integration**: Conectar com serviços backend
4. **Gráficos Reais**: Implementar biblioteca de gráficos (Chart.js)
5. **Exportação**: Funcionalidade de exportar relatórios
6. **Notificações**: Sistema de alertas e notificações

### Melhorias Planejadas
- Dashboard personalizável
- Relatórios customizáveis
- Sistema de permissões
- Auditoria de ações
- Backup automático
- Integração com APIs externas

## Como Executar

1. **Instalar dependências**:
   ```bash
   npm install
   ```

2. **Executar em desenvolvimento**:
   ```bash
   npm start
   ```

3. **Build para produção**:
   ```bash
   npm run build
   ```

## Contribuição

O projeto segue padrões de código consistentes:
- Componentes standalone
- TypeScript strict mode
- SCSS com BEM methodology
- Responsive design
- Accessibility guidelines

---

**Desenvolvido por RGR Sistemas** - Sistema completo de gestão de transporte