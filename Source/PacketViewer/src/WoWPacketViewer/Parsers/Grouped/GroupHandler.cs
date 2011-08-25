using System;
using WowTools.Core;



namespace WoWPacketViewer
{
    public class GroupHandler : Parser
    {
        [Parser(OpCodes.SMSG_GROUP_LIST)]
        public void HandleGroupList(Parser packet)
        {
            var grouptype = (GroupTypeFlag)packet.ReadByte();
            WriteLine("Group Type: " + grouptype);

            var subgroup = packet.ReadByte();
            WriteLine("Sub Group: " + subgroup);

            var flags = (GroupUpdateFlag)packet.ReadByte();
            WriteLine("Flags: " + flags);

            var isbg = packet.ReadBoolean();
            WriteLine("Is Battleground: " + isbg);

            if (grouptype.HasFlag(GroupTypeFlag.LookingForDungeon))
            {
                var dungeonStatus = (InstanceStatus)packet.ReadByte();
                WriteLine("Dungeon Status: " + dungeonStatus);

                var lfgentry = ReadInt32();//packet.ReadLfgEntry();
                WriteLine("LFG Entry: " + lfgentry);
            }

            var unkint2 = packet.ReadInt64();
            WriteLine("Unk Int64: " + unkint2);

            var counter = packet.ReadInt32();
            WriteLine("Counter: " + counter);

            var numFields = packet.ReadInt32();
            WriteLine("Member Count: " + numFields);

            for (var i = 0; i < numFields; i++)
            {
                var name = packet.ReadCString();
                WriteLine("Name " + i + ": " + name);

                var guid = packet.ReadGuid();
                WriteLine("GUID " + i + ": " + guid);

                var status = (GroupMemberStatusFlag)packet.ReadByte();
                WriteLine("Status " + i + ": " + status);

                var subgroup1 = packet.ReadByte();
                WriteLine("Sub Group" + i + ": " + subgroup1);

                var flags1 = (GroupUpdateFlag)packet.ReadByte();
                WriteLine("Update Flags " + i + ": " + flags1);

                var role = (LfgRoleFlag)packet.ReadByte();
                WriteLine("Role " + i + ": " + role);
            }

            var leaderGuid = packet.ReadGuid();
            WriteLine("Leader GUID: " + leaderGuid);

            if (numFields <= 0)
                return;

            var loot = (LootMethod)packet.ReadByte();
            WriteLine("Loot Method: " + loot);

            var looterGuid = packet.ReadGuid();
            WriteLine("Looter GUID: " + looterGuid);

            var item = (ItemQuality)packet.ReadByte();
            WriteLine("Loot Threshold: " + item);

            var dungeonDifficulty = (MapDifficulty)packet.ReadByte();
            WriteLine("Dungeon Difficulty: " + dungeonDifficulty);

            var raidDifficulty = (MapDifficulty)packet.ReadByte();
            WriteLine("Raid Difficulty: " + raidDifficulty);

            var unkbyte3 = packet.ReadByte();
            WriteLine("Unk Byte: " + unkbyte3);
        }

        [Parser(OpCodes.SMSG_GROUP_SET_LEADER)]
        public void HandleGroupSetLeader(Parser packet)
        {
            var name = packet.ReadCString();
            WriteLine("Name: " + name);
        }

        [Parser(OpCodes.CMSG_GROUP_INVITE)]
        public void HandleGroupInvite(Parser packet)
        {
            var name = packet.ReadCString();
            WriteLine("Name: " + name);

            var unkint = packet.ReadInt32();
            WriteLine("Unk Int32: " + unkint);
        }

        [Parser(OpCodes.CMSG_GROUP_ACCEPT)]
        public void HandleGroupAccept(Parser packet)
        {
            var unkint = packet.ReadInt32();
            WriteLine("Unk Int32: " + unkint);
        }
    }
}
