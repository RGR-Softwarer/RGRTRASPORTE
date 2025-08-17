# 🎯 **BOUNDED CONTEXTS - PROJETO RGRTRASPORTE**

## 📋 **DEFINIÇÃO DOS BOUNDED CONTEXTS**

Com base na análise do domínio e das entidades existentes, foram identificados os seguintes Bounded Contexts:

### **1. 🚌 Transporte Context (Contexto Principal)**
**Responsabilidade**: Gerenciar operações de transporte, viagens e rotas.

**Aggregate Roots**:
- `Viagem` - Viagem específica com data, horário e passageiros
- `GatilhoViagem` - Template para geração automática de viagens

**Entidades**:
- `ViagemPassageiro` - Relacionamento entre viagem e passageiro
- `ViagemPosicao` - Posições GPS durante a viagem

**Value Objects**:
- `CodigoViagem` - Código único da viagem
- `PeriodoViagem` - Período de duração da viagem
- `Polilinha` - Rota geográfica
- `Distancia` - Distância em quilômetros
- `Dinheiro` - Valores monetários (tarifa, custos)

---

### **2. 🚐 Frota Context**
**Responsabilidade**: Gestão da frota de veículos e modelos.

**Aggregate Roots**:
- `Veiculo` - Veículo específico da frota
- `ModeloVeicular` - Modelo/tipo de veículo

**Value Objects**:
- `Placa` - Placa do veículo
- `Chassi` - Número do chassi
- `CapacidadeVeiculo` - Capacidade de passageiros

**Domain Services**:
- `VeiculoValidationService` - Validações específicas de veículos
- `ModeloVeicularValidationService` - Validações de modelos

---

### **3. 👥 Pessoas Context**
**Responsabilidade**: Gestão de pessoas (motoristas e passageiros).

**Aggregate Roots**:
- `Motorista` - Condutor autorizado
- `Passageiro` - Cliente que utiliza o transporte

**Value Objects**:
- `CPF` - Documento de identificação
- `CNH` - Carteira de motorista (específico para motoristas)

**Domain Services**:
- `PessoaValidationService` - Validações comuns
- `MotoristaValidationService` - Validações específicas de motoristas
- `PassageiroValidationService` - Validações específicas de passageiros

---

### **4. 📍 Geografia Context**
**Responsabilidade**: Gestão de localidades e endereços.

**Aggregate Roots**:
- `Localidade` - Pontos de origem, destino e embarque

**Value Objects**:
- `Endereco` - Endereço completo
- `Coordenada` - Posição geográfica (lat/long)

**Domain Services**:
- `LocalidadeValidationService` - Validações de localidades

---

## 🔄 **COMUNICAÇÃO ENTRE CONTEXTS**

### **Domain Events para Integração**
Os Bounded Contexts se comunicam através de Domain Events:

```
Pessoas Context → Transporte Context
- MotoristaCriadoEvent
- PassageiroCriadoEvent
- MotoristaDocumentosAtualizadosEvent

Frota Context → Transporte Context  
- VeiculoCriadoEvent
- ModeloVeicularCriadoEvent
- VeiculoManutencaoEvent

Geografia Context → Transporte Context
- LocalidadeCriadaEvent
- LocalidadeAtualizadaEvent

Transporte Context → Todos
- ViagemCriadaEvent
- ViagemIniciadaEvent
- ViagemFinalizadaEvent
```

### **Anti-Corruption Layers**
Para manter a integridade dos contextos, serão implementadas camadas anti-corrupção:

- **TransporteToFrotaACL** - Traduz conceitos entre Transporte e Frota
- **TransporteToPessoasACL** - Traduz conceitos entre Transporte e Pessoas
- **TransporteToGeografiaACL** - Traduz conceitos entre Transporte e Geografia

## 📁 **ESTRUTURA DE DIRETÓRIOS PROPOSTA**

```
Dominio/
├── BoundedContexts/
│   ├── Transporte/
│   │   ├── Aggregates/
│   │   │   ├── Viagem/
│   │   │   └── GatilhoViagem/
│   │   ├── ValueObjects/
│   │   ├── Events/
│   │   ├── Services/
│   │   └── Specifications/
│   ├── Frota/
│   │   ├── Aggregates/
│   │   │   ├── Veiculo/
│   │   │   └── ModeloVeicular/
│   │   ├── ValueObjects/
│   │   ├── Events/
│   │   ├── Services/
│   │   └── Specifications/
│   ├── Pessoas/
│   │   ├── Aggregates/
│   │   │   ├── Motorista/
│   │   │   └── Passageiro/
│   │   ├── ValueObjects/
│   │   ├── Events/
│   │   ├── Services/
│   │   └── Specifications/
│   └── Geografia/
│       ├── Aggregates/
│       │   └── Localidade/
│       ├── ValueObjects/
│       ├── Events/
│       ├── Services/
│       └── Specifications/
├── SharedKernel/
│   ├── ValueObjects/
│   │   ├── Dinheiro.cs
│   │   └── Coordenada.cs
│   ├── Interfaces/
│   └── Common/
└── AntiCorruptionLayers/
    ├── TransporteToFrotaACL.cs
    ├── TransporteToPessoasACL.cs
    └── TransporteToGeografiaACL.cs
```

## 🎯 **BENEFÍCIOS DA IMPLEMENTAÇÃO**

### **Separação Clara de Responsabilidades**
- Cada contexto tem um propósito bem definido
- Redução de acoplamento entre áreas de negócio
- Facilita manutenção e evolução independente

### **Escalabilidade**
- Possibilidade de extrair contextos para microservices
- Times independentes podem trabalhar em contextos diferentes
- Deploy independente por contexto

### **Testabilidade**
- Testes focados em cada contexto
- Mocks e stubs mais simples
- Isolamento de dependências

### **Evolução do Domínio**
- Mudanças em um contexto não afetam outros
- Facilita refatorações e melhorias
- Permite experimentação segura

## 🚀 **PRÓXIMOS PASSOS PARA IMPLEMENTAÇÃO**

### **Fase 1: Reorganização**
1. Mover entidades para seus respectivos contexts
2. Criar interfaces de comunicação
3. Implementar Domain Events entre contexts

### **Fase 2: Anti-Corruption Layers**
1. Criar camadas de tradução
2. Definir contratos entre contexts
3. Implementar validações de integridade

### **Fase 3: Microservices (Futuro)**
1. Extrair contextos para serviços independentes
2. Implementar comunicação via eventos assíncronos
3. Criar APIs específicas para cada contexto

---

## ✅ **CHECKLIST DE IMPLEMENTAÇÃO**

- [ ] **Reorganizar estrutura de pastas**
- [ ] **Mover Aggregates para contexts específicos**
- [ ] **Implementar Domain Events entre contexts**
- [ ] **Criar Anti-Corruption Layers**
- [ ] **Atualizar testes para nova estrutura**
- [ ] **Documentar interfaces entre contexts**
- [ ] **Implementar contratos de comunicação**
- [ ] **Validar isolamento de responsabilidades**

Essa estrutura de Bounded Contexts fornece uma base sólida para o crescimento e evolução da aplicação, mantendo a complexidade sob controle e facilitando futuras melhorias arquiteturais.
