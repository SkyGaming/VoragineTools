using System;
using WowTools.Core;


namespace WoWPacketViewer
{
    public class PetHandler : Parser
    {
        /*[Parser(OpCodes.SMSG_PET_SPELLS)]
        public void HandlePetSpells(Parser packet)
        {
            var guid64 = packet.ReadUInt64();
            if (guid64 == 0) // Sent when player leaves vehicle - empty packet
                return;

            var guid = new WowTools.Core.Guid(guid64);
            WriteLine("GUID: " + guid);
            var isPet = guid.GetHighType() == HighGuidType.Pet;

            var family = (CreatureFamily)packet.ReadUInt16();
            WriteLine("Pet Family: " + family); // vehicles -> 0

            var unk1 = packet.ReadUInt32(); // 0
            WriteLine("Unknown 1: " + unk1);

            var reactState = packet.ReadByte(); // 1
            WriteLine("React state: " + reactState);

            var commandState = packet.ReadByte(); // 1
            WriteLine("Command state: " + commandState);

            var unk2 = packet.ReadUInt16(); // pets -> 0, vehicles -> 0x800 (2048)
            WriteLine("Unknow 2: " + unk2);

            for (var i = 1; i <= (int)MiscConstants.CreatureMaxSpells + 2; i++) // Read pet/vehicle spell ids
            {
                var spell16 = packet.ReadUInt16();
                var spell8 = packet.ReadByte();
                var slotid = packet.ReadInt8();
                var spellId = spell16 | spell8;
                if (!isPet) // cleanup vehicle spells (start at 1 instead 8,
                {           // and do not print spells with id 0)
                    slotid -= (int) MiscConstants.PetSpellsOffset - 1;
                    if (spellId == 0)
                        continue;
                }
                WriteLine("Spell " + slotid + ": " + spellId);
            }

            var spellCount = packet.ReadByte(); // vehicles -> 0, pets -> != 0. Could this be auras?
            WriteLine("Spell count: " + spellCount);

            for (var i = 0; i < spellCount; i++)
            {
                var spellId = packet.ReadUInt16();
                var active = packet.ReadInt16();
                WriteLine("Spell " + i + ": " + spellId + ", active: " + active);
            }

            var cdCount = packet.ReadByte();
            WriteLine("Cooldown count: " + cdCount);

            for (var i = 0; i < cdCount; i++)
            {
                var spellId = packet.ReadInt32();
                var category = packet.ReadUInt16();
                var cooldown = packet.ReadUInt32();
                var categoryCooldown = packet.ReadUInt32();

                WriteLine("Cooldown: Spell: " + (int)spellId + " category: " + category +
                    " cooldown: " + cooldown + " category cooldown: " + categoryCooldown);
            }
        }*/

        [Parser(OpCodes.SMSG_PET_NAME_QUERY_RESPONSE)]
        public void HandlePetNameQueryResponce(Parser packet)
        {
            var petNumber = packet.ReadInt32();
            WriteLine("Pet number: " + petNumber);

            var petName = packet.ReadCString();
            if (petName == string.Empty)
            {
                packet.ReadBytes(7); // 0s
                return;
            }
            WriteLine("Pet name: " + petName);

            var time = packet.ReadTime();
            WriteLine("Time: " + time);

            var declined = packet.ReadBoolean();
            WriteLine("Declined: " + declined);

            if (declined)
                for (var i = 0; i < (int)MiscConstants.MaxDeclinedNameCases; i++)
                    WriteLine("Declined name " + i + ": " + packet.ReadCString());
        }

        [Parser(OpCodes.MSG_LIST_STABLED_PETS)]
        public void HandleListStabledPets(Parser packet)
        {
            ReadGuid("VendorEntityId");

            var NumPets = Byte("NumPets");
            Byte("NumOwnedSlots");

            for (byte i = 0; i < NumPets; i++)
            {
                UInt32("PetIndex(?)");
                UInt32("PetNumber");
                UInt32("PetEntryID");
                UInt32("PetLevel");
                CString("PetName");
                Byte("SlotNum");
            }
        }
    }
}
