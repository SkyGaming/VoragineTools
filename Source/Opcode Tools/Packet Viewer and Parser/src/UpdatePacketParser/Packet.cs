using System.IO;
using WowTools.Core;

namespace UpdatePacketParser
{
    public class Packet
    {
        public int Size { get; set; }
        public uint OpcodeNumber;
        public OpCodes Code
        {
            get { return OpcodeDB.GetOpcode(OpcodeNumber); }
        }
        public byte[] Data { get; set; }

        public BinaryReader CreateReader()
        {
            return new BinaryReader(new MemoryStream(Data));
        }
    }
}
