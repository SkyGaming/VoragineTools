using System;

namespace WowTools.Core
{
    [Flags]
    public enum HighGuidMask
    {
        None = 0x00,
        Flag01 = 0x01,
        Flag02 = 0x02,
        Flag04 = 0x04,
        Flag08 = 0x08,
        Flag10 = 0x10,
        Flag20 = 0x20,
        Flag40 = 0x40,
        Flag80 = 0x80,
        Flag100 = 0x100
    }

    public enum HighGuidType
    {
        Player1 = 0, // NoEntry1
        GameObject = 1,
        Transport = 2,
        Unit = 3,
        Pet = 4,
        Vehicle = 5,
        Unknown1 = 6,
        Unknown2 = 7,
        Player2 = 8, // NoEntry2
        Unknown3 = 9,
        Unknown4 = 10,
        Unknown5 = 11,
        MOTransport = 12
    }

    public struct Guid
    {
        public readonly ulong Full;

        public Guid(ulong id)
            : this()
        {
            Full = id;
        }

        public bool HasEntry()
        {
            switch (GetHighType())
            {
                case HighGuidType.Player1:
                case HighGuidType.Player2:
                {
                    return false;
                }
            }

            return true;
        }

        public ulong GetLow()
        {
            switch (GetHighType())
            {
                case HighGuidType.Player1:
                case HighGuidType.Player2:
                {
                    return (Full & 0x000FFFFFFFFFFFFF) >> 0;
                }
                case HighGuidType.GameObject:
                case HighGuidType.Transport:
                case HighGuidType.MOTransport:
                {
                    return (Full & 0x0000000000FFFFFF) >> 0;
                }
            }

            return (Full & 0x00000000FFFFFFFF) >> 0;
        }

        public uint GetEntry()
        {
            if (!HasEntry())
                return 0;

            return (uint)((Full & 0x000FFFFF00000000) >> 32);
        }

        public HighGuidType GetHighType()
        {
            return (HighGuidType)((Full & 0x00F0000000000000) >> 52);
        }

        public HighGuidMask GetHighMask()
        {
            return (HighGuidMask)((Full & 0xFF00000000000000) >> 56);
        }

        public static bool operator ==(Guid first, Guid other)
        {
            return first.Full == other.Full;
        }

        public static bool operator !=(Guid first, Guid other)
        {
            return !(first == other);
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj is Guid && Equals((Guid)obj);
        }

        public bool Equals(Guid other)
        {
            return other.Full == Full;
        }

        public override int GetHashCode()
        {
            return Full.GetHashCode();
        }

        public override string ToString()
        {
            return "0x" + Full.ToString("X8") + " Flags: " + GetHighMask() + "\nType: " +
                GetHighType() + "\nEntry: " + GetEntry() + "\nLow: " + GetLow();
        }
    }
}
