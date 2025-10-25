using System.Net;
using System.Text;
using System.Text.Json;
using NuvTools.Common.ResultWrapper;
using NUnit.Framework;

namespace NuvTools.Common.Tests.ResultWrapper;

[TestFixture]
public class ResultExtensionsTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    #region Helper methods

    private static HttpResponseMessage CreateResponse(HttpStatusCode statusCode, object? content = null)
    {
        var response = new HttpResponseMessage(statusCode);

        if (content != null)
        {
            var json = JsonSerializer.Serialize(content, JsonOptions);
            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        return response;
    }

    private static HttpResponseMessage CreateRawResponse(HttpStatusCode statusCode, string? body)
    {
        var response = new HttpResponseMessage(statusCode);
        if (body != null)
            response.Content = new StringContent(body, Encoding.UTF8, "text/plain");

        return response;
    }

    #endregion

    #region DTO for tests

    private record PersonDto(int Id, string Name, string Email);

    #endregion

    [Test]
    public async Task ToResultAsyncT_ShouldReturnSuccess_WhenValidJsonResultWithString()
    {
        var expected = Result<string>.Success("data", "Success!");
        var response = CreateResponse(HttpStatusCode.OK, expected);

        var result = await response.ToResultAsync<string>();

        Assert.That(result.Succeeded, Is.True);
        Assert.That(result.Data, Is.EqualTo("data"));
        Assert.That(result.Message, Is.EqualTo("Success!"));
    }

    [Test]
    public async Task ToResultAsyncT_ShouldReturnSuccess_WhenValidJsonResultWithObject()
    {
        var person = new PersonDto(1, "Bruno", "bruno@example.com");
        var expected = Result<PersonDto>.Success(person, "User loaded successfully");
        var response = CreateResponse(HttpStatusCode.OK, expected);

        var result = await response.ToResultAsync<PersonDto>();

        Assert.That(result.Succeeded, Is.True);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data!.Id, Is.EqualTo(1));
        Assert.That(result.Data!.Name, Is.EqualTo("Bruno"));
        Assert.That(result.Data!.Email, Is.EqualTo("bruno@example.com"));
        Assert.That(result.Message, Does.Contain("User loaded"));
    }

    [Test]
    public async Task ToResultAsyncT_ShouldReturnFail_When404WithResult()
    {
        var expected = Result<string>.FailNotFound("Not found");
        var response = CreateResponse(HttpStatusCode.NotFound, expected);

        var result = await response.ToResultAsync<string>();

        Assert.That(result.Succeeded, Is.False);
        Assert.That(result.ContainsNotFound, Is.True);
        Assert.That(result.Message, Does.Contain("Not found"));
    }

    [Test]
    public async Task ToResultAsyncT_ShouldReturnFail_WhenEmptyBody()
    {
        var response = CreateRawResponse(HttpStatusCode.NotFound, string.Empty);

        var result = await response.ToResultAsync<PersonDto>();

        Assert.That(result.Succeeded, Is.False);
        Assert.That(result.MessageDetail!.Code, Is.EqualTo("404"));
        Assert.That(result.Message, Does.Contain("Empty response body"));
    }

    [Test]
    public async Task ToResultAsyncT_ShouldReturnFail_WhenInvalidJson()
    {
        var response = CreateRawResponse(HttpStatusCode.InternalServerError, "<<<invalid>>>");

        var result = await response.ToResultAsync<PersonDto>();

        Assert.That(result.Succeeded, Is.False);
        Assert.That(result.MessageDetail!.Code, Is.EqualTo("500"));
        Assert.That(result.Message, Does.Contain("Unexpected response"));
    }

    [Test]
    public async Task ToResultAsyncT_ShouldReturnFail_WhenExceptionDuringRead()
    {
        using var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new ThrowingContent()
        };

        var result = await response.ToResultAsync<PersonDto>();

        Assert.That(result.Succeeded, Is.False);
        Assert.That(result.Message, Does.Contain("Failed to read response body"));
    }

    [Test]
    public async Task ToResultAsync_ShouldReturnSuccess_WhenValidJsonResult()
    {
        var expected = Result.Success("It worked!");
        var response = CreateResponse(HttpStatusCode.OK, expected);

        var result = await response.ToResultAsync();

        Assert.That(result.Succeeded, Is.True);
        Assert.That(result.Message, Does.Contain("It worked!"));
    }

    [Test]
    public async Task ToResultAsync_ShouldReturnFail_WhenInvalidJson()
    {
        var response = CreateRawResponse(HttpStatusCode.BadRequest, "{invalid-json}");

        var result = await response.ToResultAsync();

        Assert.That(result.Succeeded, Is.False);
        Assert.That(result.MessageDetail!.Code, Is.EqualTo("400"));
        Assert.That(result.Message, Does.Contain("Unexpected response"));
    }

    #region Support classes

    /// <summary>
    /// Simula uma falha de leitura de stream no HttpContent.
    /// </summary>
    private sealed class ThrowingContent : HttpContent
    {
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context)
            => throw new InvalidOperationException("Simulated read error");

        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return true;
        }
    }

    #endregion
}