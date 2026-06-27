// UserManagement
// Copyright (C) 2026 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using System.Linq.Expressions;
using OroKernel.Domain.Specification;

namespace UserManagement.Specifications;

public sealed class UserByEmailSpecification : BaseSpecification<User>
{
    private readonly string _email;

    public UserByEmailSpecification(string email)
    {
        _email = email ?? throw new ArgumentNullException(nameof(email));
    }

    public override Expression<Func<User, bool>> ToExpression()
    {
        return user => user.Email.Equals(_email, StringComparison.OrdinalIgnoreCase);
    }
}
