using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WowTools.Core;

namespace WoWPacketViewer
{
    public class WowCorePacketReader : IPacketReader
    {
        enum Pkt
        {
            V2_1 = 0x0201,
            V2_2 = 0x0202,
            V3 = 0x0300,
            V3_1 = 0x0301,
        }

        public uint Build { get; private set; }

        public IEnumerable<Packet> ReadPackets(string file)
        {
            using (var gr = new BinaryReader(new FileStream(file, FileMode.Open, FileAccess.Read), Encoding.ASCII))
            {
                gr.ReadBytes(3);                        // PKT
                var version = (Pkt)gr.ReadUInt16();     // sniff version (0x0201, 0x0202)
                int optionalHeaderLength;
                DateTime startTime = DateTime.Now;
                uint startTickCount = 0;
                switch (version)
                {
                    case Pkt.V2_1:
                        Build = gr.ReadUInt16();        // build
                        gr.ReadBytes(40);               // session key
                        break;
                    case Pkt.V2_2:
                        gr.ReadByte();                  // 0x06
                        Build = gr.ReadUInt16();        // build
                        gr.ReadBytes(4);                // client locale
                        gr.ReadBytes(20);               // packet key
                        gr.ReadBytes(64);               // realm name
                        break;
                    case Pkt.V3:
                        gr.ReadByte();                  // snifferId
                        Build = gr.ReadUInt32();        // client build
                        gr.ReadBytes(4);                // client locale
                        gr.ReadBytes(40);               // session key
                        optionalHeaderLength = gr.ReadInt32();
                        gr.ReadBytes(optionalHeaderLength);
                        break;
                    case Pkt.V3_1:
                        gr.ReadByte();                  // snifferId
                        Build = gr.ReadUInt32();        // client build
                        gr.ReadBytes(4);                // client locale
                        gr.ReadBytes(40);               // session key
                        startTime = gr.ReadUInt32().AsUnixTime();
                        startTickCount = gr.ReadUInt32();
                        optionalHeaderLength = gr.ReadInt32();
                        gr.ReadBytes(optionalHeaderLength);
                        break;
                    default:
                        throw new Exception(String.Format("Unknown sniff version {0:X2}", version));
                }

                var packets = new List<Packet>();

                if (version < Pkt.V3)
                {
                    while (gr.PeekChar() >= 0)
                    {
                        Direction direction = gr.ReadByte() == 0xff ? Direction.Server : Direction.Client;
                        uint unixtime = gr.ReadUInt32();
                        uint tickcount = gr.ReadUInt32();
                        uint size = gr.ReadUInt32();
                        var opcode = (direction == Direction.Client) ? (uint)gr.ReadUInt32() : (uint)gr.ReadUInt16();
                        byte[] data = gr.ReadBytes((int)size - ((direction == Direction.Client) ? 4 : 2));

                        packets.Add(new Packet(direction, opcode, data, unixtime, tickcount));
                    }
                }
                else // 3.0/3.1
                {
                    while (gr.PeekChar() >= 0)
                    {
                        Direction direction = gr.ReadUInt32() == 0x47534d53 ? Direction.Server : Direction.Client;
                        uint unixtime = 0;
                        if (version == Pkt.V3)
                            unixtime = gr.ReadUInt32();
                        else  // 3.1
                            gr.ReadUInt32(); // sessionID
                        uint tickcount = gr.ReadUInt32();
                        if(version != Pkt.V3) // 3.1: has to be computed
                            unixtime = startTime.AddMilliseconds(tickcount - startTickCount).ToUnixTime();
                        int optionalSize = gr.ReadInt32();
                        int dataSize = gr.ReadInt32();
                        gr.ReadBytes(optionalSize);
                        var opcode = gr.ReadUInt32();
                        byte[] data = gr.ReadBytes(dataSize - 4);
                        packets.Add(new Packet(direction, opcode, data, unixtime, tickcount));
                    }
                }

                return packets;
            }
        }
    }
}
