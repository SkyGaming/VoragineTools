using System;
using WowTools.Core;

namespace WowPacketParser.Parsing.Parsers
{
    public class PingPong : Parser
    {
        [Parser(OpCodes.CMSG_PING)]
        public void Ping(Parser packet)
        {
            WriteLine("Ping: " + packet.ReadInt32());
            WriteLine("Latency: " + packet.ReadInt32());
        }

        [Parser(OpCodes.SMSG_PONG)]
        public void Pong(Parser packet)
        {
            WriteLine("Ping: " + packet.ReadInt32());
        }
    }
}