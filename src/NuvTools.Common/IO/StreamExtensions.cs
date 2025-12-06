namespace NuvTools.Common.IO;

/// <summary>
/// Provides extension methods for <see cref="Stream"/> objects.
/// </summary>
public static class StreamExtensions
{
    /// <summary>
    /// Converts the stream to a byte array synchronously.
    /// </summary>
    /// <param name="value">The stream to convert.</param>
    /// <returns>A byte array containing the stream data.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <remarks>
    /// This method blocks synchronously. For asynchronous operations, use <see cref="ToByteListAsync(Stream)"/>.
    /// </remarks>
    public static byte[] ToByteList(this Stream value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        return value.ToByteListAsync().Result;
    }

    /// <summary>
    /// Converts the stream to a byte array asynchronously.
    /// </summary>
    /// <param name="value">The stream to convert.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a byte array with the stream data.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <remarks>
    /// This method reads the entire stream based on its <see cref="Stream.Length"/> property.
    /// The stream must support seeking and have a known length.
    /// </remarks>
    public static async Task<byte[]> ToByteListAsync(this Stream value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        var byteArray = new byte[value.Length];

        await value.ReadAsync(byteArray.AsMemory(0, (int)value.Length)).ConfigureAwait(false);

        return byteArray;
    }
}