using WowTools.Core;

namespace WoWPacketViewer
{
    public class GuildHandler : Parser
    {
        [Parser(OpCodes.CMSG_GUILD_QUERY)]
        public void HandleGuildQuery(Parser packet)
        {
            UInt64("GuildID");
            ReadPackedGuid("PlayerGuid");
        }

        [Parser(OpCodes.CMSG_GUILD_PROMOTE)]
        [Parser(OpCodes.CMSG_GUILD_REMOVE)]
        [Parser(OpCodes.CMSG_GUILD_DEMOTE)]
        public void HandleGuildRemove(Parser packet)
        {
            UInt64("Guid");
            UInt64("unk(uint64)");
        }

        [Parser(OpCodes.CMSG_GUILD_DECLINE)]
        [Parser(OpCodes.CMSG_GUILD_ACCEPT)]
        public void HandleGuildAccept(Parser packet)
        {
            UInt64("unk(uint64)");
        }

        [Parser(OpCodes.CMSG_GUILD_INFO_TEXT)]
        public void HandleGuildInfoText(Parser packet)
        {
            UInt64("unk(uint64)");
            UInt64("unk(uint64)");
            CString("Info");
        }

        [Parser(OpCodes.CMSG_GUILD_MOTD)]
        public void HandleGuildMOTD(Parser packet)
        {
            UInt64("unk(uint64)");
            UInt64("unk(uint64)");
            CString("motd");
        }

        [Parser(OpCodes.CMSG_QUERY_GUILD_XP)]
        public void HandleGuildQueryXP(Parser packet)
        {
            UInt64("MaxDailyXP");
            UInt64("NextLevelXP");
            UInt64("WeeklyXP");
            UInt64("CurrentXP");
            UInt64("TodayXP");
            CString("motd");
        }

        [Parser(OpCodes.SMSG_GUILD_MAX_DAILY_XP)]
        public void HandleGuildMaxDailyXP(Parser packet)
        {
            UInt64("MaxDailyXP)");
        }

        [Parser(OpCodes.CMSG_QUERY_GUILD_REWARDS)]
        public void HandleGuildRewards(Parser packet)
        {
            UInt64("unk(uint64)");
            UInt32("GuidlID");
            var count = UInt32("Count");

            for (int i = 0; i < count; i++)
                UInt32("unk(uint32)");

            for (int i = 0; i < count; i++)
                UInt32("unk(uint32)");

            for (int i = 0; i < count; i++)
                UInt64("MoneyPrice");

            for (int i = 0; i < count; i++)
                UInt32("AchievementRequirement");

            for (int i = 0; i < count; i++)
                UInt32("ReputationLevel");

            for (int i = 0; i < count; i++)
                UInt32("ItemEntry");
        }

        [Parser(OpCodes.CMSG_GUILD_ADD_RANK)]
        public void HandleGuildAddRank(Parser packet)
        {
            CString("RankName");
        }

        [Parser(OpCodes.CMSG_GUILD_DEL_RANK)]
        public void HandleGuildDelRank(Parser packet)
        {
            UInt32("RankID");
        }

        [Parser(OpCodes.CMSG_GUILD_BANK_WITHDRAW_MONEY)]
        [Parser(OpCodes.CMSG_GUILD_BANK_DEPOSIT_MONEY)]
        public void HandleGuildBankDepositMoney(Parser packet)
        {
            UInt64("Guid)");
            UInt64("Money)");
        }

        [Parser(OpCodes.CMSG_GUILD_BANK_UPDATE_TAB)]
        public void HandleGuildBankUpdateTab(Parser packet)
        {
            UInt64("Guid)");
            CString("TabID");
            CString("Name");
            CString("Icon");
        }
    }
}
