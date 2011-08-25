using System;
using WowTools.Core;





namespace WoWPacketViewer
{
    public class QueryHandler : Parser
    {
        [Parser(OpCodes.SMSG_QUERY_TIME_RESPONSE)]
        public void HandleTimeQueryResponse(Parser packet)
        {
            var curTime = packet.ReadTime();
            WriteLine("Current Time: " + curTime);

            var dailyReset = packet.ReadInt32();
            WriteLine("Daily Quest Reset: " + dailyReset);
        }

        [Parser(OpCodes.CMSG_NAME_QUERY)]
        public void HandleNameQuery(Parser packet)
        {
            var guid = packet.ReadGuid();
            WriteLine("GUID: " + guid);
        }

        [Parser(OpCodes.SMSG_NAME_QUERY_RESPONSE)]
        public void HandleNameQueryResponse(Parser packet)
        {
            var pguid = packet.ReadPackedGuid();
            WriteLine("GUID: " + pguid);

            var end = packet.ReadBoolean();
            WriteLine("Name Found: " + !end);

            if (end)
                return;

            var name = packet.ReadCString();
            WriteLine("Name: " + name);

            var realmName = packet.ReadCString();
            WriteLine("Realm Name: " + realmName);

            var race = (Race)packet.ReadByte();
            WriteLine("Race: " + race);

            var gender = (Gender)packet.ReadByte();
            WriteLine("Gender: " + gender);

            var cClass = (Class)packet.ReadByte();
            WriteLine("Class: " + cClass);

            var decline = packet.ReadBoolean();
            WriteLine("Name Declined: " + decline);

            if (!decline)
                return;

            for (var i = 0; i < 5; i++)
            {
                var declinedName = packet.ReadCString();
                WriteLine("Declined Name " + i + ": " + declinedName);
            }
        }

        public void ReadQueryHeader(Parser packet)
        {
            var entry = packet.ReadInt32();
            WriteLine("Entry: " + entry);

            var guid = packet.ReadGuid();
            WriteLine("GUID: " + guid);
        }

        [Parser(OpCodes.CMSG_CREATURE_QUERY)]
        public void HandleCreatureQuery(Parser packet)
        {
            ReadQueryHeader(packet);
        }

        [Parser(OpCodes.SMSG_CREATURE_QUERY_RESPONSE)]
        public void HandleCreatureQueryResponse(Parser packet)
        {
            var entry = packet.ReadEntry();
            WriteLine("Entry: " + entry.Key);

            if (entry.Value)
                return;

            var name = new string[4];
            for (var i = 0; i < 4; i++)
            {
                name[i] = packet.ReadCString();
                WriteLine("Name " + i + ": " + name[i]);
            }

            var subName = packet.ReadCString();
            WriteLine("Sub Name: " + subName);

            var iconName = packet.ReadCString();
            WriteLine("Icon Name: " + iconName);

            var typeFlags = (CreatureTypeFlag)packet.ReadInt32();
            WriteLine("Type Flags: " + typeFlags);

            var type = (CreatureType)packet.ReadInt32();
            WriteLine("Type: " + type);

            var family = (CreatureFamily)packet.ReadInt32();
            WriteLine("Family: " + family);

            var rank = (CreatureRank)packet.ReadInt32();
            WriteLine("Rank: " + rank);

            var killCredit = new int[2];
            for (var i = 0; i < 2; i++)
            {
                killCredit[i] = packet.ReadInt32();
                WriteLine("Kill Credit " + i + ": " + killCredit[i]);
            }

            var dispId = new int[4];
            for (var i = 0; i < 4; i++)
            {
                dispId[i] = packet.ReadInt32();
                WriteLine("Display ID " + i + ": " + dispId[i]);
            }

            var mod1 = packet.ReadSingle();
            WriteLine("Modifier 1: " + mod1);

            var mod2 = packet.ReadSingle();
            WriteLine("Modifier 2: " + mod2);

            var racialLeader = packet.ReadBoolean();
            WriteLine("Racial Leader: " + racialLeader);

            var qItem = new int[6];
            for (var i = 0; i < 6; i++)
            {
                qItem[i] = packet.ReadInt32();
                WriteLine("Quest Item " + i + ": " + qItem[i]);
            }

            var moveId = packet.ReadInt32();
            WriteLine("Movement ID: " + moveId);

           //SQLStore.WriteData(SQLStore.Creatures.GetCommand(entry.Key, name[0], subName, iconName, typeFlags,
           //     type, family, rank, killCredit, dispId, mod1, mod2, racialLeader, qItem, moveId));
        }

        [Parser(OpCodes.CMSG_PAGE_TEXT_QUERY)]
        public void HandlePageTextQuery(Parser packet)
        {
            ReadQueryHeader(packet);
        }

        [Parser(OpCodes.SMSG_PAGE_TEXT_QUERY_RESPONSE)]
        public void HandlePageTextResponse(Parser packet)
        {
            var entry = packet.ReadInt32();
            WriteLine("Entry: " + entry);

            var text = packet.ReadCString();
            WriteLine("Page Text: " + text);

            var pageId = packet.ReadInt32();
            WriteLine("Next Page: " + pageId);

            //SQLStore.WriteData(SQLStore.PageTexts.GetCommand(entry, text, pageId));
        }

        [Parser(OpCodes.CMSG_NPC_TEXT_QUERY)]
        public void HandleNpcTextQuery(Parser packet)
        {
            ReadQueryHeader(packet);
        }

        [Parser(OpCodes.SMSG_NPC_TEXT_UPDATE)]
        public void HandleNpcTextUpdate(Parser packet)
        {
            var entry = packet.ReadInt32();
            WriteLine("Entry: " + entry);

            var prob = new float[8];
            var text1 = new string[8];
            var text2 = new string[8];
            var lang = new Language[8];
            var emDelay = new int[8][];
            var emEmote = new int[8][];
            for (var i = 0; i < 8; i++)
            {
                prob[i] = packet.ReadSingle();
                WriteLine("Probability " + i + ": " + prob[i]);

                text1[i] = packet.ReadCString();
                WriteLine("Text 1 " + i + ": " + text1[i]);

                text2[i] = packet.ReadCString();
                WriteLine("Text 2 " + i + ": " + text2[i]);

                lang[i] = (Language)packet.ReadInt32();
                WriteLine("Language " + i + ": " + lang[i]);

                emDelay[i] = new int[3];
                emEmote[i] = new int[3];
                for (var j = 0; j < 3; j++)
                {
                    emDelay[i][j] = packet.ReadInt32();
                    WriteLine("Emote Delay " + j + ": " + emDelay[i][j]);

                    emEmote[i][j] = packet.ReadInt32();
                    WriteLine("Emote ID " + j + ": " + emEmote[i][j]);
                }
            }

            //SQLStore.WriteData(SQLStore.NpcTexts.GetCommand(entry, prob, text1, text2, lang, emDelay, emEmote));
        }
    }
}
