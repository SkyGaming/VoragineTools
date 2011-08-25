using System;

namespace WoWPacketViewer
{
    [Flags]
    public enum CastFlag
    {
        None = 0x00000000,
        PendingCast = 0x00000001, // 4.x NoCombatLog
        Unknown1 = 0x00000002,
        Unknown2 = 0x00000004,
        Unknown3 = 0x00000008,
        Unknown4 = 0x00000010,
        Projectile = 0x00000020,
        Unknown5 = 0x00000040,
        Unknown6 = 0x00000080,
        Unknown7 = 0x00000100,
        Unknown8 = 0x00000200,
        Unknown9 = 0x00000400,
        PredictedPower = 0x00000800,
        Unknown10 = 0x00001000,
        Unknown11 = 0x00002000,
        Unknown12 = 0x00004000,
        Unknown13 = 0x00008000,
        Unknown14 = 0x00010000,
        AdjustMissile = 0x00020000, // 4.x
        Unknown16 = 0x00040000,
        VisualChain = 0x00080000, // 4.x
        Unknown18 = 0x00100000,
        RuneInfo = 0x00200000, // 4.x PredictedRunes
        Unknown19 = 0x00400000,
        Unknown20 = 0x00800000,
        Unknown21 = 0x01000000,
        Unknown22 = 0x02000000,
        Immunity = 0x04000000 // 4.x
    }
}
