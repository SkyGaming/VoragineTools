using System;
using System.Text;
using WowTools.Core;

namespace WoWPacketViewer
{
    public class ChannelHandler : Parser
    {
        [Parser(OpCodes.CMSG_CHANNEL_VOICE_ON)]
        [Parser(OpCodes.CMSG_CHANNEL_VOICE_OFF)]
        public void HandleChannelSetVoice(Parser packet)
        {
            CString("ChannelName");
        }

        [Parser(OpCodes.CMSG_CHANNEL_SILENCE_VOICE)]
        [Parser(OpCodes.CMSG_CHANNEL_UNSILENCE_VOICE)]
        [Parser(OpCodes.CMSG_CHANNEL_SILENCE_ALL)]
        [Parser(OpCodes.CMSG_CHANNEL_UNSILENCE_ALL)]
        public void HandleChannelSilencing(Parser packet)
        {
            CString("ChannelName");
            CString("PlayerName");
        }

        [Parser(OpCodes.CMSG_JOIN_CHANNEL)]
        public void HandleChannelJoin(Parser packet)
        {
            UInt8("unk");
            UInt8("unk");
            UInt32("Channel_ID");
            CString("ChannelName");
            CString("Pass");
        }

        [Parser(OpCodes.CMSG_LEAVE_CHANNEL)]
        public void HandleChannelLeave(Parser packet)
        {
            UInt32("Channel_ID");
            CString("ChannelName");
        }
    }
}