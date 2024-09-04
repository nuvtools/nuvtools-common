namespace NuvTools.Common.IO;

public static class StreamExtensions
{
    public static byte[] ToByteList(this Stream value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        return value.ToByteListAsync().Result;
    }

    public static async Task<byte[]> ToByteListAsync(this Stream value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        var byteArray = new byte[value.Length];

        await value.ReadAsync(byteArray.AsMemory(0, (int)value.Length)).ConfigureAwait(false);

        return byteArray;
    }
}