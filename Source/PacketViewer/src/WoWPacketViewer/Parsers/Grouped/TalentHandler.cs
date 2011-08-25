using System;
using WowTools.Core;



namespace WoWPacketViewer
{
    public class TalentHandler : Parser
    {
        [Parser(OpCodes.SMSG_TALENTS_INVOLUNTARILY_RESET)]
        public void HandleTalentsInvoluntarilyReset(Parser packet)
        {
            var unk = packet.ReadByte();
            WriteLine("Unk Byte: " + unk);
        }
    }
}
