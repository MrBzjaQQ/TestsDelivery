using BffPortalService.Application.DTOs.Responses;
using BffPortalService.Application.Exceptions;
using BffPortalService.Domain.Exceptions;
using BffPortalService.Domain.ValueObjects;
using BffPortalService.Application.Specifications.PortalSpecs;
using BffPortalService.Application.Specifications.GroupSpecs;
using FluentAssertions;
using Xunit;

namespace BffPortalService.Tests.Unit;

public class DomainTests
{
    [Fact]
    public void PortalDataKey_StudentProfile_ShouldGenerateCorrectKey()
    {
        var studentId = Guid.NewGuid();

        var key = PortalDataKey.StudentProfile(studentId);

        key.ToString().Should().Be($"student-profile:{studentId}");
    }

    [Fact]
    public void PortalDataKey_AvailableTests_ShouldGenerateCorrectKey()
    {
        var userId = Guid.NewGuid();

        var key = PortalDataKey.AvailableTests(userId);

        key.ToString().Should().Be($"available-tests:{userId}");
    }

    [Fact]
    public void PortalDataKey_GroupAnalytics_ShouldGenerateCorrectKey()
    {
        var groupId = Guid.NewGuid();

        var key = PortalDataKey.GroupAnalytics(groupId);

        key.ToString().Should().Be($"group-analytics:{groupId}");
    }

    [Fact]
    public void PortalDataKey_TestQuestions_ShouldGenerateCorrectKey()
    {
        var testId = Guid.NewGuid();

        var key = PortalDataKey.TestQuestions(testId);

        key.ToString().Should().Be($"test-questions:{testId}");
    }

    [Fact]
    public void CacheDuration_StudentProfile_ShouldBe5Minutes()
    {
        CacheDuration.StudentProfile.Value.Should().Be(TimeSpan.FromMinutes(5));
    }

    [Fact]
    public void CacheDuration_AvailableTests_ShouldBe10Minutes()
    {
        CacheDuration.AvailableTests.Value.Should().Be(TimeSpan.FromMinutes(10));
    }

    [Fact]
    public void CacheDuration_GroupAnalytics_ShouldBe15Minutes()
    {
        CacheDuration.GroupAnalytics.Value.Should().Be(TimeSpan.FromMinutes(15));
    }

    [Fact]
    public void CacheDuration_TestQuestions_ShouldBe1Hour()
    {
        CacheDuration.TestQuestions.Value.Should().Be(TimeSpan.FromHours(1));
    }

    [Fact]
    public void CacheDuration_FromMinutes_ShouldCreateCustomDuration()
    {
        var duration = CacheDuration.FromMinutes(30);

        duration.Value.Should().Be(TimeSpan.FromMinutes(30));
    }
}

public class ExceptionTests
{
    [Fact]
    public void PortalDataNotFoundException_ShouldSetProperties()
    {
        var dataType = "StudentProfile";
        var identifier = "123";

        var exception = new PortalDataNotFoundException(dataType, identifier);

        exception.DataType.Should().Be(dataType);
        exception.Identifier.Should().Be(identifier);
        exception.Message.Should().Contain(dataType);
        exception.Message.Should().Contain(identifier);
    }

    [Fact]
    public void ServiceUnavailableException_ShouldSetProperties()
    {
        var serviceName = "StudentService";

        var exception = new ServiceUnavailableException(serviceName);

        exception.ServiceName.Should().Be(serviceName);
        exception.Message.Should().Contain(serviceName);
    }

    [Fact]
    public void DataAggregationException_ShouldSetProperties()
    {
        var sourceService = "TestService";
        var operation = "SubmitTest";

        var exception = new DataAggregationException(sourceService, operation);

        exception.SourceService.Should().Be(sourceService);
        exception.Operation.Should().Be(operation);
        exception.Message.Should().Contain(sourceService);
        exception.Message.Should().Contain(operation);
    }
}

public class SpecificationTests
{
    [Fact]
    public void UserHasAccessSpecification_WhenUserOwnsResource_ShouldReturnTrue()
    {
        var userId = Guid.NewGuid();
        var spec = new UserHasAccessSpecification(userId, Guid.NewGuid(), "Test");

        var result = spec.IsSatisfiedBy(userId);

        result.Should().BeTrue();
    }

    [Fact]
    public void UserHasAccessSpecification_WhenUserDoesNotOwnResource_ShouldReturnFalse()
    {
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var spec = new UserHasAccessSpecification(userId, Guid.NewGuid(), "Test");

        var result = spec.IsSatisfiedBy(otherUserId);

        result.Should().BeFalse();
    }

    [Fact]
    public void CacheNotExpiredSpecification_WhenNotExpired_ShouldReturnTrue()
    {
        var createdAt = DateTime.UtcNow.AddMinutes(-1);
        var duration = TimeSpan.FromMinutes(5);
        var spec = new CacheNotExpiredSpecification(createdAt, duration);

        var result = spec.IsSatisfiedBy();

        result.Should().BeTrue();
    }

    [Fact]
    public void CacheNotExpiredSpecification_WhenExpired_ShouldReturnFalse()
    {
        var createdAt = DateTime.UtcNow.AddMinutes(-10);
        var duration = TimeSpan.FromMinutes(5);
        var spec = new CacheNotExpiredSpecification(createdAt, duration);

        var result = spec.IsSatisfiedBy();

        result.Should().BeFalse();
    }

    [Fact]
    public void GroupExistsSpecification_WhenGroupExists_ShouldReturnTrue()
    {
        var groupId = Guid.NewGuid();
        var spec = new GroupExistsSpecification(groupId);

        var result = spec.IsSatisfiedBy(true);

        result.Should().BeTrue();
    }

    [Fact]
    public void GroupExistsSpecification_WhenGroupDoesNotExist_ShouldReturnFalse()
    {
        var groupId = Guid.NewGuid();
        var spec = new GroupExistsSpecification(groupId);

        var result = spec.IsSatisfiedBy(false);

        result.Should().BeFalse();
    }

    [Fact]
    public void GroupHasStudentsSpecification_WhenHasEnoughStudents_ShouldReturnTrue()
    {
        var groupId = Guid.NewGuid();
        var spec = new GroupHasStudentsSpecification(groupId, 5);

        var result = spec.IsSatisfiedBy(10);

        result.Should().BeTrue();
    }

    [Fact]
    public void GroupHasStudentsSpecification_WhenNotEnoughStudents_ShouldReturnFalse()
    {
        var groupId = Guid.NewGuid();
        var spec = new GroupHasStudentsSpecification(groupId, 5);

        var result = spec.IsSatisfiedBy(3);

        result.Should().BeFalse();
    }
}

public class DtoTests
{
    [Fact]
    public void StudentProfileDto_ShouldInitializeWithDefaults()
    {
        var dto = new StudentProfileDto();

        dto.FirstName.Should().BeEmpty();
        dto.LastName.Should().BeEmpty();
        dto.Email.Should().BeEmpty();
        dto.RecentTests.Should().BeEmpty();
    }

    [Fact]
    public void AvailableTestsDto_ShouldInitializeWithDefaults()
    {
        var dto = new AvailableTestsDto();

        dto.Tests.Should().BeEmpty();
        dto.TotalCount.Should().Be(0);
    }

    [Fact]
    public void GroupAnalyticsDto_ShouldInitializeWithDefaults()
    {
        var dto = new GroupAnalyticsDto();

        dto.GroupName.Should().BeEmpty();
        dto.TopPerformers.Should().BeEmpty();
    }

    [Fact]
    public void TestQuestionsDto_ShouldInitializeWithDefaults()
    {
        var dto = new TestQuestionsDto();

        dto.TestTitle.Should().BeEmpty();
        dto.TestDescription.Should().BeEmpty();
        dto.Questions.Should().BeEmpty();
    }

    [Fact]
    public void TestResultsDto_ShouldInitializeWithDefaults()
    {
        var dto = new TestResultsDto();

        dto.TestTitle.Should().BeEmpty();
        dto.Answers.Should().BeEmpty();
    }
}
