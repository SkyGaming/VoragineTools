using System;
using System.Text;
using WowTools.Core;

namespace WoWPacketViewer
{
    public class AchievementHandler : Parser
    {
        [Parser(OpCodes.SMSG_ACHIEVEMENT_DELETED)]
        [Parser(OpCodes.SMSG_CRITERIA_DELETED)]
        public void HandleDeleted(Parser packet)
        {
            UInt32("ID");
        }

        [Parser(OpCodes.SMSG_SERVER_FIRST_ACHIEVEMENT)]
        public void HandleServerFirstAchievement(Parser packet)
        {
            CString("PlayerName");
            ReadGuid("PlayerGUID");
            UInt32("Achievement");
            UInt32("LinkedName");
        }

        [Parser(OpCodes.SMSG_ACHIEVEMENT_EARNED)]
        public void HandleAchievementEarned(Parser packet)
        {
            ReadPackedGuid("PlayerGUID");
            UInt32("Achievement");
            PackedTime("Time");
            UInt32("unk");
        }

        [Parser(OpCodes.SMSG_CRITERIA_UPDATE)]
        public void HandleCriteriaUpdate(Parser packet)
        {
            UInt32("CriteriaID");
            ReadPackedGuid("CriteriaCounter");
            ReadPackedGuid("PlayerGUID");
            UInt32("unk");
            ReadPackedTime("CriteriaTime");

            for (var i = 0; i < 2; i++)
                UInt32("Timer");
        }

        [Parser(OpCodes.SMSG_ALL_ACHIEVEMENT_DATA)]
        public void HandleAllAchievementData(Parser packet)
        {
            UInt32("Achievements");

            //FINISH THIS
        }

        [Parser(OpCodes.CMSG_QUERY_INSPECT_ACHIEVEMENTS)]
        public void HandleInspectAchievementData(Parser packet)
        {
            ReadPackedGuid("GUID");
        }

        [Parser(OpCodes.SMSG_RESPOND_INSPECT_ACHIEVEMENTS)]
        public void HandleInspectAchievementDataResponse(Parser packet)
        {
            ReadPackedGuid("PlayerGUID");

            UInt32("AchievementID");
            ReadPackedTime("AchievementTime");

            UInt32("CriteriaID");
            ReadPackedGuid("CriteriaCounter");
            ReadPackedGuid("PlayerGUID");
            UInt32("unk");
            PackedTime("CriteriaTime");

            for (var i = 0; i < 2; i++)
            {
                UInt32("Timer");
            }
        }
    }
}