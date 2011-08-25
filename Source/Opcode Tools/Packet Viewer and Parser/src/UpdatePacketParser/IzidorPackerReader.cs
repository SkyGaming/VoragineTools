using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WowTools.Core;

namespace UpdatePacketParser
{
    public class IzidorPacketReader : IPacketReader
    {
        private string FileName;

        public IzidorPacketReader(string filename)
        {
            FileName = filename;
            ushort build = 14333;   // assume the latest build cause the format doesn't contain it
            UpdateFieldsLoader.LoadUpdateFields(build);
        }

        public virtual IEnumerable<Packet> ReadPackets()
        {
            var packets = new List<Packet>();
            using (TextReader tr = new StreamReader(FileName))
            {
                while (tr.Peek() != -1)
                {
                    Packet p = new Packet();
                    string line = tr.ReadLine();
                    string[] data = line.Split('<', '>', '"');
                    p.OpcodeNumber = UInt16.Parse(data[6]);
                    string directdata = data[8];
                    p.Data = ParseHex(directdata);
                    p.Size = p.Data.Length;

                    packets.Add(p);
                }
            }
            return packets;
        }

        public static byte[] ParseHex(string hex)
        {
            int offset = hex.StartsWith("0x") ? 2 : 0;
            if ((hex.Length % 2) != 0)
            {
                throw new ArgumentException("Invalid length: " + hex.Length);
            }
            byte[] ret = new byte[(hex.Length - offset) / 2];

            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = (byte)((ParseNybble(hex[offset]) << 4)
                                 | ParseNybble(hex[offset + 1]));
                offset += 2;
            }
            return ret;
        }

        static int ParseNybble(char c)
        {
            if (c >= '0' && c <= '9')
            {
                return c - '0';
            }
            if (c >= 'A' && c <= 'F')
            {
                return c - 'A' + 10;
            }
            if (c >= 'a' && c <= 'f')
            {
                return c - 'a' + 10;
            }
            throw new ArgumentException("Invalid hex digit: " + c);
        }

    }
}
