using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deducto2Utility
{
    public class Stuff
    {
        public enum EasyLogColors
        {
            Reset = 0,
            Black = 30,
            Red = 31,
            Green = 32,
            Yellow = 33,
            Blue = 34,
            Magenta = 35,
            Cyan = 36,
            White = 37
        }

        public static void EasyLog(string message, EasyLogColors color)
        {
            int colorCode = (int)color;
            string coloredMessage = $"\u001b[{colorCode}m{message}\u001b[0m"; // Apply ANSI color code
            Melon<Core>.Logger.Msg(coloredMessage);
        }

    }
}
