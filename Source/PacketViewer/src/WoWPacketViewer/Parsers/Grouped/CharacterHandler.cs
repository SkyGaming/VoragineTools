using System;
using System.Text;
using WowTools.Core;

namespace WoWPacketViewer
{
    public class CharacterHandler : Parser
    {
        public void Characteristics(Parser packet)
        {
            packet.ReadByte("Skin");
            packet.ReadByte("Face");
            packet.ReadByte("HairColor");
            packet.ReadByte("Hair Style");
            packet.ReadByte("Facial Hair");
        }

        [Parser(OpCodes.CMSG_CHAR_CREATE)]
        public void HandleClientCharCreate()
        {
            CString("Name");
            ReadEnum<Race>("Race");
            ReadEnum<Class>("Class");
            ReadEnum<Gender>("Gender");

            Byte("Face");
            Byte("HairStyle");
            Byte("HairColor");
            Byte("FacialHair");
            Byte("OutfitID");
            Byte("unk");
        }

        [Parser(OpCodes.CMSG_CHAR_DELETE)]
        public void HandleClientCharDelete(Parser packet)
        {
            ReadGuid("GUID");
        }

        [Parser(OpCodes.CMSG_CHAR_RENAME)]
        public void HandleClientCharRename(Parser packet)
        {
            ReadGuid("GUID");
            CString("NewName");
        }

        [Parser(OpCodes.SMSG_CHAR_RENAME)]
        public void HandleServerCharRename(Parser packet)
        {
            var result = ReadEnum<ResponseCode>("Response");

            if (result != ResponseCode.RESPONSE_SUCCESS)
                return;

            ReadGuid("GUID");
            CString("Name");
        }

        [Parser(OpCodes.SMSG_CHAR_CREATE)]
        [Parser(OpCodes.SMSG_CHAR_DELETE)]
        public void HandleCharResponse(Parser packet)
        {
            ReadEnum<ResponseCode>("Response");
        }

        [Parser(OpCodes.CMSG_ALTER_APPEARANCE)]
        public void HandleAlterAppearance(Parser packet)
        {
            Byte("HairStyle");
            Byte("HairColor");
            Byte("Facial Hair");
        }

        [Parser(OpCodes.SMSG_BARBER_SHOP_RESULT)]
        public void HandleBarberShopResult(Parser packet)
        {
            var status = (BarberShopResult)packet.ReadInt32();
            WriteLine("Result: " + status);
        }

        [Parser(OpCodes.CMSG_CHAR_CUSTOMIZE)]
        public void HandleClientCharCustomize(Parser packet)
        {
            ReadGuid("GUID");
            CString("NewName");

            ReadEnum<Gender>("Gender");
            Characteristics(packet);
        }

        [Parser(OpCodes.SMSG_CHAR_CUSTOMIZE)]
        public void HandleServerCharCustomize(Parser packet)
        {
            var response = ReadEnum<ResponseCode>("Response");

            if (response != ResponseCode.RESPONSE_SUCCESS)
                return;

            ReadInt64("GUID");
            CString("New Name");

            ReadEnum<Gender>("Gender");
            Characteristics(packet);
        }
    }
}