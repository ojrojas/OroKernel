// UserManagement
// Copyright (C) 2026 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using System.Linq.Expressions;
using OroKernel.Shared.Specification;

namespace UserManagement.Specifications;

public sealed class UserNameContainsSpecification : BaseSpecification<User>
{
    private readonly string _substring;

    public UserNameContainsSpecification(string substring)
    {
        _substring = substring ?? throw new ArgumentNullException(nameof(substring));
    }

    public override Expression<Func<User, bool>> ToExpression()
    {
        return user => user.UserName.Contains(_substring, StringComparison.OrdinalIgnoreCase);
    }
}
