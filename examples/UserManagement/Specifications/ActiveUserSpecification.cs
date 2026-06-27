// UserManagement
// Copyright (C) 2026 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using System.Linq.Expressions;
using OroKernel.Domain.Specification;

namespace UserManagement.Specifications;

public sealed class ActiveUserSpecification : BaseSpecification<User>
{
    public override Expression<Func<User, bool>> ToExpression()
    {
        return user => user.IsActive;
    }
}
