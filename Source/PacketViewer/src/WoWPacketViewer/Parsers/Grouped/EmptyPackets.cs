using System;
using WowTools.Core;

namespace WoWPacketViewer
{
    public class EmptyPackets : Parser
    {
        [Parser(OpCodes.CMSG_CANCEL_TRADE)]
        [Parser(OpCodes.CMSG_WORLD_STATE_UI_TIMER_UPDATE)]
        [Parser(OpCodes.CMSG_CALENDAR_GET_CALENDAR)]
        [Parser(OpCodes.CMSG_ATTACKSTOP)]
        [Parser(OpCodes.CMSG_QUESTGIVER_STATUS_MULTIPLE_QUERY)]
        [Parser(OpCodes.CMSG_READY_FOR_ACCOUNT_DATA_TIMES)]
        [Parser(OpCodes.CMSG_CHAR_ENUM)]
        [Parser(OpCodes.CMSG_REQUEST_RAID_INFO)]
        [Parser(OpCodes.CMSG_GMTICKET_GETTICKET)]
        [Parser(OpCodes.CMSG_CALENDAR_GET_NUM_PENDING)]
        [Parser(OpCodes.CMSG_KEEP_ALIVE)]
        [Parser(OpCodes.CMSG_BATTLEFIELD_STATUS)]
        [Parser(OpCodes.CMSG_MEETINGSTONE_INFO)]
        public void HandleEmptyCMSGPacket(Parser packet)
        {
            packet.WriteLine("CMSG Packet that wants response from server");
        }

        [Parser(OpCodes.SMSG_FORCE_SEND_QUEUED_PACKETS)]
        public void HandleEmptySMSGPacket(Parser packet)
        {
            packet.WriteLine("SMSG Packet that wants response from client");
        }
    }
}
 