using System;
using System.Text;
using WowTools.Core;

namespace WoWPacketViewer
{
    public class EquipmentSetHandler : Parser
    {
        public static void ReadSetInfo(Parser packet)
        {
            packet.ReadPackedGuid("SetID");
            packet.ReadInt32("Index");
            packet.ReadString("SetName");
            packet.ReadString("SetIcon");

            for (var j = 0; j < 19; j++)
                packet.ReadPackedGuid("Item GUID");
        }

        [Parser(OpCodes.SMSG_EQUIPMENT_SET_LIST)]
        public void HandleEquipmentSetList(Parser packet)
        {
            var count = packet.ReadInt32("Count");
            for (var i = 0; i < count; i++)
                ReadSetInfo(packet);
        }

        [Parser(OpCodes.CMSG_EQUIPMENT_SET_SAVE)]
        public void HandleEquipmentSetSave(Parser packet)
        {
            ReadSetInfo(packet);
        }

        [Parser(OpCodes.SMSG_EQUIPMENT_SET_SAVED)]
        public void HandleReclaimCorpse(Parser packet)
        {
            UInt32("Index");
            ReadPackedGuid("SetID");
        }

        [Parser(OpCodes.CMSG_EQUIPMENT_SET_USE)]
        public void HandleEquipmentSetUse(Parser packet)
        {
            for (var i = 0; i < 19; i++)
            {
                ReadPackedGuid("ItemGUID");
                Byte("SourceBag");
                Byte("SourceSlot");
            }
        }

        [Parser(OpCodes.SMSG_EQUIPMENT_SET_USE_RESULT)]
        public void HandleEquipmentSetUseResult(Parser packet)
        {
            Byte("Result");
        }

        [Parser(OpCodes.CMSG_EQUIPMENT_SET_DELETE)]
        public void HandleEquipmentSetDelete(Parser packet)
        {
            ReadPackedGuid("SetID");
        }
    }
}