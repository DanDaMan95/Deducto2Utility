using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deducto2Utility
{
    public class WeaponData
    {
        public int Range { get; set; }
        public int RateOfUse { get; set; }

        public WeaponData(int range, int rateOfUse)
        {
            Range = range;
            RateOfUse = rateOfUse;
        }
    }

    public class Dicts
    {
        public static Dictionary<string, string> RoleStrings = new Dictionary<string, string>
        {
            { "ImposterData", "Kill all Co-workers, also press '4' on your keyboard. Thank me later." },
            { "CoworkerData", "I am pretty sure anyone else using this cheat is walling, Become friends with them." },
            { "InfectedData", "Haha, L + Bozo + Ratio, Anyway, go touch others or something." },
            { "JudgeData", "JUDGE JUDY ??!!! Don't be rude okay...?" },
            { "SecurityData", "Kill literally anyone but you'll be voted out. Kill Imposters" },
            { "SupervisorData", "You wote fow the pwecious pweople. It's aww about Women's Suffewage !! OwO UwU ^w^" },
            { "Patrick", "What's up!!"}
        };

        public static Dictionary<string, WeaponData> Weapons = new Dictionary<string, WeaponData>
        {
            { "Knife01Data", new WeaponData(int.MaxValue, 0) },
            { "NailGunData", new WeaponData(int.MaxValue, 0) },
            { "Pistol01Data", new WeaponData(int.MaxValue, 0) },
            { "StaplerData", new WeaponData(int.MaxValue, 0) },
        };

        public static Dictionary<string, SliderInfo> Sliders = new Dictionary<string, SliderInfo>
        {
            { "MaxFPSSlider", new SliderInfo { Value = -1, IsMax = false } },
            { "Slider_01", new SliderInfo { Value = 300, IsMax = true } }
        };
    }
}
