using System;
using WowTools.Core;
using System.Collections.Generic;





namespace WoWPacketViewer
{
    public class QuestHandler : Parser
    {
        [Parser(OpCodes.CMSG_QUEST_QUERY)]
        public void HandleQuestQuery(Parser packet)
        {
            var entry = packet.ReadInt32();
            WriteLine("Entry: " + entry);
        }

        [Parser(OpCodes.SMSG_QUEST_QUERY_RESPONSE)]
        public void HandleQuestQueryResponse(Parser packet)
        {
            var id = packet.ReadInt32();
            WriteLine("Entry: " + id);

            var method = (QuestMethod)packet.ReadInt32();
            WriteLine("Method: " + method);

            var level = packet.ReadInt32();
            WriteLine("Level: " + level);

            var minLevel = packet.ReadInt32();
            WriteLine("Min Level: " + minLevel);

            var sort = (QuestSort)packet.ReadInt32();
            WriteLine("Sort: " + sort);

            var type = (QuestType)packet.ReadInt32();
            WriteLine("Type: " + type);

            var players = packet.ReadInt32();
            WriteLine("Suggested Players: " + players);

            var factId = new int[2];
            var factRep = new int[2];
            for (var i = 0; i < 2; i++)
            {
                factId[i] = packet.ReadInt32();
                WriteLine("Required Faction ID " + i + ": " + factId[i]);

                factRep[i] = packet.ReadInt32();
                WriteLine("Required Faction Rep " + i + ": " + factRep[i]);
            }

            var nextQuest = packet.ReadInt32();
            WriteLine("Next Chain Quest: " + nextQuest);

            var xpId = packet.ReadInt32();
            WriteLine("Quest XP ID: " + xpId);

            var rewReqMoney = packet.ReadInt32();
            WriteLine("Reward/Required Money: " + rewReqMoney);

            var rewMoneyMaxLvl = packet.ReadInt32();
            WriteLine("Reward Money Max Level: " + rewMoneyMaxLvl);

            var rewSpell = packet.ReadInt32();
            WriteLine("Reward Spell: " + rewSpell);

            var rewSpellCast = packet.ReadInt32();
            WriteLine("Reward Spell Cast: " + rewSpellCast);

            var rewHonor = packet.ReadInt32();
            WriteLine("Reward Honor: " + rewHonor);

            var rewHonorBonus = packet.ReadSingle();
            WriteLine("Reward Honor Multiplier: " + rewHonorBonus);

            var srcItemId = packet.ReadInt32();
            WriteLine("Source Item ID: " + srcItemId);

            var flags = (QuestFlag)(packet.ReadInt32() | 0xFFFF);
            WriteLine("Flags: " + flags);

            var titleId = packet.ReadInt32();
            WriteLine("Title ID: " + titleId);

            var reqPlayerKills = packet.ReadInt32();
            WriteLine("Required Player Kills: " + reqPlayerKills);

            var bonusTalents = packet.ReadInt32();
            WriteLine("Bonus Talents: " + bonusTalents);

            var bonusArenaPoints = packet.ReadInt32();
            WriteLine("Bonus Arena Points: " + bonusArenaPoints);

            var bonusUnk = packet.ReadInt32();
            WriteLine("Unk Int32: " + bonusUnk);

            var rewItemId = new int[4];
            var rewItemCnt = new int[4];
            for (var i = 0; i < 4; i++)
            {
                rewItemId[i] = packet.ReadInt32();
                WriteLine("Reward Item ID " + i + ": " + rewItemId[i]);

                rewItemCnt[i] = packet.ReadInt32();
                WriteLine("Reward Item Count " + i + ": " + rewItemCnt[i]);
            }

            var rewChoiceItemId = new int[6];
            var rewChoiceItemCnt = new int[6];
            for (var i = 0; i < 6; i++)
            {
                rewChoiceItemId[i] = packet.ReadInt32();
                WriteLine("Reward Choice Item ID " + i + ": " + rewChoiceItemId[i]);

                rewChoiceItemCnt[i] = packet.ReadInt32();
                WriteLine("Reward Choice Item Count " + i + ": " + rewChoiceItemCnt[i]);
            }

            var rewFactionId = new int[5];
            for (var i = 0; i < 5; i++)
            {
                rewFactionId[i] = packet.ReadInt32();
                WriteLine("Reward Faction ID " + i + ": " + rewFactionId[i]);
            }

            var rewRepIdx = new int[5];
            for (var i = 0; i < 5; i++)
            {
                rewRepIdx[i] = packet.ReadInt32();
                WriteLine("Reward Reputation ID " + i + ": " + rewRepIdx[i]);
            }

            var rewRepOverride = new int[5];
            for (var i = 0; i < 5; i++)
            {
                rewRepOverride[i] = packet.ReadInt32();
                WriteLine("Reward Rep Override " + i + ": " + rewRepOverride[i]);
            }

            var pointMap = packet.ReadInt32();
            WriteLine("Point Map ID: " + pointMap);

            var pointX = packet.ReadSingle();
            WriteLine("Point X: " + pointX);

            var pointY = packet.ReadSingle();
            WriteLine("Point Y: " + pointY);

            var pointOpt = packet.ReadInt32();
            WriteLine("Point Opt: " + pointOpt);

            var title = packet.ReadCString();
            WriteLine("Title: " + title);

            var objectives = packet.ReadCString();
            WriteLine("Objectives: " + objectives);

            var details = packet.ReadCString();
            WriteLine("Details: " + details);

            var endText = packet.ReadCString();
            WriteLine("End Text: " + endText);

            var returnText = packet.ReadCString();
            WriteLine("Return Text: " + returnText);

            var reqId = new KeyValuePair<int, bool>[4];
            var reqCnt = new int[4];
            var srcId = new int[4];
            var srcCnt = new int[4];
            for (var i = 0; i < 4; i++)
            {
                reqId[i] = packet.ReadEntry();
                WriteLine("Required " + (reqId[i].Value ? "GO" : "NPC") +
                    " ID " + i + ": " + reqId[i].Key);

                reqCnt[i] = packet.ReadInt32();
                WriteLine("Required Count: " + i + ": " + reqCnt[i]);

                srcId[i] = packet.ReadInt32();
                WriteLine("Source ID: " + i + ": " + srcId[i]);

                srcCnt[i] = packet.ReadInt32();
                WriteLine("Source Count: " + i + ": " + srcCnt[i]);
            }

            var reqItemId = new int[6];
            var reqItemCnt = new int[6];
            for (var i = 0; i < 6; i++)
            {
                reqItemId[i] = packet.ReadInt32();
                WriteLine("Required Item ID " + i + ": " + reqItemId[i]);

                reqItemCnt[i] = packet.ReadInt32();
                WriteLine("Required Item Count: " + i + ": " + reqItemCnt[i]);
            }

            var objectiveText = new string[4];
            for (var i = 0; i < 4; i++)
            {
                objectiveText[i] = packet.ReadCString();
                WriteLine("Objective Text " + i + ": " + objectiveText[i]);
            }

            //SQLStore.WriteData(SQLStore.Quests.GetCommand(id, method, level, minLevel, sort, type,
            //    players, factId, factRep, nextQuest, xpId, rewReqMoney, rewMoneyMaxLvl,
            //    rewSpell, rewSpellCast, rewHonor, rewHonorBonus, srcItemId, flags, titleId,
            //    reqPlayerKills, bonusTalents, bonusArenaPoints, bonusUnk, rewItemId, rewItemCnt,
            //    rewChoiceItemId, rewChoiceItemCnt, rewFactionId, rewRepIdx, rewRepOverride,
            //    pointMap, pointX, pointY, pointOpt, title, objectives, details, endText,
            //    returnText, reqId, reqCnt, srcId, srcCnt, reqItemId, reqItemCnt, objectiveText));
        }

        [Parser(OpCodes.CMSG_QUEST_POI_QUERY)]
        public void HandleQuestPoiQuery(Parser packet)
        {
            var count = packet.ReadInt32();
            WriteLine("Count: " + count);

            for (var i = 0; i < count; i++)
            {
                var questId = packet.ReadInt32();
                WriteLine("Quest ID " + i + ": " + questId);
            }
        }

        [Parser(OpCodes.SMSG_QUEST_POI_QUERY_RESPONSE)]
        public void HandleQuestPoiQueryResponse(Parser packet)
        {
            var count = packet.ReadInt32();
            WriteLine("Count: " + count);

            for (var i = 0; i < count; i++)
            {
                var questId = packet.ReadInt32();
                WriteLine("Quest ID " + i + ": " + questId);

                var poiSize = packet.ReadInt32();
                WriteLine("POI Size " + i + ": " + poiSize);

                for (var j = 0; j < poiSize; j++)
                {
                    var idx = packet.ReadInt32();
                    WriteLine("POI Index " + j + ": " + idx);

                    var objIndex = packet.ReadInt32();
                    WriteLine("Objective Index " + j + ": " + objIndex);

                    var mapId = packet.ReadInt32();
                    WriteLine("Map ID " + j + ": " + mapId);

                    var wmaId = packet.ReadInt32();
                    WriteLine("World Map Area " + j + ": " + wmaId);

                    var unk2 = packet.ReadInt32();
                    WriteLine("Floor ID " + j + ": " + unk2);

                    var unk3 = packet.ReadInt32();
                    WriteLine("Unk Int32 2 " + j + ": " + unk3);

                    var unk4 = packet.ReadInt32();
                    WriteLine("Unk Int32 3 " + j + ": " + unk4);

                    //SQLStore.WriteData(SQLStore.QuestPois.GetCommand(questId, idx, objIndex, mapId, wmaId,
                    //    unk2, unk3, unk4));

                    var pointsSize = packet.ReadInt32();
                    WriteLine("Point Size " + j + ": " + pointsSize);

                    for (var k = 0; k < pointsSize; k++)
                    {
                        var pointX = packet.ReadInt32();
                        WriteLine("Point X " + k + ": " + pointX);

                        var pointY = packet.ReadInt32();
                        WriteLine("Point Y " + k + ": " + pointY);

                        //SQLStore.WriteData(SQLStore.QuestPoiPoints.GetCommand(questId, idx, objIndex, pointX,
                        //    pointY));
                    }
                }
            }
        }

        [Parser(OpCodes.SMSG_QUEST_FORCE_REMOVED)]
        public void HandleQuestForceRemoved(Parser packet)
        {
            var questId = packet.ReadInt32();
            WriteLine("Quest ID: " + questId);
        }

        [Parser(OpCodes.SMSG_QUESTGIVER_QUEST_COMPLETE)]
        public void HandleQuestCompleted(Parser packet)
        {
            var questId = packet.ReadInt32();
            WriteLine("Quest ID: " + questId);

            WriteLine("Reward:");
            var xp = packet.ReadInt32();
            WriteLine("XP: " + xp);

            var money = packet.ReadInt32();
            WriteLine("Money: " + money);

            var honor = packet.ReadInt32();
            if (honor < 0)
                WriteLine("Honor: " + honor);

            var talentpoints = packet.ReadInt32();
            if (talentpoints < 0)
                WriteLine("Talentpoints: " + talentpoints);

            var arenapoints = packet.ReadInt32();
            if (arenapoints < 0)
                WriteLine("Arenapoints: " + arenapoints);
        }

        [Parser(OpCodes.CMSG_QUESTGIVER_STATUS_QUERY)]
        public void HandleQuestGiverStatusQuery(Parser packet)
        {
            ReadGuid("Guid");
        }
    }
}
