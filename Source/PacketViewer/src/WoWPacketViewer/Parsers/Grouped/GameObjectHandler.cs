using System;
using WowTools.Core;





namespace WoWPacketViewer
{
    public class GameObjectHandler : QueryHandler
    {
        [Parser(OpCodes.CMSG_GAMEOBJECT_QUERY)]
        public void HandleGameObjectQuery(Parser packet)
        {
            ReadQueryHeader(packet);
        }

        [Parser(OpCodes.SMSG_GAMEOBJECT_QUERY_RESPONSE)]
        public void HandleGameObjectQueryResponse(Parser packet)
        {
            var entry = packet.ReadEntry();
            WriteLine("Entry: " + entry.Key);

            if (entry.Value)
                return;

            var type = (GameObjectType)packet.ReadInt32();
            WriteLine("Type: " + type);

            var dispId = packet.ReadInt32();
            WriteLine("Display ID: " + dispId);

            var name = new string[4];
            for (var i = 0; i < 4; i++)
            {
                name[i] = packet.ReadCString();
                WriteLine("Name " + i + ": " + name[i]);
            }

            var iconName = packet.ReadCString();
            WriteLine("Icon Name: " + iconName);

            var castCaption = packet.ReadCString();
            WriteLine("Cast Caption: " + castCaption);

            var unkStr = packet.ReadCString();
            WriteLine("Unk String: " + unkStr);

            var data = new int[24];
            for (var i = 0; i < 24; i++)
            {
                data[i] = packet.ReadInt32();
                WriteLine("Data " + i + ": " + data[i]);
            }

            var size = packet.ReadSingle();
            WriteLine("Size: " + size);

            var qItem = new int[6];
            for (var i = 0; i < 6; i++)
            {
                qItem[i] = packet.ReadInt32();
                WriteLine("Quest Item " + i + ": " + qItem[i]);
            }

            //SQLStore.WriteData(SQLStore.GameObjects.GetCommand(entry.Key, type, dispId, name[0], iconName,
            //    castCaption, unkStr, data, size, qItem));
        }

        [Parser(OpCodes.SMSG_DESTRUCTIBLE_BUILDING_DAMAGE)]
        public void HandleDestructibleBuildingDamage(Parser packet)
        {
            var goGuid = packet.ReadPackedGuid();
            WriteLine("GO GUID: " + goGuid);

            var vehGuid = packet.ReadPackedGuid();
            WriteLine("Vehicle GUID: " + vehGuid);

            var plrGuid = packet.ReadPackedGuid();
            WriteLine("Player GUID: " + plrGuid);

            var dmg = packet.ReadInt32();
            WriteLine("Damage: " + dmg);

            var spellId = packet.ReadInt32();
            WriteLine("Spell ID: " + spellId);
        }

        [Parser(OpCodes.SMSG_GAMEOBJECT_DESPAWN_ANIM)]
        [Parser(OpCodes.CMSG_GAMEOBJ_USE)]
        [Parser(OpCodes.CMSG_GAMEOBJ_REPORT_USE)]
        public void HandleGOMisc(Parser packet)
        {
            var guid = packet.ReadGuid();
            WriteLine("GUID: " + guid);
        }

        [Parser(OpCodes.SMSG_GAMEOBJECT_CUSTOM_ANIM)]
        public void HandleGOCustomAnim(Parser packet)
        {
            var guid = packet.ReadGuid();
            WriteLine("GUID: " + guid);

            var anim = packet.ReadInt32();
            WriteLine("Anim: " + anim);
        }
    }
}
