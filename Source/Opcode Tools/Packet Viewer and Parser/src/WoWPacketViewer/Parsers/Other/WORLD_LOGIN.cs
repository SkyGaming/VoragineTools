using System;
using WowTools.Core;

namespace WowPacketParser.Parsing.Parsers
{
    public class WorldLogin : Parser
    {
        [Parser(OpCodes.CMSG_WORLD_LOGIN)]
        public void HanleWorldLogin(Parser packet)
        {
            WriteLine("unk: " + packet.ReadInt32());
            WriteLine("unk: " + packet.ReadInt8());
        }
    }
}