using System;
using System.Text;
using WowTools.Core;

namespace WoWPacketViewer
{
    public class ActionBarHandler : Parser
    {
        [Parser(OpCodes.SMSG_ACTION_BUTTONS)]
        public void HandleInitialButtons()
        {
            Byte("TalentSpec");

            for (var i = 0; i < 144; i++)
            {
                var packed = Int32();

                if (packed == 0)
                    continue;
                
                var action = packed & 0x00FFFFFF;

                var type = (ActionButtonType)((packed & 0xFF000000) >> 24);
                WriteLine("{0}: {1} - {2}", i, type, action);
            }
        }
    }
}