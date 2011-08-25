using System;
using System.Text;
using WowTools.Core;

namespace WoWPacketViewer
{
    public class SpellHandler : Parser
    {
        [Parser(OpCodes.SMSG_SEND_UNLEARN_SPELLS)]
        public void UnlearnSpell(Parser packet)
        {
            var count = packet.ReadInt32();
            WriteLine("Count: " + count);

            for (var i = 0; i < count; i++)
            {
                var spellId = packet.ReadInt32();
                WriteLine("Spell ID " + i + ": " + spellId);
            }
        }

        [Parser(OpCodes.SMSG_POWER_UPDATE)]
        public void HandleSetPower(Parser packet)
        {
            ReadPackedGuid("GUID");
            AppendFormatLine("Type: {0}", (PowerType)Reader.ReadByte());
            UInt32("unk");
            UInt32("Value");
        }
        

        [Parser(OpCodes.SMSG_UNIT_SPELLCAST_START)]
        public void HandleUnitSpellCastStart(Parser packet)
        {
            ReadPackedGuid("CasterGUID");
            ReadPackedGuid("Target GUID");
            UInt32("SpellID");
            UInt32("CastTime");
            UInt32("unk");
        }

        [Parser(OpCodes.SMSG_INITIAL_SPELLS)]
        public void HandleInitialSpells(Parser packet)
        {
            ReadByte("Talent Spec");

            var spellsCount = Reader.ReadUInt16();
            AppendFormatLine("Spells count: {0}", spellsCount);

            for (var i = 0; i < spellsCount; ++i)
            {
                UInt32("SpellID");
                UInt16("slot");
            }

            var cooldownsCount = Reader.ReadUInt16();
            AppendFormatLine("Cooldowns count: {0}", cooldownsCount);

            for (var i = 0; i < cooldownsCount; ++i)
            {
                UInt32("spellId");
                UInt16("itemId");
                UInt32("category");
                UInt32("time1");
                UInt32("time2");
            }
        }

        [Parser(OpCodes.SMSG_AURA_UPDATE)]
        public void HandleAuraUpdate(Parser packet)
        {
            ReadPackedGuid("GUID");
            ReadAuraUpdateBlock(packet);
        }

        [Parser(OpCodes.SMSG_LEARNED_SPELL)]
        public void HandleLearnedSpell(Parser packet)
        {
            UInt32("Spell ID");
            UInt16("unk");
        }

        [Parser(OpCodes.CMSG_UPDATE_PROJECTILE_POSITION)]
        public void HandleUpdateProjectilePosition(Parser packet)
        {
            ReadGuid("GUID");
            UInt32("Spell ID");
            Byte("Cast ID");

            var pos = packet.ReadCoords3();
            Console.WriteLine("Position: " + pos);
        }

        [Parser(OpCodes.SMSG_SET_PROJECTILE_POSITION)]
        public void HandleSetProjectilePosition(Parser packet)
        {
            ReadGuid("GUID");
            Byte("Cast ID");

            var pos = packet.ReadCoords3();
            Console.WriteLine("Position: " + pos);
        }

        [Parser(OpCodes.SMSG_PET_SPELLS)]
        public void HandlePetSpells(Parser packet)
        {
            var petGUID = Reader.ReadUInt64();
            AppendFormatLine("GUID: {0:X16}", petGUID);

            if (petGUID != 0)
            {
                UInt16("Pet family");
                UInt32("unk1");
                UInt32("unk2");

                for (var i = 0; i < 10; ++i)
                {
                    UInt16("spellOrAction");
                    UInt16("type");
                }

                var spellsCount = Reader.ReadByte();
                AppendFormatLine("Spells count: {0}", spellsCount);

                for (var i = 0; i < spellsCount; ++i)
                {
                    UInt16("spellId");
                    UInt16("active");
                }

                var cooldownsCount = Reader.ReadByte();
                AppendFormatLine("Cooldowns count: {0}", cooldownsCount);

                for (var i = 0; i < cooldownsCount; ++i)
                {
                    UInt32("spellId");
                    UInt16("category");
                    UInt32("cooldown");
                    UInt32("categoryCooldown");
                }
            }
        }

        [Parser(OpCodes.SMSG_SPELL_START)]
        public void HandleSpellStart(Parser packet)
        {
            ReadPackedGuid("Caster");
            ReadPackedGuid("Target");
            Byte("Pending Cast");
            UInt32("Spell ID");

            var cf = (CastFlag)Reader.ReadUInt32();
            WriteLine("Cast Flags: {0}", cf);
            UInt32("Timer");

            ReadTargets();

            if (cf.HasFlag(CastFlag.PredictedPower))
                UInt32("PredictedPower");

            if (cf.HasFlag(CastFlag.RuneInfo))
            {
                var v1 = Reader.ReadByte();
                WriteLine("RuneState Before: {0}", (CooldownMask)v1);
                var v2 = Reader.ReadByte();
                WriteLine("RuneState Now: {0}", (CooldownMask)v2);

                for (var i = 0; i < 6; ++i)
                {
                    var v3 = (i << i);

                    if ((v3 & v1) != 0)
                    {
                        if ((v3 & v2) == 0)
                        {
                            var v4 = Reader.ReadByte();
                            WriteLine("Cooldown for {0} is {1}", (CooldownMask)v3, v4);
                        }
                    }
                }
            }

            if (cf.HasFlag(CastFlag.Projectile))
            {
                UInt32("Projectile displayid");
                UInt32("inventoryType");
            }
        }

        [Parser(OpCodes.SMSG_SPELL_GO)]
        public void HandleSpellGo(Parser packet)
        {
            ReadPackedGuid("Caster");
            ReadPackedGuid("Target");
            Byte("Pending Cast");
            UInt32("Spell ID");

            var cf = (CastFlag)Reader.ReadUInt32();
            WriteLine("Cast Flags: {0}", cf);
            UInt32("TickCount");

            ReadSpellGoTargets();
            var tf = ReadTargets();

            if (cf.HasFlag(CastFlag.PredictedPower))
                UInt32("PredictedPower");

            if (cf.HasFlag(CastFlag.RuneInfo))
            {
                var v1 = Reader.ReadByte();
                WriteLine("Cooldowns Before: {0}", (CooldownMask)v1);
                var v2 = Reader.ReadByte();
                WriteLine("Cooldowns Now: {0}", (CooldownMask)v2);

                for (var i = 0; i < 6; ++i)
                {
                    var v3 = (1 << i);

                    if ((v3 & v1) != 0)
                    {
                        if ((v3 & v2) == 0)
                        {
                            var v4 = Reader.ReadByte();
                            WriteLine("Cooldown for {0} is {1}", (CooldownMask)v3, v4);
                        }
                    }
                }
            }

            if (cf.HasFlag(CastFlag.AdjustMissile))
            {
                ReadSingle("Unk");
                UInt32("unk");
            }

            if (cf.HasFlag(CastFlag.Projectile))
            {
                UInt32("Projectile displayid");
                UInt32("inventoryType");
            }

            if (cf.HasFlag(CastFlag.VisualChain))
            {
                UInt32("unk");
                UInt32("unk");
            }

            if (tf.HasFlag(TargetFlags.TARGET_FLAG_DEST_LOCATION))
            {
                AppendFormatLine("targetFlags & 0x40: byte {0}", Reader.ReadByte());
            }
        }

        [Parser(OpCodes.SMSG_SET_PCT_SPELL_MODIFIER)]
        [Parser(OpCodes.SMSG_SET_FLAT_SPELL_MODIFIER)]
        public void HandleSetSpellModifier(Parser packet)
        {
            UInt32("Mods count");
            var count = UInt32("Groups count");
            ReadEnum<SpellMod>("Modifier");

            for (int i = 0; i < count; i++)
            {
                UInt8("  Spell group");
                ReadSingle("  Value");
            }
        }

        public void ReadAuraUpdateBlock(Parser packet)
        {
            ReadByte("Slot");
            
            var id = packet.ReadInt32();
            WriteLine("Spell: " + id);

            if (id <= 0)
                return;

            var flags = (AuraFlag)packet.ReadByte();
            WriteLine("Flags: " + flags);

            Byte("unk");
            Byte("Level");
            Byte("Charges");

            if (!flags.HasFlag(AuraFlag.NotCaster))
                ReadPackedGuid("Caster GUID");

            if (!flags.HasFlag(AuraFlag.Duration))
                return;

            UInt32("Max Duration");
            UInt32("Duration");
        }

        public TargetFlags ReadTargets()
        {
            var tf = (TargetFlags)Reader.ReadUInt32();
            AppendFormatLine("TargetFlags: {0}", tf);

            if (tf.HasFlag(TargetFlags.TARGET_FLAG_UNIT) ||
                tf.HasFlag(TargetFlags.TARGET_FLAG_PVP_CORPSE) ||
                tf.HasFlag(TargetFlags.TARGET_FLAG_OBJECT) ||
                tf.HasFlag(TargetFlags.TARGET_FLAG_CORPSE) ||
                tf.HasFlag(TargetFlags.TARGET_FLAG_UNK2))
            {
                AppendFormatLine("ObjectTarget: 0x{0:X16}", Reader.ReadPackedGuid());
            }

            if (tf.HasFlag(TargetFlags.TARGET_FLAG_ITEM) ||
                tf.HasFlag(TargetFlags.TARGET_FLAG_TRADE_ITEM))
            {
                AppendFormatLine("ItemTarget: 0x{0:X16}", Reader.ReadPackedGuid());
            }

            if (tf.HasFlag(TargetFlags.TARGET_FLAG_SOURCE_LOCATION))
            {
                AppendFormatLine("SrcTargetGuid: {0}", Reader.ReadPackedGuid().ToString("X16"));
                AppendFormatLine("SrcTarget: {0} {1} {2}", Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle());
            }

            if (tf.HasFlag(TargetFlags.TARGET_FLAG_DEST_LOCATION))
            {
                AppendFormatLine("DstTargetGuid: {0}", Reader.ReadPackedGuid().ToString("X16"));
                AppendFormatLine("DstTarget: {0} {1} {2}", Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle());
            }

            if (tf.HasFlag(TargetFlags.TARGET_FLAG_STRING))
            {
                AppendFormatLine("StringTarget: {0}", Reader.ReadCString());
            }

            return tf;
        }

        public void ReadSpellGoTargets()
        {
            var hitCount = Reader.ReadByte();

            for (var i = 0; i < hitCount; ++i)
            {
                AppendFormatLine("GO Hit Target {0}: 0x{1:X16}", i, Reader.ReadUInt64());
            }

            var missCount = Reader.ReadByte();

            for (var i = 0; i < missCount; ++i)
            {
                AppendFormatLine("GO Miss Target {0}: 0x{1:X16}", i, Reader.ReadUInt64());
                var missReason = Reader.ReadByte();
                AppendFormatLine("GO Miss Reason {0}: {1}", i, missReason);
                if (missReason == 11) // reflect
                {
                    AppendFormatLine("GO Reflect Reason {0}: {1}", i, Reader.ReadByte());
                }
            }
        }
        
    }

}