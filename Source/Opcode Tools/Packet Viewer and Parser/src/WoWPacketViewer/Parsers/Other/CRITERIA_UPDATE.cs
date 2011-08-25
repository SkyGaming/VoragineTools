using System;
using WowTools.Core;

namespace WowPacketParser.Parsing.Parsers
{
    public class CriteriaUpdate : Parser
    {
        [Parser(OpCodes.SMSG_CRITERIA_UPDATE)]
        public void HandleCriteriaUpdate(Parser packet)
        {
            WriteLine("ID: " + packet.ReadInt32());
            WriteLine("Counter: " + packet.ReadPackedGuid());
            WriteLine("PlayerGuid: " + packet.ReadPackedGuid());
            WriteLine("unk: " + packet.ReadInt32());
            WriteLine("Date: " + packet.ReadTime());
            WriteLine("Timer_1: " + packet.ReadInt32());
            WriteLine("Timer_2: " + packet.ReadInt32());
        }
    }
}
