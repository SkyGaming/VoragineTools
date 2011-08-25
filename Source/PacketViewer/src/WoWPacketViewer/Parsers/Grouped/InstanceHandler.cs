using System;
using WowTools.Core;



namespace WoWPacketViewer
{
    public class InstanceHandler : Parser
    {
        [Parser(OpCodes.MSG_SET_DUNGEON_DIFFICULTY)]
        [Parser(OpCodes.MSG_SET_RAID_DIFFICULTY)]
        public void HandleSetDifficulty(Parser packet)
        {
            var difficulty = (MapDifficulty)packet.ReadInt32();
            WriteLine("Difficulty: " + difficulty);

            if (Packet.Direction != Direction.Server)
                return;

            var unkByte = packet.ReadInt32();
            WriteLine("Unk Int32 1: " + unkByte);

            var inGroup = packet.ReadInt32();
            WriteLine("In Group: " + inGroup);
        }

        [Parser(OpCodes.SMSG_INSTANCE_DIFFICULTY)]
        public void HandleInstanceDifficulty(Parser packet)
        {
            var diff = (MapDifficulty)packet.ReadInt32();
            WriteLine("Instance Difficulty: " + diff);

            var unk2 = packet.ReadInt32();
            WriteLine("Player Difficulty: " + unk2);
        }

        [Parser(OpCodes.SMSG_PLAYER_DIFFICULTY_CHANGE)]
        public void HandlePlayerChangeDifficulty(Parser packet)
        {
            var type = (DifficultyChangeType)packet.ReadInt32();
            WriteLine("Change Type: " + type);

            switch (type)
            {
                case DifficultyChangeType.PlayerDifficulty1:
                {
                    var difficulty = packet.ReadByte();
                    WriteLine("Player Difficulty: " + difficulty);
                    break;
                }
                case DifficultyChangeType.SpellDuration:
                {
                    var duration = packet.ReadInt32();
                    WriteLine("Spell Duration: " + duration);
                    break;
                }
                case DifficultyChangeType.Time:
                {
                    var time = packet.ReadInt32();
                    WriteLine("Time: " + time);
                    break;
                }
                case DifficultyChangeType.MapDifficulty:
                {
                    var difficulty = (MapDifficulty)packet.ReadInt32();
                    WriteLine("Map Difficulty: " + difficulty);
                    break;
                }
            }
        }
    }
}
