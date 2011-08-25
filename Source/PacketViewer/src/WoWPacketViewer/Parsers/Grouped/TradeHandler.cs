using System;
using System.Text;
using WowTools.Core;

namespace WoWPacketViewer
{
    public class TradeHandler : Parser
    {
        [Parser(OpCodes.SMSG_TRADE_STATUS)]
        public void HandleTradeStatus(Parser packet)
        {
            UInt8("Unk(uint8)");
            UInt64("Unk(uint64)");
            UInt32("Unk(uint32)");
            UInt32("Status");
            UInt32("Unk(uint32)");
            UInt8("Unk(uint8)");
            UInt32("Unk(uint32)");
            UInt32("Unk(uint32)");
            UInt32("Unk(uint32)");
        }

        [Parser(OpCodes.SMSG_TRADE_STATUS_EXTENDED)]
        public void HandleTradeStatusExtended(Parser packet)
        {
            UInt32("Unk(uint32)");
            UInt32("Unk(uint32)");
            UInt8("Unk(uint8)");
            UInt32("Unk(uint32)");
            UInt32("Unk(uint32)");
            UInt32("Unk(uint32)");
            var count = UInt32("SlotCount");
            UInt64("Money");
            UInt32("Unk(uint32)");

            for (int i = 0; i < count; ++i)
            {
                UInt32("Unk(uint32)");
                UInt64("Unk(uint64)");
                UInt32("Unk(uint32)");
                UInt32("ID");
                UInt32("Unk(uint32)");
                UInt32("Unk(uint32)");
                UInt32("Unk(uint32)");
                UInt8("Unk(uint8)");
                UInt64("Unk(uint64)");
                UInt32("Unk(uint32)");
                UInt8("SlotNumber");
                UInt32("Unk(uint32)");
                UInt32("Unk(uint32)");
                UInt32("Unk(uint32)");
                UInt32("Unk(uint32)");
                UInt32("Unk(uint32)");
                UInt32("Unk(uint32)");
                UInt32("Unk(uint32)");
            }
        }

        [Parser(OpCodes.CMSG_SET_TRADE_ITEM)]
        public void HandleSetTradeItem(Parser packet)
        {
            UInt8("Slot");
            UInt8("Bag");
            UInt8("TradeSlot");
        }
    }
}