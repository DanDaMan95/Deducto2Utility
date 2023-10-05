using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Deducto2Utility
{

    class PropertyChecking
    {
        public int PropertyName { get; set; }
    }

    public class SliderInfo
    {
        public int Value { get; set; }
        public bool IsMax { get; set; }
    }

    /// <summary>
    /// Provides utility methods for logging and dictionary operations.
    /// </summary>
    public class Stuff
    {
        /// <summary>
        /// Enum representing ANSI color codes for use in logging.
        /// Basically settings colors using a method Mr.Dumb doesnt know how to use.
        /// </summary>
        public enum EasyLogColors
        {
            /// <summary>Reset text color to default.</summary>
            Reset = 0,
            /// <summary>Set text color to black.</summary>
            Black = 30,
            /// <summary>Set text color to red.</summary>
            Red = 31,
            /// <summary>Set text color to green.</summary>
            Green = 32,
            /// <summary>Set text color to yellow.</summary>
            Yellow = 33,
            /// <summary>Set text color to blue.</summary>
            Blue = 34,
            /// <summary>Set text color to magenta.</summary>
            Magenta = 35,
            /// <summary>Set text color to cyan.</summary>
            Cyan = 36,
            /// <summary>Set text color to white.</summary>
            White = 37
        }

        /// <summary>
        /// Logs a message with the specified ANSI color code.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="color">The ANSI color code to apply to the message.</param>
        public static void EasyLog(string message, EasyLogColors color)
        {
            int colorCode = (int)color;
            string coloredMessage = $"\u001b[{colorCode}m{message}\u001b[0m"; // Apply ANSI color code
            Melon<Core>.Logger.Msg(coloredMessage);
        }

        /// <summary>
        /// Logs an error message in red text.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        public static void EasyErr(string message)
        {
            string coloredMessage = $"\u001b[{EasyLogColors.Red}m{message}\u001b[0m"; // Apply ANSI color code
            Melon<Core>.Logger.Msg(coloredMessage);
        }

        /// <summary>
        /// Gets the value from the dictionary by key.
        /// </summary>
        /// <param name="dictionary">The dictionary to search.</param>
        /// <param name="key">The key to search for.</param>
        /// <returns>The value associated with the key, or -1 if the key is not found.</returns>
        public static string GetValueByKey(Dictionary<string, string> dictionary, string key)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            else
            {
                return null;
            }
        }
        public static PropertyInfo FindPropertyByName(Type type, string propertyName)
        {
            PropertyInfo property = type.GetProperty(propertyName);

            if (property != null)
            {
                Console.WriteLine($"Found property with name: {property.Name}");
            }
            else
            {
                Console.WriteLine($"Property with name {propertyName} not found.");
            }

            return property;
        }
        public static bool TrySetProperty<T>(object obj, string propertyName, T value)
        {
            Type type = obj.GetType();
            PropertyInfo property = type.GetProperty(propertyName);

            if (property != null && property.PropertyType == typeof(T))
            {
                property.SetValue(obj, value);
                return true;
            }
            else
            {
                Debug.LogError($"Property '{propertyName}' not found or is not of type {typeof(T)}.");
                return false;
            }
        }

    }
}
