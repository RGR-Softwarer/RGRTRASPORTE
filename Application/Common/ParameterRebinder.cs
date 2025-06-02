using System.Linq.Expressions;

namespace Application.Common;

/// <summary>
/// Classe auxiliar para combinar expressões lambda modificando os parâmetros
/// </summary>
public class ParameterRebinder : ExpressionVisitor
{
    private readonly ParameterExpression _parameter;

    public ParameterRebinder(ParameterExpression parameter)
    {
        _parameter = parameter;
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        return _parameter;
    }
} 