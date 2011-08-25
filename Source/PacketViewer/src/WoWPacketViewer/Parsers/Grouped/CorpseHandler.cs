using System;
using System.Text;
using WowTools.Core;

namespace WoWPacketViewer
{
    public class CorpseHandler : Parser
    {
        [Parser(OpCodes.CMSG_CORPSE_MAP_POSITION_QUERY)]
        public void HandleCorpseMapPositionQuery(Parser packet)
        {
            UInt32("LowGUID");
        }

        [Parser(OpCodes.SMSG_CORPSE_RECLAIM_DELAY)]
        public void HandleCorpseReclaimDelay(Parser packet)
        {
            UInt32("Delay");
        }

        [Parser(OpCodes.CMSG_RECLAIM_CORPSE)]
        public void HandleReclaimCorpse(Parser packet)
        {
            ReadGuid("CorpseGUID");
        }

        [Parser(OpCodes.SMSG_CORPSE_MAP_POSITION_QUERY_RESPONSE)]
        public void HandleCorpseMapPositionResponse(Parser packet)
        {
            var curr = Reader.ReadCoords3();
            WriteLine("Corpse Position: {0}", curr);

            ReadSingle("Unk(Single)");
        }

        [Parser(OpCodes.MSG_CORPSE_QUERY)]
        public void HandleCorpseQuery(Parser packet)
        {
            if (Packet.Direction != Direction.Server)
            {
                WriteLine("MSG Packet that wants response from server");
                return;
            }

            var found = UInt8("Found");
            if (found == 0) return;
            UInt32("MapID");
            ReadCoords3("Corpse Position");
            UInt32("CorpseMapID");
            UInt32("CorpseLowGuid");
        }
    }
}