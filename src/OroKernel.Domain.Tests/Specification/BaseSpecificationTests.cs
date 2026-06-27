using System.Linq.Expressions;
using OroKernel.Domain.Specification;

namespace OroKernel.Domain.Tests.Specification;

public class BaseSpecificationTests
{
    private class TestEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    private sealed class ActiveEntitySpec : BaseSpecification<TestEntity>
    {
        public override Expression<Func<TestEntity, bool>> ToExpression()
        {
            return e => e.IsActive;
        }
    }

    private sealed class IdGreaterThanSpec(int minId) : BaseSpecification<TestEntity>
    {
        public override Expression<Func<TestEntity, bool>> ToExpression()
        {
            return e => e.Id > minId;
        }
    }

    [Fact]
    public void IsSatisfiedBy_ReturnsTrue_WhenConditionMet()
    {
        var spec = new ActiveEntitySpec();
        var entity = new TestEntity { Id = 1, IsActive = true };

        var result = spec.IsSatisfiedBy(entity);

        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_ReturnsFalse_WhenConditionNotMet()
    {
        var spec = new ActiveEntitySpec();
        var entity = new TestEntity { Id = 1, IsActive = false };

        var result = spec.IsSatisfiedBy(entity);

        Assert.False(result);
    }

    [Fact]
    public void And_CombinesTwoSpecifications()
    {
        var activeSpec = new ActiveEntitySpec();
        var idSpec = new IdGreaterThanSpec(0);
        var entity = new TestEntity { Id = 1, IsActive = true };

        var combined = activeSpec.And(idSpec);
        var result = combined.IsSatisfiedBy(entity);

        Assert.True(result);
    }

    [Fact]
    public void Or_CombinesTwoSpecifications()
    {
        var activeSpec = new ActiveEntitySpec();
        var idSpec = new IdGreaterThanSpec(100);
        var entity = new TestEntity { Id = 1, IsActive = true };

        var combined = activeSpec.Or(idSpec);
        var result = combined.IsSatisfiedBy(entity);

        Assert.True(result);
    }

    [Fact]
    public void Not_NegatesSpecification()
    {
        var spec = new ActiveEntitySpec();
        var entity = new TestEntity { Id = 1, IsActive = true };

        var negated = spec.Not();
        var result = negated.IsSatisfiedBy(entity);

        Assert.False(result);
    }
}
