using NUnit.Framework;
using NuvTools.Common.Exceptions;
using System;

namespace NuvTools.Common.Tests.Exceptions;

[TestFixture()]
public class ExceptionExtensionsTests
{
    [Test()]
    public void InnerExceptionTest()
    {
        try
        {
            throw new InvalidOperationException("Outer exception",
                new ArgumentException("Inner exception",
                    new Exception("Innermost exception")));
        }
        catch (Exception ex)
        {
            var message = ex.AggregateExceptionMessages(1); // Limit to 3 levels
            Assert.That(message.Contains("Innermost exception"));
        }
    }
}