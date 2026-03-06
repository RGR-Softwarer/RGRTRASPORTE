# Refatoração: AutoMapper → Mapeamento Manual em CQRS

## 📋 Análise: AutoMapper vs Mapeamento Manual em CQRS

### ❌ Problemas do AutoMapper em CQRS

1. **Ocultação de Lógica**: O mapeamento fica escondido em configurações, dificultando entender o que está sendo transformado
2. **Dependência Externa**: Adiciona uma dependência que pode não ser necessária
3. **Performance**: Overhead de reflection e configuração
4. **Debugging**: Mais difícil debugar quando algo dá errado
5. **Flexibilidade**: Limita transformações complexas que podem ser necessárias em queries específicas
6. **Violação de Princípios CQRS**: Em CQRS puro, cada query pode ter seu próprio formato de DTO, não necessariamente 1:1 com a entidade

### ✅ Vantagens do Mapeamento Manual

1. **Explicitude**: Você vê exatamente o que está sendo mapeado
2. **Controle Total**: Transformações complexas são mais fáceis
3. **Performance**: Sem overhead de reflection
4. **Manutenibilidade**: Mais fácil de entender e modificar
5. **Testabilidade**: Mais fácil de testar mapeamentos específicos
6. **Alinhamento com CQRS**: Cada query pode ter seu próprio DTO otimizado

## 🔄 Estratégia de Migração

### Opção 1: Métodos de Extensão (Recomendado)
```csharp
// Application/Queries/Viagem/Mappers/ViagemDtoMapper.cs
public static class ViagemDtoMapper
{
    public static ViagemDto ToDto(this Viagem viagem)
    {
        return new ViagemDto { /* mapeamento explícito */ };
    }
    
    public static IEnumerable<ViagemDto> ToDto(this IEnumerable<Viagem> viagens)
    {
        return viagens.Select(v => v.ToDto());
    }
}

// Uso no Handler
var viagensDto = viagens.ToDto();
```

### Opção 2: Classes de Mapeamento Específicas
```csharp
// Application/Queries/Viagem/Mappers/ViagemParaListagemMapper.cs
public static class ViagemParaListagemMapper
{
    public static ViagemListagemDto Map(Viagem viagem)
    {
        // DTO otimizado apenas para listagem
    }
}
```

### Opção 3: Mapeamento Inline (Para casos simples)
```csharp
// No próprio Handler, para queries muito específicas
var dto = new ViagemDto
{
    Id = viagem.Id,
    // ... mapeamento direto
};
```

## 📝 Plano de Ação

### Fase 1: Criar Mappers Manuais (✅ Completo)
- [x] Criar `ViagemDtoMapper` com métodos de extensão
- [x] Refatorar `ObterViagensMotoristaQueryHandler`
- [x] Refatorar `ObterHistoricoViagensMotoristaQueryHandler`

### Fase 2: Migrar Outros Handlers de Viagem (✅ Completo)
- [x] `ObterViagensQueryHandler`
- [x] `ObterViagemPorIdQueryHandler`
- [x] `ObterViagensPassageiroQueryHandler`
- [x] `ObterViagensPendentesConfirmacaoQueryHandler`

**Status**: Todos os handlers de queries de Viagem agora usam mapeamento manual via extension methods.

### Fase 3: Outros Handlers (Opcional - Avaliar Necessidade)
Os seguintes handlers ainda usam AutoMapper:
- `QueryPaginadoHandlerBase` (paginação genérica - **RECOMENDADO MANTER** AutoMapper aqui)
- Handlers de Gatilho de Viagem (2 handlers)
- Handlers de Passageiro (3 handlers: Listar, ObterPorId, ObterPaginados)
- Handlers de Veículo (4 handlers: Listar, ObterPorId, ObterPaginados, ModeloVeicular)
- Handlers de Localidade (3 handlers: Listar, ObterPorId, ObterTodos)

**Decisão**: Refatorar apenas se necessário (casos específicos com lógica complexa). Para paginação genérica, manter AutoMapper é aceitável.

### Fase 4: Remover AutoMapper Completamente (Opcional - NÃO RECOMENDADO)
- [ ] Remover dependência do AutoMapper (só se todos os handlers forem migrados)
- [ ] Remover configurações do `Startup.cs`
- [ ] Remover arquivos de Profile do AutoMapper

**Nota**: Não é necessário remover completamente o AutoMapper. Pode ser mantido para casos genéricos como paginação.

## 🎯 Recomendação Final

**Manter AutoMapper apenas para:**
- Casos de mapeamento 1:1 muito simples
- Paginação genérica (QueryPaginadoHandlerBase)
- Migração gradual (não quebrar tudo de uma vez)

**Usar Mapeamento Manual para:**
- Queries específicas de negócio
- DTOs complexos com transformações
- Performance crítica
- Queries que precisam de lógica de mapeamento customizada

## 📚 Referências

- [CQRS Pattern - Martin Fowler](https://martinfowler.com/bliki/CQRS.html)
- [AutoMapper vs Manual Mapping](https://www.jerriepelser.com/blog/automapper-vs-manual-mapping/)
- [Clean Architecture - Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

