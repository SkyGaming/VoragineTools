using System;
using WowTools.Core;
using System.Text;



namespace WoWPacketViewer
{
    public class VoiceChatHandler : Parser
    {
        [Parser(OpCodes.CMSG_VOICE_SESSION_ENABLE)]
        public void HandleVoiceSessionEnable(Parser packet)
        {
            var voiceEnabled = packet.ReadBoolean();
            WriteLine("Voice Enabled: " + voiceEnabled);

            var micEnabled = packet.ReadByte();
            WriteLine("Microphone Enabled. " + micEnabled);
        }

        [Parser(OpCodes.SMSG_VOICE_SESSION_ROSTER_UPDATE)]
        public void HandleVoiceRosterUpdate(Parser packet)
        {
            var unk64 = packet.ReadInt64();
            WriteLine("Unk Int64: " + unk64);

            var chanId = packet.ReadInt16();
            WriteLine("Channel ID: " + chanId);

            var chanName = packet.ReadCString();
            WriteLine("Channel Name: " + chanName);

            var key = Encoding.UTF8.GetString(packet.ReadBytes(16));
            WriteLine("Encryption Key: " + key);

            var ip = packet.ReadInt32();
            WriteLine("Voice Server IP: " + ip);

            var count = packet.ReadByte();
            WriteLine("Player Count: " + count);

            var leaderGuid = packet.ReadGuid();
            WriteLine("Leader GUID: " + leaderGuid);

            var leaderFlags = packet.ReadByte();
            WriteLine("Leader Flags: 0x" + leaderFlags.ToString("X2"));

            var unk = packet.ReadByte();
            WriteLine("Unk Byte 1: " + unk);

            for (var i = 0; i < count - 1; i++)
            {
                var guid = packet.ReadGuid();
                WriteLine("Player GUID: " + guid);

                var idx = packet.ReadByte();
                WriteLine("Index: " + idx);

                var flags = packet.ReadByte();
                WriteLine("Flags: 0x" + flags.ToString("X2"));

                var unk2 = packet.ReadByte();
                WriteLine("Unk Byte 2: " + unk2);
            }
        }

        [Parser(OpCodes.SMSG_VOICE_SESSION_LEAVE)]
        public void HandleVoiceLeave(Parser packet)
        {
            var unk1 = packet.ReadInt64();
            WriteLine("Unk Int64 1: " + unk1);

            var unk2 = packet.ReadInt64();
            WriteLine("Unk Int64 2: " + unk2);
        }

        [Parser(OpCodes.SMSG_VOICE_SET_TALKER_MUTED)]
        public void HandleSetTalkerMuted(Parser packet)
        {
            var guid = packet.ReadGuid();
            WriteLine("GUID: " + guid);

            var unk = packet.ReadByte();
            WriteLine("Unk Byte: " + unk);
        }

        [Parser(OpCodes.SMSG_VOICE_PARENTAL_CONTROLS)]
        public void HandleVoiceParentalControls(Parser packet)
        {
            var disableAll = packet.ReadBoolean();
            WriteLine("Disable All: " + disableAll);

            var disableMic = packet.ReadBoolean();
            WriteLine("Disable Microphone: " + disableMic);
        }

        [Parser(OpCodes.SMSG_AVAILABLE_VOICE_CHANNEL)]
        public void HandleAvailableVoiceChannel(Parser packet)
        {
            var unk = packet.ReadInt64();
            WriteLine("Unk Int64 1: " + unk);

            var type = packet.ReadByte();
            WriteLine("Channel Type: " + type);

            var name = packet.ReadCString();
            WriteLine("Channel Name: " + name);

            var unk2 = packet.ReadInt64();
            WriteLine("Unk Int64 2: " + unk2);
        }

        [Parser(OpCodes.CMSG_SET_ACTIVE_VOICE_CHANNEL)]        
        public void HandleSetActiveVoiceChannel(Parser packet)
        {
            var chanId = packet.ReadInt32();
            WriteLine("Channel ID: " + chanId);

            var name = packet.ReadCString();
            WriteLine("Channel Name: " + name);
        }

        [Parser(OpCodes.CMSG_ADD_VOICE_IGNORE)]
        public void HandleAddVoiceIgnore(Parser packet)
        {
            var name = packet.ReadCString();
            WriteLine("Name: " + name);
        }

        [Parser(OpCodes.CMSG_DEL_VOICE_IGNORE)]
        public void HandleDelVoiceIgnore(Parser packet)
        {
            var guid = packet.ReadGuid();
            WriteLine("GUID: " + guid);
        }

        [Parser(OpCodes.SMSG_VOICE_CHAT_STATUS)]
        public void HandleVoiceStatus(Parser packet)
        {
            var status = packet.ReadByte();
            WriteLine("Status: " + status);
        }
    }
}
