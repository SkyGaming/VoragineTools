using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WowTools.Core;

namespace WoWPacketViewer
{
    public class IzidorPacketReader : IPacketReader
    {
        public uint Build { get; private set; }

        public IEnumerable<Packet> ReadPackets(string file)
        {
            Build = 14333;  // unknown. not stored
            var packets = new List<Packet>();
            using (TextReader tr = new StreamReader(file))
            {
                while (tr.Peek() != -1)
                {
                    string line = tr.ReadLine();
                    string[] data = line.Split('<', '>', '"');
                    uint unixtime = 0; // not stored
                    uint tickcount = UInt32.Parse(data[2]);
                    var direction = data[4] == "StoC" ? Direction.Server : Direction.Client;
                    var opcode = UInt16.Parse(data[6]);
                    string directdata = data[8];
                    byte[] byteData = ParseHex(directdata);
                    packets.Add(new Packet(direction, opcode, byteData, unixtime, tickcount));
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
