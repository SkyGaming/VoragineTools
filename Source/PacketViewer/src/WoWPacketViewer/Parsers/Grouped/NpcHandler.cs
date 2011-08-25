using System;
using WowTools.Core;





namespace WoWPacketViewer
{
    public class NpcHandler : Parser
    {
        [Parser(OpCodes.SMSG_GOSSIP_POI)]
        public void HandleGossipPoi(Parser packet)
        {
            var flags = packet.ReadInt32();
            WriteLine("Flags: 0x" + flags.ToString("X8"));

            Float("X");
            Float("Y");

            var icon = (GossipPoiIcon)packet.ReadInt32();
            WriteLine("Icon: " + icon);

            var data = packet.ReadInt32();
            WriteLine("Data: " + data);

            var iconName = packet.ReadCString();
            WriteLine("Icon Name: " + iconName);
        }

        [Parser(OpCodes.SMSG_TRAINER_LIST)]
        public void HandleServerTrainerList(Parser packet)
        {
            var guid = packet.ReadGuid();
            WriteLine("GUID: " + guid);

            var type = (TrainerType)packet.ReadInt32();
            WriteLine("Type: " + type);

            var count = packet.ReadInt32();
            WriteLine("Count: " + count);

            for (var i = 0; i < count; i++)
            {
                var spell = packet.ReadInt32();
                WriteLine("Spell ID " + i + ": " + spell);

                var state = (TrainerSpellState)packet.ReadByte();
                WriteLine("State " + i + ": " + state);

                var cost = packet.ReadInt32();
                WriteLine("Cost " + i + ": " + cost);

                var profDialog = packet.ReadBoolean();
                WriteLine("Profession Dialog " + i + ": " + profDialog);

                var profButton = packet.ReadBoolean();
                WriteLine("Profession Button " + i + ": " + profButton);

                var reqLevel = packet.ReadInt32();
                WriteLine("Required Level " + i + ": " + reqLevel);

                var reqSkill = packet.ReadInt32();
                WriteLine("Required Skill " + i + ": " + reqSkill);

                var reqSkLvl = packet.ReadInt32();
                WriteLine("Required Skill Level " + i + ": " + reqSkLvl);

                var chainNode1 = packet.ReadInt32();
                WriteLine("Chain Node 1 " + i + ": " + chainNode1);

                var chainNode2 = packet.ReadInt32();
                WriteLine("Chain Node 2 " + i + ": " + chainNode2);

                var unk = packet.ReadInt32();
                WriteLine("Unk Int32 " + i + ": " + unk);

                //SQLStore.WriteData(SQLStore.TrainerSpells.GetCommand(guid.GetEntry(), spell, cost, reqLevel,
                //    reqSkill, reqSkLvl));
            }

            var titleStr = packet.ReadCString();
            WriteLine("Title: " + titleStr);
        }

        [Parser(OpCodes.SMSG_LIST_INVENTORY)]
        public void HandleVendorInventoryList(Parser packet)
        {
            var guid = packet.ReadGuid();
            WriteLine("GUID: " + guid);

            var itemCount = packet.ReadByte();
            WriteLine("Item Count: " + itemCount);

            for (var i = 0; i < itemCount; i++)
            {
                var position = packet.ReadInt32();
                WriteLine("Item Position " + position + ": " + position);

                var itemId = packet.ReadInt32();
                WriteLine("Item ID " + i + ": " + itemId);

                var dispid = packet.ReadInt32();
                WriteLine("Display ID " + i + ": " + dispid);

                var maxCount = packet.ReadInt32();
                WriteLine("Max Count " + i + ": " + maxCount);

                var price = packet.ReadInt32();
                WriteLine("Price " + i + ": " + price);

                var maxDura = packet.ReadInt32();
                WriteLine("Max Durability " + i + ": " + maxDura);

                var buyCount = packet.ReadInt32();
                WriteLine("Buy Count " + i + ": " + buyCount);

                var extendedCost = packet.ReadInt32();
                WriteLine("Extended Cost " + i + ": " + extendedCost);

                //SQLStore.WriteData(SQLStore.VendorItems.GetCommand(guid.GetEntry(), itemId, maxCount,
                //    extendedCost));
            }
        }

        [Parser(OpCodes.CMSG_GOSSIP_HELLO)]
        [Parser(OpCodes.CMSG_TRAINER_LIST)]
        [Parser(OpCodes.CMSG_BATTLEMASTER_HELLO)]
        [Parser(OpCodes.CMSG_LIST_INVENTORY)]
        [Parser(OpCodes.MSG_TABARDVENDOR_ACTIVATE)]
        [Parser(OpCodes.CMSG_BANKER_ACTIVATE)]
        [Parser(OpCodes.CMSG_SPIRIT_HEALER_ACTIVATE)]
        [Parser(OpCodes.CMSG_BINDER_ACTIVATE)]
        public void HandleNpcHello(Parser packet)
        {
            var guid = packet.ReadGuid();
            WriteLine("GUID: " + guid);
        }

        [Parser(OpCodes.SMSG_GOSSIP_MESSAGE)]
        public void HandleNpcGossip(Parser packet)
        {
            var guid = packet.ReadGuid();
            WriteLine("GUID: " + guid);

            var entry = packet.ReadUInt32();
            WriteLine("Menu id: " + entry);

            var textid = packet.ReadUInt32();
            WriteLine("Text id: " + textid);

            var count = packet.ReadUInt32();
            WriteLine("- Amount of Options: " + count);

            for (var i = 0; i < count; i++)
            {
                if (i != 0)
                    WriteLine("\t--");

                var index = packet.ReadUInt32();
                WriteLine("\tIndex: " + index);

                var icon = packet.ReadByte();
                WriteLine("\tIcon: " + icon);

                var box = packet.ReadBoolean();
                WriteLine("\tBox: " + box);

                var boxMoney = packet.ReadUInt32();
                if (box) // Only print if there's a box. avaliable.
                    WriteLine("\tRequired money: " + boxMoney);

                var text = packet.ReadCString();
                WriteLine("\tText: " + text);

                var boxText = packet.ReadCString();
                if (box) // Only print if there's a box avaliable.
                    WriteLine("\tBox text: " + boxText);
            }

            var questgossips = packet.ReadUInt32();
            WriteLine("- Amount of Quest gossips: " + questgossips);

            for (var i = 0; i < questgossips; i++)
            {
                if (i != 0)
                    WriteLine("\t--");

                var questID = packet.ReadUInt32();
                WriteLine("\tQuest ID: " + questID);

                var questicon = packet.ReadUInt32();
                WriteLine("\tIcon: " + questicon);

                var questlevel = packet.ReadInt32();
                WriteLine("\tLevel: " + questlevel);

                var flags = (QuestFlag)(packet.ReadUInt32() | 0xFFFF);
                WriteLine("\tFlags: " + flags);

                var unk1 = packet.ReadBoolean();
                WriteLine("\tUnknown bool: " + unk1);

                var title = packet.ReadCString();
                WriteLine("\tTitle: " + title);
            }
        }


        [Parser(OpCodes.SMSG_THREAT_UPDATE)]
        [Parser(OpCodes.SMSG_HIGHEST_THREAT_UPDATE)]
        public void HandleThreatlistUpdate(Parser packet)
        {
            var guid = packet.ReadPackedGuid();
            WriteLine("GUID: " + guid);

            if (Packet.Code == OpCodes.SMSG_HIGHEST_THREAT_UPDATE)
            {
                var newhigh = packet.ReadPackedGuid();
                WriteLine("New Highest: " + newhigh);
            }

            var count = packet.ReadUInt32();
            WriteLine("Size: " + count);
            for (int i = 0; i < count; i++)
            {
                var unit = packet.ReadPackedGuid();
                WriteLine("Hostile: " + unit);
                var threat = packet.ReadUInt32();
                // No idea why, but this is in core.
                /*if (packet.GetOpcode() == Opcode.SMSG_THREAT_UPDATE)
                    threat *= 100;*/
                WriteLine("Threat: " + threat);
            }
        }

        [Parser(OpCodes.SMSG_THREAT_CLEAR)]
        [Parser(OpCodes.SMSG_THREAT_REMOVE)]
        public void HandleRemoveThreatlist(Parser packet)
        {
            var guid = packet.ReadPackedGuid();
            WriteLine("GUID: " + guid);

            if (Packet.Code == OpCodes.SMSG_THREAT_REMOVE)
            {
                var victim = packet.ReadPackedGuid();
                WriteLine("Victim GUID: " + victim);
            }
        }
    }
}
