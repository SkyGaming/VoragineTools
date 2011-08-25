using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using WowTools.Core;

namespace WowTools.Core
{
    public static class Extensions
    {
        public static byte[] ToByteArray(this string data)
        {
            var bytes = new List<byte>();
            for (var i = 0; i < data.Length; i += 2)
                bytes.Add(Byte.Parse(data.Substring(i, 2), NumberStyles.HexNumber));
            return bytes.ToArray();
        }

        public static string ToHexString(this byte[] data)
        {
            var str = String.Empty;
            for (var i = 0; i < data.Length; ++i)
                str += data[i].ToString("X2", CultureInfo.InvariantCulture);
            return str;
        }

        public static string HexLike(this byte[] data, int start, int size)
        {
            var result = String.Empty;
            var counter = start;

            while (counter != size)
            {
                for (var i = 0; i < 0x10; ++i)
                {
                    result += String.Format(i != 0xF ? "{0,-3:X2}" : "{0,-2:X2}", data[counter]);

                    counter++;

                    if (counter == size)
                    {
                        result += Environment.NewLine;
                        break;
                    }
                }
                result += Environment.NewLine;
            }
            return result;
        }

        public static byte[] Decompress(this byte[] data)
        {
            var uncompressedLength = BitConverter.ToUInt32(data, 0);
            var output = new byte[uncompressedLength];

            using (var ms = new MemoryStream(data, 6, data.Length - 6))
            using (var ds = new DeflateStream(ms, CompressionMode.Decompress))
            {
                ds.Read(output, 0, output.Length);
            }
            return output;
        }

        public static string HexLike(this Packet packet)
        {
            var length = packet.Data.Length;
            var dir = (packet.Direction == Direction.Client) ? "C->S" : "S->C";

            var result = new StringBuilder();
            result.AppendFormat("Packet {0}, {1} ({2}), len {3}", dir, packet.Code, packet.OpcodeNumber, length);
            result.AppendLine();

            if (length == 0)
            {
                result.AppendLine("0000: -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- : ................");
            }
            else
            {
                var offset = 0;
                for (var i = 0; i < length; i += 0x10)
                {
                    var bytes = new StringBuilder();
                    var chars = new StringBuilder();

                    for (var j = 0; j < 0x10; ++j)
                    {
                        if (offset < length)
                        {
                            int c = packet.Data[offset];
                            offset++;

                            bytes.AppendFormat("{0,-3:X2}", c);
                            chars.Append((c >= 0x20 && c < 0x80) ? (char)c : '.');
                        }
                        else
                        {
                            bytes.Append("-- ");
                            chars.Append('.');
                        }
                    }

                    result.AppendLine(i.ToString("X4") + ": " + bytes + ": " + chars);
                }
            }

            result.AppendLine();

            return result.ToString();
        }

        static DateTime s_unixEpochReference = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        /// <summary>
        /// Converts the number of seconds passed since UTC Unix Epoch to local time.
        /// </summary>
        /// <param name="unixTimeUtc">
        /// Number of seconds passed since UTC Unix Epoch.
        /// </param>
        /// <returns>
        /// <see cref="System.DateTime"/> converted from UTC Unix Epoch.
        /// </returns>
        public static DateTime AsUnixTime(this uint unixTimeUtc)
        {
            return AsUnixTime((long)unixTimeUtc);
        }

        /// <summary>
        /// Converts the number of seconds passed since UTC Unix Epoch to local time.
        /// </summary>
        /// <param name="unixTimeUtc">
        /// Number of seconds passed since UTC Unix Epoch.
        /// </param>
        /// <returns>
        /// <see cref="System.DateTime"/> converted from UTC Unix Epoch.
        /// </returns>
        public static DateTime AsUnixTime(this long unixTimeUtc)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(s_unixEpochReference.AddSeconds(unixTimeUtc));
        }

        public static DateTime AsGameTime(this int packedDate)
        {
            var minute = packedDate & 0x3F;
            var hour = (packedDate >> 6) & 0x1F;
            var day = (packedDate >> 14) & 0x3F;
            var month = (packedDate >> 20) & 0xF;
            var year = (packedDate >> 24) & 0x1F;

            return new DateTime(year + 2000, month + 1, day + 1, hour, minute, 0);
        }
    }
}
