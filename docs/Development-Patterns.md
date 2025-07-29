# 📋 **PADRÕES DE DESENVOLVIMENTO - PROJETO RGRTRASPORTE**

## 🎯 **PADRÕES OBRIGATÓRIOS**

### **1. 🏭 Factory Methods para Entidades**

#### **✅ PADRÃO CORRETO**
```csharp
public class MinhaEntidade : AggregateRoot
{
    private MinhaEntidade() { } // Para EF Core
    
    private MinhaEntidade(/* parâmetros */)
    {
        // Validações básicas de integridade
        // Inicialização de propriedades
        // AddDomainEvent se aplicável
    }
    
    // Factory Method principal
    public static MinhaEntidade CriarEntidade(/* parâmetros */)
    {
        return new MinhaEntidade(/* parâmetros */);
    }
    
    // Factory Method com validação
    public static (MinhaEntidade?, bool) CriarEntidadeComValidacao(
        /* parâmetros */,
        INotificationContext notificationContext)
    {
        var validationService = new EntidadeValidationService();
        var valido = validationService.ValidarCriacao(/* params */, notificationContext);
        
        if (!valido)
            return (null, false);
            
        try
        {
            var entidade = CriarEntidade(/* parâmetros */);
            return (entidade, true);
        }
        catch (DomainException ex)
        {
            notificationContext.AddNotification(ex.Message);
            return (null, false);
        }
    }
}
```

#### **❌ EVITAR**
```csharp
// ❌ Construtor público
public MinhaEntidade(/* parâmetros */) { }

// ❌ Instanciação direta
var entidade = new MinhaEntidade();
```

### **2. 📋 Specifications Pattern**

#### **✅ PADRÃO CORRETO**
```csharp
// Specification simples
public class EntidadePodeSerAtivadaSpecification : ISpecification<MinhaEntidade>
{
    public bool IsSatisfiedBy(MinhaEntidade entidade)
    {
        return !entidade.Ativo && entidade.ValidadePara > DateTime.UtcNow;
    }
    
    public string ErrorMessage => "Entidade não pode ser ativada no momento";
}

// NotificationSpecification para coleta de erros
public class EntidadeDadosBasicosSpecification : NotificationSpecification<(string nome, int valor)>
{
    public override bool IsSatisfiedBy((string nome, int valor) dados)
    {
        return !string.IsNullOrEmpty(dados.nome) && dados.valor > 0;
    }
    
    public override string ErrorMessage => "Dados básicos inválidos";
    
    public override void ValidateAndNotify((string nome, int valor) dados, INotificationContext context)
    {
        if (string.IsNullOrEmpty(dados.nome))
            context.AddNotification("O nome é obrigatório");
            
        if (dados.valor <= 0)
            context.AddNotification("O valor deve ser maior que zero");
    }
}
```

### **3. 🔧 Domain Services para Validações**

#### **✅ PADRÃO CORRETO**
```csharp
public class EntidadeValidationService
{
    public bool ValidarCriacao(
        string nome,
        int valor,
        INotificationContext notificationContext)
    {
        var dadosBasicosSpec = new EntidadeDadosBasicosSpecification();
        var dados = (nome, valor);
        
        dadosBasicosSpec.ValidateAndNotify(dados, notificationContext);
        
        return !notificationContext.HasNotifications();
    }
    
    public bool ValidarAtualizacao(
        MinhaEntidade entidade,
        string novoNome,
        INotificationContext notificationContext)
    {
        // Verificar se pode ser editada
        var podeSerEditadaSpec = new EntidadePodeSerEditadaSpecification();
        if (!podeSerEditadaSpec.IsSatisfiedBy(entidade))
        {
            notificationContext.AddNotification(podeSerEditadaSpec.ErrorMessage);
            return false;
        }
        
        // Validar novos dados
        return ValidarCriacao(novoNome, entidade.Valor, notificationContext);
    }
}
```

### **4. 🛡️ BaseValidator para Commands**

#### **✅ PADRÃO CORRETO**
```csharp
public class CriarEntidadeCommandValidator : BaseValidator<CriarEntidadeCommand>
{
    public CriarEntidadeCommandValidator()
    {
        ValidarTextoObrigatorio(RuleFor(x => x.Nome), "nome", 100);
        ValidarIdObrigatorio(RuleFor(x => x.CategoriaId), "categoria");
        ValidarValorPositivo(RuleFor(x => x.Preco), "preço");
        ValidarEmail(RuleFor(x => x.Email));
        
        RuleFor(x => x.DataVencimento)
            .GreaterThan(DateTime.Today)
            .WithMessage("Data de vencimento deve ser futura");
    }
}
```

#### **❌ EVITAR**
```csharp
// ❌ AbstractValidator direto
public class MeuValidator : AbstractValidator<MeuCommand>
{
    public MeuValidator()
    {
        // ❌ Duplicação de lógica de validação
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("O nome é obrigatório")
            .MaximumLength(100)
            .WithMessage("O nome deve ter no máximo 100 caracteres");
    }
}
```

### **5. 📊 Controllers Padronizados**

#### **✅ PADRÃO CORRETO**
```csharp
[Route("api/[controller]")]
[ApiController]
public class EntidadeController : AbstractControllerBase
{
    private readonly IMediator _mediator;

    public EntidadeController(
        IMediator mediator,
        INotificationContext notificationHandler)
        : base(notificationHandler)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(long id)
    {
        var entidade = await _mediator.Send(new ObterEntidadePorIdQuery(id));
        if (entidade == null)
            return await RGRResult(HttpStatusCode.NotFound, $"Entidade com ID {id} não encontrada");

        return await RGRResult(HttpStatusCode.OK, entidade);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarEntidadeCommand command)
    {
        var id = await _mediator.Send(command);
        return await RGRResult(HttpStatusCode.Created, id);
    }
}
```

#### **❌ EVITAR**
```csharp
// ❌ Retornos inconsistentes
public async Task<IActionResult> ObterPorId(long id)
{
    var entidade = await _mediator.Send(new ObterEntidadePorIdQuery(id));
    if (entidade == null)
        return NoContent(); // ❌ Inconsistente
        
    return Ok(entidade); // ❌ Não usa RGRResult
}
```

## 🎯 **PADRÕES DE VALUE OBJECTS**

### **✅ PADRÃO CORRETO**
```csharp
public class MeuValueObject
{
    public string Valor { get; private set; } = string.Empty;
    
    private MeuValueObject() { } // Para EF Core
    
    public MeuValueObject(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new DomainException("Valor é obrigatório");
            
        if (!ValidarFormato(valor))
            throw new DomainException("Formato inválido");
            
        Valor = valor.Trim().ToUpper();
    }
    
    public static bool ValidarFormato(string valor)
    {
        // Lógica de validação
        return !string.IsNullOrWhiteSpace(valor) && valor.Length >= 3;
    }
    
    public override bool Equals(object? obj)
    {
        return obj is MeuValueObject other && Valor == other.Valor;
    }
    
    public override int GetHashCode()
    {
        return Valor.GetHashCode();
    }
    
    public static implicit operator string(MeuValueObject valueObject) => valueObject.Valor;
    public static implicit operator MeuValueObject(string valor) => new(valor);
}
```

## 🔧 **PADRÕES DE REPOSITORY**

### **✅ PADRÃO CORRETO**
```csharp
public class EntidadeRepository : GenericRepository<MinhaEntidade>, IEntidadeRepository
{
    public EntidadeRepository(IUnitOfWorkContext unitOfWork) : base(unitOfWork)
    {
    }
    
    public async Task<MinhaEntidade?> ObterPorIdComIncludes(long id)
    {
        return await Query()
            .Include(e => e.Categoria)
            .Include(e => e.Itens)
                .ThenInclude(i => i.SubItem)
            .FirstOrDefaultAsync(e => e.Id == id);
    }
    
    public async Task<IEnumerable<MinhaEntidade>> ObterPorCategoriaAsync(long categoriaId)
    {
        return await Query()
            .Include(e => e.Categoria)
            .Where(e => e.CategoriaId == categoriaId && e.Ativo)
            .ToListAsync();
    }
}
```

## 📋 **PADRÕES DE MAPEAMENTO**

### **✅ PADRÃO CORRETO**
```csharp
public class EntidadeMapper : Profile
{
    public EntidadeMapper()
    {
        CreateMap<MinhaEntidade, EntidadeDto>()
            // Mapeamento de Value Objects
            .ForMember(dest => dest.CodigoFormatado, opt => opt.MapFrom(src => src.Codigo.Valor))
            .ForMember(dest => dest.DataCriacao, opt => opt.MapFrom(src => src.CreatedAt))
            
            // Mapeamento de propriedades calculadas
            .ForMember(dest => dest.StatusDescricao, opt => opt.MapFrom(src => src.Status.ToString()))
            
            // Ignorar propriedades de navegação complexas
            .ForMember(dest => dest.ItensCount, opt => opt.MapFrom(src => src.Itens.Count));
    }
}
```

## 🎯 **CHECKLIST DE QUALIDADE**

### **Para Novas Entidades:**
- [ ] ✅ Herda de `AggregateRoot` ou `BaseEntity`
- [ ] ✅ Construtor privado + Factory Methods
- [ ] ✅ Value Objects para tipos complexos
- [ ] ✅ Domain Events quando aplicável
- [ ] ✅ Specifications para regras de negócio
- [ ] ✅ Validation Service com NotificationContext
- [ ] ✅ Métodos `ComValidacao` quando aplicável

### **Para Novos Commands:**
- [ ] ✅ Herda de `BaseValidator<T>`
- [ ] ✅ Usa métodos do BaseValidator
- [ ] ✅ Mensagens de erro em português
- [ ] ✅ Validações específicas do domínio

### **Para Novos Controllers:**
- [ ] ✅ Herda de `AbstractControllerBase`
- [ ] ✅ Usa `RGRResult` consistentemente
- [ ] ✅ Mensagens de erro padronizadas
- [ ] ✅ HTTP status codes corretos

### **Para Novos DTOs:**
- [ ] ✅ Tipos corretos (decimal para coordenadas, etc.)
- [ ] ✅ Propriedades `init` quando aplicável
- [ ] ✅ Valores padrão para evitar nullable warnings

## 🚀 **EVOLUÇÃO CONTÍNUA**

### **Quando Adicionar Novos Padrões:**
1. **Documentar** o padrão aqui
2. **Criar exemplos** práticos
3. **Atualizar** entidades existentes gradualmente
4. **Escrever testes** para validar o padrão
5. **Code review** rigoroso

### **Red Flags - Quando Revisar:**
- ❌ Reflection em runtime (exceto casos específicos)
- ❌ Construtores públicos em entidades
- ❌ Validações duplicadas
- ❌ Retornos inconsistentes em controllers
- ❌ DTOs com tipos incorretos
- ❌ Mapeamentos usando `ToString()` inadequadamente

---

## 📚 **RECURSOS ADICIONAIS**

- **Architecture Guide**: `docs/Architecture-Guide.md`
- **Performance Guidelines**: `docs/Performance-Guidelines.md`
- **Testing Standards**: `docs/Testing-Standards.md`

---

**🎯 Seguindo estes padrões, garantimos código consistente, maintível e escalável!** 