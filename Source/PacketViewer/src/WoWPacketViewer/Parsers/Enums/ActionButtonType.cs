using System;
using System.Text;
using WowTools.Core;

namespace WoWPacketViewer
{
    public enum ActionButtonType
    {
        Spell = 0,
        Click = 1,
        EquipSet = 32,
        Macro = 64,
        Item = 128
    };
}