using NUnit.Framework;
using NuvTools.Common.ResultWrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NuvTools.Common.Tests.ResultWrapper;

[TestFixture()]
public class ResultTests
{
    [Test()]
    public async Task GetResultOnly()
    {
        var list = new List<MessageDetail>() { new("aa"), new("bb") };

        await Result.ValidationFailAsync(messages: list);
    }

    [Test()]
    public async Task GetResultLong()
    {
        var list = new List<MessageDetail>() { new("aa"), new("bb") };

        await Result<long>.ValidationFailAsync(0L, list);
    }
}