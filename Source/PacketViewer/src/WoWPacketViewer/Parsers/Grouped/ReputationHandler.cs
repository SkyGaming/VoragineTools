using System;
using WowTools.Core;



namespace WoWPacketViewer
{
    public class ReputationHandler : Parser
    {
        [Parser(OpCodes.SMSG_INITIALIZE_FACTIONS)]
        public  void HandleInitializeFactions(Parser packet)
        {
            var flags = packet.ReadInt32();
            WriteLine("Flags: 0x" + flags.ToString("X8"));

            for (var i = 0; i < 128; i++)
            {
                var sFlags = (FactionFlag)packet.ReadByte();
                WriteLine("Faction Flags " + i + ": " + sFlags);

                var sStand = packet.ReadInt32();
                WriteLine("Faction Standing " + i + ": " + sStand);
            }
        }
    }
}
