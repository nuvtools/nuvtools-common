using NUnit.Framework;
using NuvTools.Common.ResultWrapper;
using NuvTools.Common.ResultWrapper.Enumerations;

namespace NuvTools.Common.Tests.ResultWrapper;

[TestFixture]
public class ResultTests
{
    #region ValidationFail

    [Test]
    public void ValidationFail_ShouldCreateResultWithMessages()
    {
        // Arrange
        var list = new List<MessageDetail> { new("aa"), new("bb") };

        // Act
        var result = Result.ValidationFail(list);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.ResultType, Is.EqualTo(ResultType.ValidationError));
            Assert.That(result.Messages, Has.Count.EqualTo(2));
            Assert.That(result.Messages[0].Title, Is.EqualTo("aa"));
        });
    }

    [Test]
    public void ValidationFail_ShouldHandleDifferentOverloads()
    {
        // Act
        var result1 = Result.ValidationFail(new MessageDetail("Validation error"));
        var result2 = Result.ValidationFail("Validation error");
        var result3 = Result.ValidationFail(["Validation error"]);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result1.Messages[0].Title, Is.EqualTo("Validation error"));
            Assert.That(result2.Messages[0].Title, Is.EqualTo("Validation error"));
            Assert.That(result3.Messages[0].Title, Is.EqualTo("Validation error"));
        });
    }

    [Test]
    public void ValidationFailTyped_ShouldSupportDataAndMessages()
    {
        // Act
        var result1 = Result<int>.ValidationFail("Validation error");
        var result2 = Result<int>.ValidationFail("Validation error", 10);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result1.Messages[0].Title, Is.EqualTo("Validation error"));
            Assert.That(result1.Data, Is.EqualTo(0));

            Assert.That(result2.Messages[0].Title, Is.EqualTo("Validation error"));
            Assert.That(result2.Data, Is.EqualTo(10));
            Assert.That(result2.ResultType, Is.EqualTo(ResultType.ValidationError));
        });
    }

    #endregion

    #region Fail

    [Test]
    public void Fail_ShouldCreateErrorResult()
    {
        // Act
        var result = Result.Fail("Operation failed");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.ResultType, Is.EqualTo(ResultType.Error));
            Assert.That(result.Messages[0].Title, Is.EqualTo("Operation failed"));
            Assert.That(result.Message, Does.Contain("Operation failed"));
            Assert.That(result.MessageDetail?.Title, Is.EqualTo("Operation failed"));
        });
    }

    [Test]
    public void FailNotFound_ShouldSet404CodeAndContainsNotFound()
    {
        // Act
        var result = Result.FailNotFound("Not found");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Messages[0].Code, Is.EqualTo("404"));
            Assert.That(result.ContainsNotFound, Is.True);
            Assert.That(result.ResultType, Is.EqualTo(ResultType.Error));
        });
    }

    [Test]
    public void FailTyped_ShouldSupportGenericType()
    {
        // Act
        var result = Result<int>.Fail("Generic failure", 99);
        var notfound = Result<int>.FailNotFound("Missing data");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Data, Is.EqualTo(99));
            Assert.That(result.Messages[0].Title, Is.EqualTo("Generic failure"));

            Assert.That(notfound.Messages[0].Code, Is.EqualTo("404"));
            Assert.That(notfound.ContainsNotFound, Is.True);
        });
    }

    #endregion

    #region Success

    [Test]
    public void Success_ShouldCreateSucceededResult()
    {
        // Act
        var result = Result.Success("It works!");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.ResultType, Is.EqualTo(ResultType.Success));
            Assert.That(result.Messages[0].Title, Is.EqualTo("It works!"));
            Assert.That(result.Message, Does.Contain("It works!"));
        });
    }

    [Test]
    public void SuccessTyped_ShouldIncludeDataAndMessage()
    {
        // Act
        var result = Result<int>.Success(1, "Operation completed");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.Data, Is.EqualTo(1));
            Assert.That(result.Messages[0].Title, Is.EqualTo("Operation completed"));
            Assert.That(result.Message, Does.Contain("Operation completed"));
        });
    }

    [Test]
    public void SuccessTyped_ShouldWorkWithoutMessage()
    {
        // Act
        var result = Result<int>.Success(42);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.Data, Is.EqualTo(42));
            Assert.That(result.Messages, Is.Empty);
        });
    }

    #endregion

    #region DerivedProperties

    [Test]
    public void MessageAndMessageDetail_ShouldReturnNull_WhenNoMessages()
    {
        // Act
        var result = Result.Success();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Messages, Is.Empty);
            Assert.That(result.MessageDetail, Is.Null);
            Assert.That(result.Message, Is.Null);
        });
    }

    #endregion
}