// OroKernel
// Copyright (C) 2026 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroKernel.Shared.Interfaces;

public interface ISpecification<T>
{
    /// <summary>
    /// Converts the specification to a LINQ expression that can be used for querying
    /// </summary>
    /// <returns>Lambda function</returns>
    Expression<Func<T, bool>> ToExpression();
    bool IsSatisfiedBy(T entity);
}
