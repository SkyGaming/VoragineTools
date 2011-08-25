using System;
using WowTools.Core;



namespace WoWPacketViewer
{
    public class MailHandler : Parser
    {
        [Parser(OpCodes.SMSG_RECEIVED_MAIL)]
        public void HandleReceivedMail(Parser packet)
        {
            var unkInt = packet.ReadInt32();
            WriteLine("Unk Int32: " + unkInt);
        }

        [Parser(OpCodes.CMSG_MAIL_MARK_AS_READ)]
        [Parser(OpCodes.CMSG_MAIL_TAKE_MONEY)]
        public void HandleMailMarkAsRead(Parser packet)
        {
            UInt64("mailbox");
            UInt32("MailID");
            UInt64("Amount");
        }

        [Parser(OpCodes.SMSG_MAIL_LIST_RESULT)]
        public void HandleListResultMail(Parser packet)
        {
            UInt32("RealCount");

            var displayCount = Reader.ReadByte();
            WriteLine("Displayed Count: {0}", displayCount);

            for (var i = 0; i < displayCount; ++i)
            {
                var len = Reader.ReadUInt16();
                var id = Reader.ReadUInt32();
                var type = Reader.ReadByte();

                AppendFormatLine("Message {0}: data len {1}, id {2}, type {3}", i, len, id, type);

                switch (type)
                {
                    case 0:
                        {
                            UInt64("SenderGUID: {0:X16}");
                            break;
                        }
                    default:
                        {
                            UInt32("SenderEntry: {0:X16}");
                            break;
                        }
                }

                UInt64("COD");
                UInt32("Item Text Id");
                UInt32("Stationary");
                UInt64("Money");
                UInt32("Flags");
                ReadSingle("Time");
                UInt32("Template Id");
                CString("Subject");
                CString("Body");

                var itemsCount = Reader.ReadByte();
                WriteLine("Items Count: {0}", itemsCount);

                for (var j = 0; j < itemsCount; ++j)
                {
                    AppendFormatLine("Item: {0}", j);
                    var slot = Reader.ReadByte();
                    var guid = Reader.ReadUInt32();
                    var entry = Reader.ReadUInt32();

                    WriteLine("Slot {0}, guid {1}, entry {2}", slot, guid, entry);

                    for (var k = 0; k < 7; ++k)
                    {
                        var charges = Reader.ReadUInt32();
                        var duration = Reader.ReadUInt32();
                        var enchid = Reader.ReadUInt32();

                        WriteLine("Enchant {0}: charges {1}, duration {2}, id {3}", k, charges, duration, enchid);
                    }

                    UInt32("Random Property");
                    UInt32("Item Suffix Factor");
                    UInt32("Stack Count");
                    UInt32("Spell Charges");
                    var maxDurability = Reader.ReadUInt32();
                    var durability = Reader.ReadUInt32();
                    WriteLine("Durability: {0} (max {1})", durability, maxDurability);
                    Byte("Unk");

                    AppendLine();
                }

                AppendLine();
            }
        }
    }
}
