// Code based on Steve Gordon's article:
// https://www.stevejgordon.co.uk/using-high-performance-dotnetcore-csharp-techniques-to-base64-encode-a-guid


using System;
using System.Buffers.Text;
using System.Runtime.InteropServices;

namespace ConsoleApp
{
    public static class GuidExtensions
    {
        private const byte ForwardSlashByte = (byte)'/';
        private const byte DashByte = (byte)'-';
        private const byte PlusByte = (byte)'+';
        private const byte UnderscoreByte = (byte)'_';
        private const char Underscore = '_';
        private const char Dash = '-';

        public static string EncodeBase64StringMR(this Guid guid)
        {
            Span<byte> guidBytes = stackalloc byte[16];
            Span<byte> encodedBytes = stackalloc byte[24];

            MemoryMarshal.TryWrite(guidBytes, ref guid); // write bytes from the Guid
            Base64.EncodeToUtf8(guidBytes, encodedBytes, out _, out _);

            Span<char> chars = stackalloc char[22];

            // replace any characters which are not URL safe
            // skip the final two bytes as these will be '==' padding we don't need
            for (var i = 0; i < 22; i++)
            {
                switch (encodedBytes[i])
                {
                    case ForwardSlashByte:
                        chars[i] = Dash;
                        break;

                    case PlusByte:
                        chars[i] = Underscore;
                        break;

                    default:
                        chars[i] = (char)encodedBytes[i];
                        break;
                }
            }

            var final = chars.ToString();

            return final;
        }

        public static string EncodeBase64StringR(this Guid guid)
        {
            var aa = new byte[10];
            Span<char> chars = stackalloc char[23];
            Span<byte> bytes = MemoryMarshal.Cast<char, byte>(chars);

            MemoryMarshal.TryWrite(bytes.Slice(0,20), ref guid); // write bytes from the Guid
            Base64.EncodeToUtf8(bytes.Slice(0, 20), bytes.Slice(22, 24), out _, out _);

            // replace any characters which are not URL safe
            // skip the final two bytes as these will be '==' padding we don't need
            for (var i = 0; i < 22; i++)
            {
                byte value = bytes[i + 22];
                switch (value)
                {
                    case ForwardSlashByte:
                        chars[i] = Dash;
                        break;

                    case PlusByte:
                        chars[i] = Underscore;
                        break;

                    default:
                        chars[i] = (char)value;
                        break;
                }
            }

            var final = chars.Slice(0,22).ToString();

            return final;
        }
    }
}
