using System;
using WowTools.Core;

namespace WoWPacketViewer
{
    public class CalendarHandler : Parser
    {
        [Parser(OpCodes.SMSG_CALENDAR_SEND_CALENDAR)]
        public void HandleSenddCalendar(Parser packet)
        {
            var inviteCount = packet.ReadInt32("InviteCount");
            for (var i = 0; i < inviteCount; ++i)
            {
                WriteLine("  EventID: " + packet.ReadInt64());
                WriteLine("  InviteID: " + packet.ReadInt64());
                WriteLine("  InviteStats: " + packet.ReadInt8());
                WriteLine("  Mod_Type: " + packet.ReadInt8());
                WriteLine("  Invite_Type: " + packet.ReadInt8());
                WriteLine("  InvitedBy: " + packet.ReadPackedGuid());
                WriteLine("");
            }

            var EventCount = packet.ReadInt32("EventCount");
            for (var i = 0; i < EventCount; ++i)
            {
                WriteLine("  EventID: " + packet.ReadInt64());
                WriteLine("  EventName: " + packet.ReadString());
                WriteLine("  EventModFlags: " + packet.ReadInt32());
                WriteLine("  EventDate: " + packet.ReadPackedTime());
                WriteLine("  EventFlags: " + packet.ReadInt32());
                WriteLine("  DungeonID: " + packet.ReadInt32());
                WriteLine("  unk: " + packet.ReadInt64());
                WriteLine("  InvitedBy: " + packet.ReadPackedGuid());
                WriteLine("");
            }

            WriteLine("CurrentUnixTime: " + packet.ReadTime());
            WriteLine("CurrentPacketTime: " + packet.ReadPackedTime());

            var InstanceResetCount = packet.ReadInt32("InstanceResetCount");
            for (var i = 0; i < InstanceResetCount; ++i)
            {
                WriteLine("  MapID: " + packet.ReadInt32());
                WriteLine("  Difficulty: " + packet.ReadInt32());
                WriteLine("  ResetTime: " + packet.ReadTime());
                WriteLine("  RaidID: " + packet.ReadInt64());
                WriteLine("");
            }

            WriteLine("BaseTime: " + packet.ReadTime());

            var RaidResetCount = packet.ReadInt32("RaidResetCount");
            for (var i = 0; i < RaidResetCount; ++i)
            {
                WriteLine("  MapID: " + packet.ReadInt32());
                WriteLine("  ResetTime: " + packet.ReadTime());
                WriteLine("  NegativeOffset: " + packet.ReadInt32());
                WriteLine("");
            }

            var Counter = packet.ReadInt32();
            WriteLine("Counter: " + Counter + "(Never seen this larger than 0)");
        }
     
        [Parser(OpCodes.SMSG_CALENDAR_COMMAND_RESULT)]
        public void HandleCommandCalendar(Parser packet)
        {
            WriteLine("unk: " + packet.ReadInt32() + "(unused)");
            WriteLine("unk: " + packet.ReadInt8() + "(unused)");
            WriteLine("unk: " + packet.ReadString());
            packet.ReadEnum<CalendarResponseResult>("Result");
        }

        [Parser(OpCodes.CMSG_CALENDAR_GET_EVENT)]
        public void HandleGetEvent(Parser packet)
        {
            WriteLine("EventID: " + packet.ReadInt32());

            if (packet.GetSize() == 8)
                WriteLine("unk: " + packet.ReadInt32());
        }

        [Parser(OpCodes.CMSG_CALENDAR_ADD_EVENT)]
        public void HandleAddEvent(Parser packet)
        {
            WriteLine("Name: " + packet.ReadString());
            WriteLine("Description: " + packet.ReadString());
            WriteLine("Type: " + packet.ReadInt8());
            WriteLine("Repeat_Option: " + packet.ReadInt8());
            WriteLine("maxSize: " + packet.ReadInt32());
            WriteLine("dungeonID: " + packet.ReadInt32());
            WriteLine("time: " + packet.ReadPackedTime());
            WriteLine("lockoutTime: " + packet.ReadInt32());
            WriteLine("flags: " + packet.ReadInt32());

            var inviteCount = packet.ReadInt32("inviteCount");
            WriteLine("");
            WriteLine("Invited Players");

            for (var i = 0; i < inviteCount; ++i)
            {
                WriteLine(" PlayerGuid: " + packet.ReadPackedGuid());
                WriteLine(" inviteStatus: " + packet.ReadInt8());
                WriteLine(" modType: " + packet.ReadInt8());
                WriteLine("");
            }
        }

        [Parser(OpCodes.SMSG_CALENDAR_EVENT_INVITE_ALERT)]
        public void HandleEventInviteAlert(Parser packet)
        {
            WriteLine("EventID: " + packet.ReadInt64());
            WriteLine("EventName: " + packet.ReadString());
            WriteLine("EventTime: " + packet.ReadTime());
            WriteLine("EventFlags: " + packet.ReadInt32());
            WriteLine("EventType: " + packet.ReadInt32());
            WriteLine("DungeonID: " + packet.ReadInt32());
            WriteLine("unk: " + packet.ReadInt32());
            WriteLine("InviteID: " + packet.ReadInt64());
            WriteLine("InviteStatus: " + packet.ReadInt8());
            WriteLine("Mod_Type: " + packet.ReadInt8());
            WriteLine("unk: " + packet.ReadInt32());
            WriteLine("Inviter_1: " + packet.ReadPackedGuid());
            WriteLine("Inviter_2: " + packet.ReadPackedGuid());
        }

        [Parser(OpCodes.SMSG_CALENDAR_SEND_EVENT)]
        public void HandleSendEvent(Parser packet)
        {
            WriteLine("Invite_Type: " + packet.ReadInt8());
            WriteLine("Creator " + packet.ReadPackedGuid());
            WriteLine("EventID: " + packet.ReadInt32());
            WriteLine("unk: " + packet.ReadInt32());
            WriteLine("EventName: " + packet.ReadString());
            WriteLine("EventDescription: " + packet.ReadString());
            WriteLine("Event_Type: " + packet.ReadInt8());
            WriteLine("Repeat_Option: " + packet.ReadInt8());
            WriteLine("MaxSize: " + packet.ReadInt32());
            WriteLine("DungeonID: " + packet.ReadInt32());
            WriteLine("EventFlags: " + packet.ReadInt32());
            WriteLine("EventTime: " + packet.ReadPackedTime());
            WriteLine("LockOutTime: " + packet.ReadInt32());
            WriteLine("unk: " + packet.ReadInt32());
            WriteLine("unk: " + packet.ReadInt32());
            
            var inviteCount = packet.ReadInt32("InviteCount");
            for (var i = 0; i < inviteCount; ++i)
            {
                WriteLine("  PlayerGuid: " + packet.ReadPackedGuid());
                WriteLine("  PlayerLevel: " + packet.ReadInt8());
                WriteLine("  InviteStatus: " + packet.ReadInt8());
                WriteLine("  Mod_Type: " + packet.ReadInt8());
                WriteLine("  unk: " + packet.ReadInt8());
                WriteLine("  inviteID: " + packet.ReadInt64());
                WriteLine("  unk: " + packet.ReadInt8());
                WriteLine("  unk: " + packet.ReadInt32());
                WriteLine("");
            }
        }

        [Parser(OpCodes.CMSG_CALENDAR_EVENT_REMOVE_INVITE)]
        public void HandleRemove_Invite(Parser packet)
        {
            WriteLine("Removee'sGuid: " + packet.ReadPackedGuid());
            WriteLine("Removee'sInviteID: " + packet.ReadInt64());
            WriteLine("unk: " + packet.ReadInt64());
            WriteLine("EventID: " + packet.ReadInt64());
        }
    }
}