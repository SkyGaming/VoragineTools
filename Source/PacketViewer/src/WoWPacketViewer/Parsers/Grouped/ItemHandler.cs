using System;
using WowTools.Core;

namespace WoWPacketViewer
{
    public class ItemHandler : Parser
    {
        [Parser(OpCodes.SMSG_ITEM_QUERY_SINGLE_RESPONSE)]
        public void HandleItemQueryResponse(Parser packet)
        {
            var entry = packet.ReadEntry();
            WriteLine("Entry: " + entry.Key);

            if (entry.Value)
                return;

            var iClass = (ItemClass)packet.ReadInt32();
            WriteLine("Class: " + iClass);

            var subClass = packet.ReadInt32();
            WriteLine("Sub Class: " + subClass);

            var unk0 = packet.ReadInt32();
            WriteLine("Unk Int32: " + unk0);

            var name = new string[4];
            for (var i = 0; i < 4; i++)
            {
                name[i] = packet.ReadCString();
                WriteLine("Name " + i + ": " + name[i]);
            }

            var dispId = packet.ReadInt32();
            WriteLine("Display ID: " + dispId);

            var quality = (ItemQuality)packet.ReadInt32();
            WriteLine("Quality: " + quality);

            var flags = (ItemFlag)packet.ReadInt32();
            WriteLine("Flags: " + flags);

            var flags2 = (ItemFlagExtra)packet.ReadInt32();
            WriteLine("Extra Flags: " + flags2);

            var buyPrice = packet.ReadInt32();
            WriteLine("Buy Price: " + buyPrice);

            var sellPrice = packet.ReadInt32();
            WriteLine("Sell Price: " + sellPrice);

            var invType = (InventoryType)packet.ReadInt32();
            WriteLine("Inventory Type: " + invType);

            var allowClass = (ClassMask)packet.ReadInt32();
            WriteLine("Allowed Classes: " + allowClass);

            var allowRace = (RaceMask)packet.ReadInt32();
            WriteLine("Allowed Races: " + allowRace);

            var itemLvl = packet.ReadInt32();
            WriteLine("Item Level: " + itemLvl);

            var reqLvl = packet.ReadInt32();
            WriteLine("Required Level: " + reqLvl);

            var reqSkill = packet.ReadInt32();
            WriteLine("Required Skill ID: " + reqSkill);

            var reqSkLvl = packet.ReadInt32();
            WriteLine("Required Skill Level: " + reqSkLvl);

            var reqSpell = packet.ReadInt32();
            WriteLine("Required Spell: " + reqSpell);

            var reqHonor = packet.ReadInt32();
            WriteLine("Required Honor Rank: " + reqHonor);

            var reqCity = packet.ReadInt32();
            WriteLine("Required City Rank: " + reqCity);

            var reqRepFaction = packet.ReadInt32();
            WriteLine("Required Rep Faction: " + reqRepFaction);

            var reqRepValue = packet.ReadInt32();
            WriteLine("Required Rep Value: " + reqRepValue);

            var maxCount = packet.ReadInt32();
            WriteLine("Max Count: " + maxCount);

            var stacks = packet.ReadInt32();
            WriteLine("Max Stack Size: " + stacks);

            var contSlots = packet.ReadInt32();
            WriteLine("Container Slots: " + contSlots);

            var statsCount = packet.ReadInt32();
            WriteLine("Stats Count: " + statsCount);

            var type = new ItemModType[statsCount];
            var value = new int[statsCount];
            for (var i = 0; i < statsCount; i++)
            {
                type[i] = (ItemModType)packet.ReadInt32();
                WriteLine("Stat Type " + i + ": " + type[i]);

                value[i] = packet.ReadInt32();
                WriteLine("Stat Value " + i + ": " + value[i]);
            }

            var ssdId = packet.ReadInt32();
            WriteLine("SSD ID: " + ssdId);

            var ssdVal = packet.ReadInt32();
            WriteLine("SSD Value: " + ssdVal);

            var dmgMin = new float[2];
            var dmgMax = new float[2];
            var dmgType = new DamageType[2];
            for (var i = 0; i < 2; i++)
            {
                dmgMin[i] = packet.ReadSingle();
                WriteLine("Damage Min " + i + ": " + dmgMin[i]);

                dmgMax[i] = packet.ReadSingle();
                WriteLine("Damage Max " + i + ": " + dmgMax[i]);

                dmgType[i] = (DamageType)packet.ReadInt32();
                WriteLine("Damage Type " + i + ": " + dmgType[i]);
            }

            var resistance = new int[7];
            for (var i = 0; i < 7; i++)
            {
                resistance[i] = packet.ReadInt32();
                WriteLine((DamageType)i + " Resistance: " + resistance[i]);
            }

            var delay = packet.ReadInt32();
            WriteLine("Delay: " + delay);

            var ammoType = (AmmoType)packet.ReadInt32();
            WriteLine("Ammo Type: " + ammoType);

            var rangedMod = packet.ReadSingle();
            WriteLine("Ranged Mod: " + rangedMod);

            var spellId = new int[5];
            var spellTrigger = new ItemSpellTriggerType[5];
            var spellCharges = new int[5];
            var spellCooldown = new int[5];
            var spellCategory = new int[5];
            var spellCatCooldown = new int[5];
            for (var i = 0; i < 5; i++)
            {
                spellId[i] = packet.ReadInt32();
                WriteLine("Triggered Spell ID " + i + ": " + spellId[i]);

                spellTrigger[i] = (ItemSpellTriggerType)packet.ReadInt32();
                WriteLine("Triggered Spell Type " + i + ": " + spellTrigger[i]);

                spellCharges[i] = packet.ReadInt32();
                WriteLine("Triggered Spell Charges " + i + ": " + spellCharges[i]);

                spellCooldown[i] = packet.ReadInt32();
                WriteLine("Triggered Spell Cooldown " + i + ": " + spellCooldown[i]);

                spellCategory[i] = packet.ReadInt32();
                WriteLine("Triggered Spell Category " + i + ": " + spellCategory[i]);

                spellCatCooldown[i] = packet.ReadInt32();
                WriteLine("Triggered Spell Category Cooldown " + i + ": " + spellCatCooldown[i]);
            }

            var binding = (ItemBonding)packet.ReadInt32();
            WriteLine("Bonding: " + binding);

            var description = packet.ReadCString();
            WriteLine("Description: " + description);

            var pageText = packet.ReadInt32();
            WriteLine("Page Text: " + pageText);

            var langId = (Language)packet.ReadInt32();
            WriteLine("Language ID: " + langId);

            var pageMat = (PageMaterial)packet.ReadInt32();
            WriteLine("Page Material: " + pageMat);

            var startQuest = packet.ReadInt32();
            WriteLine("Start Quest: " + startQuest);

            var lockId = packet.ReadInt32();
            WriteLine("Lock ID: " + lockId);

            var material = (Material)packet.ReadInt32();
            WriteLine("Material: " + material);

            var sheath = (SheathType)packet.ReadInt32();
            WriteLine("Sheath Type: " + sheath);

            var randomProp = packet.ReadInt32();
            WriteLine("Random Property: " + randomProp);

            var randomSuffix = packet.ReadInt32();
            WriteLine("Random Suffix: " + randomSuffix);

            var block = packet.ReadInt32();
            WriteLine("Block: " + block);

            var itemSet = packet.ReadInt32();
            WriteLine("Item Set: " + itemSet);

            var maxDura = packet.ReadInt32();
            WriteLine("Max Durability: " + maxDura);

            var area = packet.ReadInt32();
            WriteLine("Area: " + area);

            var map = packet.ReadInt32();
            WriteLine("Map: " + map);

            var bagFamily = (BagFamilyMask)packet.ReadInt32();
            WriteLine("Bag Family: " + bagFamily);

            var totemCat = (TotemCategory)packet.ReadInt32();
            WriteLine("Totem Category: " + totemCat);

            var color = new ItemSocketColor[3];
            var content = new int[3];
            for (var i = 0; i < 3; i++)
            {
                color[i] = (ItemSocketColor)packet.ReadInt32();
                WriteLine("Socket Color " + i + ": " + color[i]);

                content[i] = packet.ReadInt32();
                WriteLine("Socket Item " + i + ": " + content[i]);
            }

            var socketBonus = packet.ReadInt32();
            WriteLine("Socket Bonus: " + socketBonus);

            var gemProps = packet.ReadInt32();
            WriteLine("Gem Properties: " + gemProps);

            var reqDisEnchSkill = packet.ReadInt32();
            WriteLine("Required Disenchant Skill: " + reqDisEnchSkill);

            var armorDmgMod = packet.ReadSingle();
            WriteLine("Armor Damage Modifier: " + armorDmgMod);

            var duration = packet.ReadInt32();
            WriteLine("Duration: " + duration);

            var limitCategory = packet.ReadInt32();
            WriteLine("Limit Category: " + limitCategory);

            var holidayId = (Holiday)packet.ReadInt32();
            WriteLine("Holiday: " + holidayId);
        }

        [Parser(OpCodes.SMSG_UPDATE_ITEM_ENCHANTMENTS)]
        public void HandleUpdateItemEnchantments(Parser packet)
        {
            for (var i = 0; i < 4; i++)
            {
                var enchId = packet.ReadInt32();
                WriteLine("Aura ID " + i + ": " + enchId);
            }
        }

        [Parser(OpCodes.CMSG_BUY_ITEM)]
        public void HandleBuyItem(Parser packet)
        {
            UInt64("VendorGuid");
            UInt8("unk(uint8)");
            UInt32("Item");
            UInt32("Slot");
            UInt32("Count");
            UInt64("unk(uint64)");
            UInt8("unk(uint8)");
        }
    }
}
