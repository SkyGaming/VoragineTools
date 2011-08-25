namespace WoWPacketViewer
{
    enum SpellMod : byte
    {
        Damage = 0,
        Duration = 1,
        Threat = 2,
        Effect1 = 3,
        Charges = 4,
        Range = 5,
        Radius = 6,
        CriticalChance = 7,
        AllEffects = 8,
        NotLoseCastingTime = 9,
        CastingTime = 10,
        Cooldown = 11,
        Effect2 = 12,
        IgnoreArmor = 13,
        Cost = 14,
        CritDamageBonus = 15,
        ResistMissChance = 16,
        JumpTargets = 17,
        ChanceOfSuccess = 18,
        ActivationTime = 19,
        DamageMultiplier = 20,
        GlobalCooldown = 21,
        Dot = 22,
        Effect3 = 23,
        BonusMultiplier = 24,
        // Spellmod 25
        ProcPerMinute = 26,
        ValueMultiplier = 27,
        ResistDispelChance = 28,
        CritDamageBonus2 = 29, //One Not Used Spell
        SpellCostRefundOnFail = 30
    }
}
