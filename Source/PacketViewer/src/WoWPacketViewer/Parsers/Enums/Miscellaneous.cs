using System.Text;
using WowTools.Core;

namespace WoWPacketViewer
{
    public enum RealmSplitState
    {
        Normal = 0,
        Split = 1,
        Pending = 2
    }

    public enum WeatherState
    {
        Fine = 0,
        LightRain = 3,
        MediumRain = 4,
        HeavyRain = 5,
        LightSnow = 6,
        MediumSnow = 7,
        HeavySnow = 8,
        LightSandstorm = 22,
        MediumSandstorm = 41,
        HeavySandstorm = 42,
        Thunder = 86,
        BlackRain = 90
    }
}