using System.IO;
using System.Threading.Tasks;

namespace NuvTools.Common.IO;

public static class StreamExtensions
{
    public static byte[] ToByteList(this Stream value)
    {
        if (value is null) return null;
        return value.ToByteListAsync().Result;
    }

    public static async Task<byte[]> ToByteListAsync(this Stream value)
    {
        if (value is null) return null;

        var byteArray = new byte[value.Length];

        await value.ReadAsync(byteArray, 0, (int)value.Length).ConfigureAwait(false);

        return byteArray;
    }



}