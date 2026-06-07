// UserManagement
// Copyright (C) 2026 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using UserManagement.Specifications;

namespace UserManagement;

/// Demonstrates usage of BaseSpecification<T> combinators (.And, .Or, .Not)
public static class SpecificationDemo
{
    public static void Run(List<User> users)
    {
        Console.WriteLine("=== Specification Combinator Demo ===");
        Console.WriteLine();

        var activeSpec = new ActiveUserSpecification();
        var emailSpec = new UserByEmailSpecification("john.doe@example.com");
        var nameSpec = new UserNameContainsSpecification("john");

        // Single specification
        var activeUsers = users.Where(u => activeSpec.IsSatisfiedBy(u)).ToList();
        Console.WriteLine($"ActiveUserSpecification: {activeUsers.Count} active user(s)");

        // AND combinator
        var andSpec = activeSpec.And(emailSpec);
        var activeWithEmail = users.Where(u => andSpec.IsSatisfiedBy(u)).ToList();
        Console.WriteLine($"ActiveUserSpecification AND UserByEmailSpecification: {activeWithEmail.Count} user(s)");

        // OR combinator
        var orSpec = activeSpec.Or(nameSpec);
        var activeOrNameMatch = users.Where(u => orSpec.IsSatisfiedBy(u)).ToList();
        Console.WriteLine($"ActiveUserSpecification OR UserNameContainsSpecification('john'): {activeOrNameMatch.Count} user(s)");

        // NOT combinator
        var notSpec = activeSpec.Not();
        var inactiveUsers = users.Where(u => notSpec.IsSatisfiedBy(u)).ToList();
        Console.WriteLine($"NOT ActiveUserSpecification: {inactiveUsers.Count} inactive user(s)");

        // All three combined: active AND (email match OR name contains)
        var complexSpec = activeSpec.And(emailSpec.Or(nameSpec));
        var complexResults = users.Where(u => complexSpec.IsSatisfiedBy(u)).ToList();
        Console.WriteLine($"Active AND (Email OR Name): {complexResults.Count} user(s)");

        Console.WriteLine();
        Console.WriteLine("=== End of Specification Demo ===");
        Console.WriteLine();
    }
}
