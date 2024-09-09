using NUnit.Framework;
using NUnit.Framework.Internal;
using NuvTools.Common.ResultWrapper;
using System.Collections.Generic;

namespace NuvTools.Common.Tests.ResultWrapper;

[TestFixture()]
public class ResultTests
{
    [Test()]
    public void ValidationFail()
    {
        var list = new List<MessageDetail>() { new("aa"), new("bb") };

        Result.ValidationFail(list);

        Result.ValidationFail(new MessageDetail("Validation error"));

        Result.ValidationFail("Validation error");

        var result = Result.ValidationFail(["Validation error"]);
        Assert.That(result.Messages[0].Title, Is.EqualTo("Validation error"));
    }

    [Test()]
    public void ValidationFailTyped()
    {
        Result<int>.ValidationFail("Validation error");
        Result<int>.ValidationFail(new MessageDetail("Validation error"));

        Result<int>.ValidationFail("Validation error", 1);
        Result<int>.ValidationFail(new MessageDetail("Validation error"), 1);

        var resultTyped = Result<int>.ValidationFail(["Validation error"]);
        Assert.That(resultTyped.Messages[0].Title, Is.EqualTo("Validation error"));

        resultTyped = Result<int>.ValidationFail(["Validation error"], 0);
        Assert.That(resultTyped.Messages[0].Title, Is.EqualTo("Validation error"));
    }

    [Test()]
    public void Fail()
    {
        Result.Fail();

        var result = Result.Fail("Not work");

        Assert.That(result.Messages[0].Title, Is.EqualTo("Not work"));

        Result.Fail(new MessageDetail("Not work"));

        Result.Fail(["not work"]);

        Result.Fail([new MessageDetail("Not work")]);

        Assert.That(result.Messages[0].Title, Is.EqualTo("Not work"));

    }

    [Test()]
    public void FailTyped()
    {
        var resultTyped = Result<int>.Fail("Not work");
        Assert.That(resultTyped.Messages[0].Title, Is.EqualTo("Not work"));
    }

    [Test()]
    public void Success()
    {
        Result.Success();

        var result = Result.Success("It works!");
        Assert.That(result.Messages[0].Title, Is.EqualTo("It works!"));

        Result.Success(new MessageDetail("It works!"));
    }

    private static IResult<int> TestSuccessTypedReturn()
    {
        return Result<int>.Success();
    }

    [Test()]
    public void SuccessTyped()
    {
        TestSuccessTypedReturn();

        Result<int>.Success(1);
        Result<int>.Success(1, "It works!");

        var resultTyped = Result<int>.Success(1, new MessageDetail("It works!"));
        Assert.That(resultTyped.Messages[0].Title, Is.EqualTo("It works!"));
        Assert.That(resultTyped.Data, Is.EqualTo(1));
    }
}