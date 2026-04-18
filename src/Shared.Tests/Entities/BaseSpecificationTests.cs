using System.Linq.Expressions;
using OroKernel.Shared.Entities;

namespace Shared.Tests.Entities;

public class BaseSpecificationTests
{
    private class TestEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    private class TestSpecification : BaseSpecification<TestEntity>
    {
        public TestSpecification(Expression<Func<TestEntity, bool>> criteria) : base(criteria) { }
    }

    [Fact]
    public void Constructor_SetsCriteria()
    {
        // Arrange
        Expression<Func<TestEntity, bool>> criteria = e => e.Id > 0;

        // Act
        var specification = new TestSpecification(criteria);

        // Assert
        Assert.Equal(criteria, specification.Criteria);
    }

    [Fact]
    public void Includes_IsInitializedAsEmptyList()
    {
        // Arrange
        Expression<Func<TestEntity, bool>> criteria = e => e.Id > 0;
        var specification = new TestSpecification(criteria);

        // Act & Assert
        Assert.NotNull(specification.Includes);
        Assert.Empty(specification.Includes);
    }

    [Fact]
    public void IncludeStrings_IsInitializedAsEmptyList()
    {
        // Arrange
        Expression<Func<TestEntity, bool>> criteria = e => e.Id > 0;
        var specification = new TestSpecification(criteria);

        // Act & Assert
        Assert.NotNull(specification.IncludeStrings);
        Assert.Empty(specification.IncludeStrings);
    }

    [Fact]
    public void AddInclude_WithExpression_AddsToIncludesList()
    {
        // Arrange
        Expression<Func<TestEntity, bool>> criteria = e => e.Id > 0;
        var specification = new TestSpecification(criteria);
        Expression<Func<TestEntity, object>> includeExpression = e => e.Name;

        // Act
        specification.GetType().GetMethod("AddInclude",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
            null, [typeof(Expression<Func<TestEntity, object>>)], null)?
            .Invoke(specification, [includeExpression]);

        // Assert
        Assert.Single(specification.Includes);
        Assert.Contains(includeExpression, specification.Includes);
    }

    [Fact]
    public void AddInclude_WithString_AddsToIncludeStringsList()
    {
        // Arrange
        Expression<Func<TestEntity, bool>> criteria = e => e.Id > 0;
        var specification = new TestSpecification(criteria);
        var includeString = "RelatedEntity";

        // Act
        specification.GetType().GetMethod("AddInclude",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
            null, [typeof(string)], null)?
            .Invoke(specification, [includeString]);

        // Assert
        Assert.Single(specification.IncludeStrings);
        Assert.Contains(includeString, specification.IncludeStrings);
    }
}