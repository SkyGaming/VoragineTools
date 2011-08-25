using System;
using System.Text;
using WowTools.Core;

namespace WoWPacketViewer
{
    public class MiscellaneousHandler : Parser
    {
        [Parser(OpCodes.CMSG_PING)]
        public void Ping(Parser packet)
        {
            UInt32("Latency");
            UInt32("Ping");
        }

        [Parser(OpCodes.SMSG_PONG)]
        public void Pong(Parser packet)
        {
            UInt32("Ping");
        }

        [Parser(OpCodes.CMSG_REALM_SPLIT)]
        public void HandleClientRealmSplit(Parser packet)
        {
            UInt32("Unk");
        }

        [Parser(OpCodes.SMSG_REALM_SPLIT)]
        public void HandleServerRealmSplit(Parser packet)
        {
            UInt32("Unk");
            ReadEnum<RealmSplitState>("SplitState");
            CString("Unk");
        }

        [Parser(OpCodes.SMSG_CLIENTCACHE_VERSION)]
        public void HandleClientCacheVersion(Parser packet)
        {
            UInt32("Version");
        }

        [Parser(OpCodes.SMSG_TIME_SYNC_REQ)]
        public void HandleTimeSyncReq(Parser packet)
        {
            UInt32("Count");
        }

        [Parser(OpCodes.SMSG_LEARNED_DANCE_MOVES)]
        public void HandleLearnedDanceMoves(Parser packet)
        {
            UInt64("DanceMoveID");
        }

        [Parser(OpCodes.SMSG_TRIGGER_CINEMATIC)]
        [Parser(OpCodes.SMSG_TRIGGER_MOVIE)]
        public void HandleTriggerSequence(Parser packet)
        {
            UInt32("SequenceID");
        }

        [Parser(OpCodes.SMSG_PLAY_SOUND)]
        [Parser(OpCodes.SMSG_PLAY_MUSIC)]
        [Parser(OpCodes.SMSG_PLAY_OBJECT_SOUND)]
        public void HandleSoundMessages(Parser packet)
        {
            UInt32("Sound ID");

            if (packet.GetSize() > 4)
                ReadGuid("Guid");
        }

        [Parser(OpCodes.SMSG_WEATHER)]
        public void HandleWeatherStatus(Parser packet)
        {
            ReadEnum<WeatherState>("State");
            ReadSingle("Grade");
            Byte("Unk");
        }

        [Parser(OpCodes.CMSG_TUTORIAL_FLAG)]
        public void HandleTutorialFlag(Parser packet)
        {
            var flag = packet.ReadInt32();
            WriteLine("Flag: 0x" + flag.ToString("X8"));
        }

        [Parser(OpCodes.SMSG_TUTORIAL_FLAGS)]
        public void HandleTutorialFlags(Parser packet)
        {
            For(8, i => ReadUInt32("Mask {0}: 0x{1:X8}", i));
        }

        [Parser(OpCodes.CMSG_AREATRIGGER)]
        public void HandleClientAreaTrigger(Parser packet)
        {
            UInt32("AreaTriggerID");
        }

        [Parser(OpCodes.SMSG_PRE_RESURRECT)]
        public void HandlePreRessurect(Parser packet)
        {
            ReadPackedGuid("GUID");
        }

        [Parser(OpCodes.CMSG_SET_ALLOW_LOW_LEVEL_RAID1)]
        [Parser(OpCodes.CMSG_SET_ALLOW_LOW_LEVEL_RAID2)]
        public void HandleLowLevelRaidPackets(Parser packet)
        {
            UInt8("Allow");
        }

        [Parser(OpCodes.SMSG_SET_FACTION_STANDING)]
        public void HandleSetFactionStanding(Parser packet)
        {
            ReadSingle("unk(Float)");
            Byte("unk(unit8)");

            var amount = packet.ReadInt32();
            WriteLine("Count: " + amount);

            for (int i = 0; i < amount; i++)
            {
                UInt32("FactionListID");
                UInt32("Standing");
            }
        }

        [Parser(OpCodes.SMSG_SET_PROFICIENCY)]
        public void HandleSetProficiency(Parser packet)
        {
            var itemclass = (ItemClass)packet.ReadByte();
            WriteLine("ItemClass: " + itemclass);

            UInt32("ItemSubClassMask");
        }

        [Parser(OpCodes.SMSG_BINDPOINTUPDATE)]
        public void HandleSetBindPointUpdate(Parser packet)
        {
            var curr = Reader.ReadCoords3();
            WriteLine("Current Position: {0}", curr);

            UInt32("MapID");
            UInt32("ZoneID");
        }

        [Parser(OpCodes.SMSG_FEATURE_SYSTEM_STATUS)]
        public void HandleFeatureSystemStatus(Parser packet)
        {
            WriteLine("Have Travel Pass: " + packet.ReadBit());
            WriteLine("Voice Chat Allowed: " + packet.ReadBit());

            Byte("Complain System Status");
            UInt32("Unknown Mail Url Related Value (SR)");
        }

        [Parser(OpCodes.SMSG_HEALTH_UPDATE)]
        public void HandleHealthUpdate(Parser packet)
        {
            ReadPackedGuid("GUID: {0:X16}");
            ReadUInt16("Value");
        }

        [Parser(OpCodes.CMSG_LOADING_SCREEN_NOTIFY)]
        public void HanleLoadingScreenNotify(Parser packet)
        {
            WriteLine("Loading Screen Active: " + packet.ReadBit());
            UInt32("MapID");
        }

        [Parser(OpCodes.SMSG_PVP_TYPES_ENABLED)]
        public void HanlePVPTypesEnabled(Parser packet)
        {
            WriteLine("War Games Enabled: " + packet.ReadBit());
            WriteLine("Unk Type Enabled: " + packet.ReadBit());
            WriteLine("Rated Battlegrounds Enabled: " + packet.ReadBit());
            WriteLine("Rated Arenas Enabled: " + packet.ReadBit());
            WriteLine("Unk Type Enabled: " + packet.ReadBit());
        }

        [Parser(OpCodes.SMSG_PROCRESIST)]
        public void HanleProcresist(Parser packet)
        {
            ReadGuid("PlayerGuid");
            ReadGuid("TragetGuid");
            UInt32("SpellID");
            UInt8("SomeBool");
        }

        [Parser(OpCodes.CMSG_PLAYED_TIME)]
        public void HanleCPlayedTime(Parser packet)
        {
            ReadBoolean("Unk(bool)");
        }

        [Parser(OpCodes.SMSG_PLAYED_TIME)]
        public void HanleSPlayedTime(Parser packet)
        {
            UInt32("Total PlayedTime");
            UInt32("Total PlayedTime");
            ReadBoolean("unk");
        }
    }
}