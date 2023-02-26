using NUnit.Framework;
using NuvTools.Common.ResultWrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NuvTools.Common.Tests.ResultWrapper
{
    [TestFixture()]
    public class ResultTests
    {
        [Test()]
        public async Task GetEnumByDescriptionTestAsync()
        {
            var list = new List<string>() { "aa", "bb", "cc" };

            await Result.ValidationFailAsync(messages: list);
        }

        [Test()]
        public async Task GetResultLong()
        {
            var list = new List<string>() { "aa", "bb", "cc" };

            await Result<long>.ValidationFailAsync(messages: list);
        }
    }
}