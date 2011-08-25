using System;
using WowTools.Core;



namespace WoWPacketViewer
{
    public class ChatHandler : Parser
    {
        [Parser(OpCodes.SMSG_EMOTE)]
        public void HandleEmote(Parser packet)
        {
            UInt32("EmoteID");
            ReadGuid("GUID");
        }

        [Parser(OpCodes.SMSG_MESSAGECHAT)]
        public void HandleServerChatMessage(Parser packet)
        {
            var type = (ChatMessageType)packet.ReadByte();
            WriteLine("Type: " + type);

            var lang = (Language)packet.ReadInt32();
            WriteLine("Language: " + lang);

            var guid = packet.ReadGuid();
            WriteLine("GUID: " + guid);

            var unkInt = packet.ReadInt32();
            WriteLine("Unk Int32: " + unkInt);

            switch (type)
            {
                case ChatMessageType.Say:
                case ChatMessageType.Yell:
                case ChatMessageType.Party:
                case ChatMessageType.PartyLeader:
                case ChatMessageType.Raid:
                case ChatMessageType.RaidLeader:
                case ChatMessageType.RaidWarning:
                case ChatMessageType.Guild:
                case ChatMessageType.Officer:
                case ChatMessageType.Emote:
                case ChatMessageType.TextEmote:
                case ChatMessageType.Whisper:
                case ChatMessageType.WhisperInform:
                case ChatMessageType.System:
                case ChatMessageType.Channel:
                case ChatMessageType.Battleground:
                case ChatMessageType.BattlegroundNeutral:
                case ChatMessageType.BattlegroundAlliance:
                case ChatMessageType.BattlegroundHorde:
                case ChatMessageType.BattlegroundLeader:
                case ChatMessageType.Achievement:
                case ChatMessageType.GuildAchievement:
                {
                    if (type == ChatMessageType.Channel)
                    {
                        var chanName = packet.ReadCString();
                        WriteLine("Channel Name: " + chanName);
                    }

                    var senderGuid = packet.ReadGuid();
                    WriteLine("Sender GUID: " + senderGuid);
                    break;
                }
                case ChatMessageType.MonsterSay:
                case ChatMessageType.MonsterYell:
                case ChatMessageType.MonsterParty:
                case ChatMessageType.MonsterEmote:
                case ChatMessageType.MonsterWhisper:
                case ChatMessageType.RaidBossEmote:
                case ChatMessageType.RaidBossWhisper:
                case ChatMessageType.BattleNet:
                {
                    var nameLen = packet.ReadInt32();
                    WriteLine("Name Length: " + nameLen);

                    var name = packet.ReadCString();
                    WriteLine("Name: " + name);

                    var target = packet.ReadGuid();
                    WriteLine("Receiver GUID: " + guid);

                    if (target.Full != 0)
                    {
                        var tNameLen = packet.ReadInt32();
                        WriteLine("Receiver Name Length: " + tNameLen);

                        var tName = packet.ReadCString();
                        WriteLine("Receiver Name: " + tName);
                    }
                    break;
                }
            }

            var textLen = packet.ReadInt32();
            WriteLine("Text Length: " + textLen);

            var text = packet.ReadCString();
            WriteLine("Text: " + text);

            var chatTag = (ChatTag)packet.ReadByte();
            WriteLine("Chat Tag: " + chatTag);

            if (type != ChatMessageType.Achievement && type != ChatMessageType.GuildAchievement)
                return;

            var achId = packet.ReadInt32();
            WriteLine("Achievement ID: " + achId);
        }

        [Parser(OpCodes.CMSG_MESSAGECHAT_AFK)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_BATTLEGROUND)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_BATTLEGROUND_LEADER)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_CHANNEL)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_DND)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_EMOTE)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_GUILD)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_OFFICER)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_PARTY)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_PARTY_LEADER)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_RAID)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_RAID_LEADER)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_RAID_WARNING)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_SAY)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_WHISPER)]
        [Parser(OpCodes.CMSG_MESSAGECHAT_YELL)]
        public void HandleClientChatMessage(Parser packet)
        {
            var type = (ChatMessageType)packet.ReadInt32();
            WriteLine("Type: " + type);

            var lang = (Language)packet.ReadInt32();
            WriteLine("Language: " + lang);

            switch (type)
            {
                case ChatMessageType.Whisper:
                {
                    var to = packet.ReadCString();
                    WriteLine("Recipient: " + to);
                    goto default;
                }
                case ChatMessageType.Channel:
                {
                    var chan = packet.ReadCString();
                    WriteLine("Channel: " + chan);
                    goto default;
                }
                default:
                {
                    var msg = packet.ReadCString();
                    WriteLine("Message: " + msg);
                    break;
                }
            }
        }
    }
}
