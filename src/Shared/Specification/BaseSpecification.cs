// OroKernel
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroKernel.Shared.Specification;

public abstract class BaseSpecification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>> Criteria => throw new NotImplementedException();

    public List<Expression<Func<T, object>>> Includes => throw new NotImplementedException();

    public List<string> IncludeStrings => throw new NotImplementedException();

    public abstract Expression<Func<T, bool>> ToExpression();

    public bool IsSatisfiedBy(T entity)
    {
        var predicate = ToExpression().Compile();
        return predicate(entity);
    }

    public BaseSpecification<T> And(BaseSpecification<T> other) =>
        new AndBaseSpecification<T>(this, other);

    public BaseSpecification<T> Or(BaseSpecification<T> other) =>
        new OrSpecification<T>(this, other);

    public BaseSpecification<T> Not() =>
        new NotSpecification<T>(this);
}

internal sealed class AndBaseSpecification<T> : BaseSpecification<T>
{
    private readonly BaseSpecification<T> _left;
    private readonly BaseSpecification<T> _right;

    public AndBaseSpecification(BaseSpecification<T> left, BaseSpecification<T> right)
    {
        _left = left;
        _right = right;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpr = _left.ToExpression();
        var rightExpr = _right.ToExpression();
        var param = Expression.Parameter(typeof(T));
        var body = Expression.AndAlso(
            Expression.Invoke(leftExpr, param),
            Expression.Invoke(rightExpr, param));
        return Expression.Lambda<Func<T, bool>>(body, param);
    }
}

internal sealed class OrSpecification<T> : BaseSpecification<T>
{
    private readonly BaseSpecification<T> _left;
    private readonly BaseSpecification<T> _right;

    public OrSpecification(BaseSpecification<T> left, BaseSpecification<T> right)
    {
        _left = left;
        _right = right;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpr = _left.ToExpression();
        var rightExpr = _right.ToExpression();
        var param = Expression.Parameter(typeof(T));
        var body = Expression.OrElse(
            Expression.Invoke(leftExpr, param),
            Expression.Invoke(rightExpr, param));
        return Expression.Lambda<Func<T, bool>>(body, param);
    }
}

internal sealed class NotSpecification<T> : BaseSpecification<T>
{
    private readonly BaseSpecification<T> _spec;

    public NotSpecification(BaseSpecification<T> spec) => _spec = spec;

    public override Expression<Func<T, bool>> ToExpression()
    {
        var expr = _spec.ToExpression();
        var param = Expression.Parameter(typeof(T));
        var body = Expression.Not(Expression.Invoke(expr, param));
        return Expression.Lambda<Func<T, bool>>(body, param);
    }
}
