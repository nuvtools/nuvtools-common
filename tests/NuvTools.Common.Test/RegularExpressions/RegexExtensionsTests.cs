using NUnit.Framework;
using NuvTools.Common.RegularExpressions;

namespace NuvTools.Common.Tests.RegularExpressions;

[TestFixture]
public class RegexExtensionsTests
{
    #region Match Tests

    [Test]
    public void Match_ShouldReturnValidMatch_WhenPatternMatches()
    {
        // Arrange
        var input = "The price is $100.";
        var pattern = @"\$\d+";

        // Act
        var match = input.Match(pattern);

        // Assert
        Assert.That(match.Success, Is.True);
        Assert.That(match.Value, Is.EqualTo("$100"));
    }

    [Test]
    public void Match_ShouldReturnEmptyMatch_WhenValueIsNull()
    {
        // Arrange
        string? input = null;
        var pattern = @"\d+";

        // Act
        var match = input.Match(pattern);

        // Assert
        Assert.That(match.Success, Is.False);
        Assert.That(match.Value, Is.EqualTo(string.Empty));
    }

    [Test]
    public void Match_ShouldThrowArgumentException_WhenPatternIsNullOrEmpty()
    {
        string input = "abc";

        Assert.Multiple(() =>
        {
            var ex1 = Assert.Throws<ArgumentException>(() => input.Match(null!));
            Assert.That(ex1!.Message, Does.Contain("Regex pattern cannot be null or empty."));

            var ex2 = Assert.Throws<ArgumentException>(() => input.Match(string.Empty));
            Assert.That(ex2!.Message, Does.Contain("Regex pattern cannot be null or empty."));
        });
    }

    #endregion

    #region IsMatch Tests

    [Test]
    public void IsMatch_ShouldReturnTrue_WhenPatternMatches()
    {
        var input = "Email: test@example.com";
        var pattern = @"\b[\w\.-]+@[\w\.-]+\.\w+\b";

        var result = input.IsMatch(pattern);

        Assert.That(result, Is.True);
    }

    [Test]
    public void IsMatch_ShouldReturnFalse_WhenNoMatch()
    {
        var input = "No email here";
        var pattern = @"\b[\w\.-]+@[\w\.-]+\.\w+\b";

        var result = input.IsMatch(pattern);

        Assert.That(result, Is.False);
    }

    [Test]
    public void IsMatch_ShouldReturnFalse_WhenValueIsNullOrEmpty()
    {
        string? input1 = null;
        string input2 = "";

        Assert.Multiple(() =>
        {
            Assert.That(input1.IsMatch(@"\d+"), Is.False);
            Assert.That(input2.IsMatch(@"\d+"), Is.False);
        });
    }

    [Test]
    public void IsMatch_ShouldThrowArgumentException_WhenPatternIsNullOrEmpty()
    {
        var input = "abc";

        Assert.Multiple(() =>
        {
            var ex1 = Assert.Throws<ArgumentException>(() => input.IsMatch(null!));
            Assert.That(ex1!.Message, Does.Contain("Regex pattern cannot be null or empty."));

            var ex2 = Assert.Throws<ArgumentException>(() => input.IsMatch(string.Empty));
            Assert.That(ex2!.Message, Does.Contain("Regex pattern cannot be null or empty."));
        });
    }

    #endregion

    #region Replace Tests

    [Test]
    public void Replace_ShouldReplaceMatchingText()
    {
        var input = "foo bar foo";
        var pattern = "foo";
        var newValue = "baz";

        var result = input.ReplacePattern(pattern, newValue);

        Assert.That(result, Is.EqualTo("baz bar baz"));
    }

    [Test]
    public void Replace_ShouldReturnEmptyString_WhenInputIsNull()
    {
        string? input = null;
        var pattern = "foo";

        
        var result = input.ReplacePattern(pattern, "bar");

        Assert.That(result, Is.Null);
    }

    [Test]
    public void Replace_ShouldTreatNullReplacementAsEmpty()
    {
        var input = "foo bar foo";
        var pattern = "foo";

        var result = input.ReplacePattern(pattern, null!);

        Assert.That(result, Is.EqualTo(" bar "));
    }

    [Test]
    public void Replace_ShouldThrowArgumentException_WhenPatternIsNullOrEmpty()
    {
        var input = "abc";

        Assert.Multiple(() =>
        {
            var ex1 = Assert.Throws<ArgumentException>(() => input.ReplacePattern(null!, "x"));
            Assert.That(ex1!.Message, Does.Contain("Regex pattern cannot be null or empty."));

            var ex2 = Assert.Throws<ArgumentException>(() => input.ReplacePattern(string.Empty, "x"));
            Assert.That(ex2!.Message, Does.Contain("Regex pattern cannot be null or empty."));
        });
    }

    #endregion
}
