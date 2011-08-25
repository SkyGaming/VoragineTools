using System;
using System.Text;
using WowTools.Core;

namespace WoWPacketViewer
{
    public class AccountHandler : Parser
    {
        private const int NUM_ACCOUNT_DATA_TYPES = 8;

        [Parser(OpCodes.SMSG_ACCOUNT_DATA_TIMES)]
        public void AccountDataTimes(Parser packet)
        {
            Time("Time");
            Byte("Unk(byte)");
            
            int mask = packet.ReadInt32("Mask");
            for (int i = 0; i < NUM_ACCOUNT_DATA_TYPES; ++i)
            {
                if ((mask & (1 << i)) != 0)
                    packet.ReadTime(((AccountDataType)i).ToString());
            }
        }

        [Parser(OpCodes.CMSG_REQUEST_ACCOUNT_DATA)]
        public void AccountDataRequest(Parser packet)
        {
            ReadEnum<AccountDataType>("Type");
        }

        [Parser(OpCodes.SMSG_UPDATE_ACCOUNT_DATA)]
        public void AccountUpdateData(Parser packet)
        {
            UTF8Encoding encoder = new System.Text.UTF8Encoding();

            UInt64("GUID: {0:X16}");
            ReadEnum<AccountDataType>("Type");
            Time("Time");

            byte[] compressedData = Reader.ReadBytes((int)Reader.BaseStream.Length - (int)Reader.BaseStream.Position);
            byte[] data = compressedData.Decompress();
            AppendFormatLine("Data: {0}", encoder.GetString(data));
        }

        [Parser(OpCodes.CMSG_UPDATE_ACCOUNT_DATA)]
        public void AccountDataUpdate(Parser packet)
        {
            UTF8Encoding encoder = new System.Text.UTF8Encoding();

            ReadEnum<AccountDataType>("Type");
            Time("Time");

            byte[] compressedData = Reader.ReadBytes((int)Reader.BaseStream.Length - (int)Reader.BaseStream.Position);
            byte[] data = compressedData.Decompress();
            AppendFormatLine("Data: {0}", encoder.GetString(data));
        }

        [Parser(OpCodes.SMSG_UPDATE_ACCOUNT_DATA_COMPLETE)]
        public void HandleUpdateAccountDataComplete(Parser packet)
        {
            ReadEnum<AccountDataType>("Type");
            UInt32("unk");
        }
    }
}
