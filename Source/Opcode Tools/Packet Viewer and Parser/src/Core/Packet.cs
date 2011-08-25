using System.IO;

namespace WowTools.Core
{
    public class Packet
    {
        public Direction Direction { get; private set; }

        public uint OpcodeNumber { get; private set; }
        public OpCodes Code { get { return OpcodeDB.GetOpcode(OpcodeNumber); } }

        public byte[] Data { get; private set; }

        public uint UnixTime { get; private set; }

        public uint TicksCount { get; private set; }

        public Packet(Direction direction, uint opcode, byte[] data, uint unixtime, uint tickscount)
        {
            Direction = direction;
            OpcodeNumber = opcode;
            Data = data;
            UnixTime = unixtime;
            TicksCount = tickscount;
        }

        public BinaryReader CreateReader()
        {
            return new BinaryReader(new MemoryStream(Data));
        }

        public void DecompressDataAndSetOpcode(OpCodes opcode)
        {
            Data = Data.Decompress();
            OpcodeNumber = OpcodeDB.GetOpcode(opcode);
        }
    }
}
