using System;
using WowTools.Core;

namespace WowPacketParser.Parsing.Parsers
{
    public class WorldStateHandler : Parser
    {
        //[Parser(OpCodes.SMSG_INIT_WORLD_STATES)]
        public void SMSG_INIT_WORLD_STATES(Parser packet)
        {
            var mapId = packet.ReadInt32();
            WriteLine("Map ID: " + mapId);

            var zoneId = packet.ReadInt32();
            WriteLine("Zone ID: " + zoneId);

            var areaId = packet.ReadInt32();
            WriteLine("Area ID: " + areaId);

            var numFields = packet.ReadInt16();
            WriteLine("Field Count: " + numFields);

            for (var i = 0; i < numFields; i++)
                ReadWorldStateBlock(packet);
        }

        public void ReadWorldStateBlock(Parser packet)
        {
            var fieldId = packet.ReadInt32();
            WriteLine("  Field: " + fieldId);

            var fieldVal = packet.ReadInt32();
            WriteLine("  Value: " + fieldVal);
            WriteLine("");
        }

        [Parser(OpCodes.SMSG_UPDATE_WORLD_STATE)]
        public void HandleUpdateWorldState(Parser packet)
        {
            ReadWorldStateBlock(packet);
        }

        [Parser(OpCodes.SMSG_WORLD_STATE_UI_TIMER_UPDATE)]
        public void HandleUpdateUITimer(Parser packet)
        {
            var time = packet.ReadTime();
            WriteLine("Time: " + time);
        }
    }
}
