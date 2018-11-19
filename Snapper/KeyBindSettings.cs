using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Snapper
{
    public static class KeyBindSettings
    {
        public static Tuple<ModifierKeys, Key> GetSetting(KeyBindEnum keyBind)
        {
            var keyCombo = (string)Properties.Settings.Default[keyBind.ToString()];        
            var keyStrings = keyCombo.Split(',');
            var possibleKey = Key.None;
            var modifierKeys = ModifierKeys.None;

            foreach (var keyString in keyStrings)
            {
                Enum.TryParse(keyString, out Key tmpKey);
                Enum.TryParse(keyString, out ModifierKeys tmpModifier);

                if (tmpKey != Key.None)
                {
                    possibleKey = tmpKey;
                }
                else if (tmpModifier != ModifierKeys.None)
                {
                    modifierKeys = modifierKeys | tmpModifier;
                }
            }

            return (possibleKey == Key.None && modifierKeys == ModifierKeys.None)
                // Return a default for this keybind.
                ? GetDefaultKeyBind(keyBind)
                // Or the specified one in the settings file                
                : new Tuple<ModifierKeys, Key>(modifierKeys, possibleKey);
        }

        private static readonly Dictionary<KeyBindEnum, Key> DefaultKeyBindings = new Dictionary<KeyBindEnum, Key>
        {
            { KeyBindEnum.BottomLeft, Key.NumPad1 },
            { KeyBindEnum.Bottom, Key.NumPad2 },
            { KeyBindEnum.BottomRight, Key.NumPad3 },
            { KeyBindEnum.Left, Key.NumPad4 },
            { KeyBindEnum.Mid, Key.NumPad5 },
            { KeyBindEnum.Right, Key.NumPad6 },
            { KeyBindEnum.TopLeft, Key.NumPad7 },
            { KeyBindEnum.Top, Key.NumPad8 },
            { KeyBindEnum.TopRight, Key.NumPad9 },
            { KeyBindEnum.SwapMonitor, Key.NumPad0 }
        };

        private static Tuple<ModifierKeys, Key> GetDefaultKeyBind(KeyBindEnum keyBind)
        {
            var key = DefaultKeyBindings[keyBind];
            return new Tuple<ModifierKeys, Key>(ModifierKeys.Control | ModifierKeys.Alt, key);
        }

        private static readonly Dictionary<KeyBindEnum, string> KeyBindToSettingMap = new Dictionary<KeyBindEnum, string>
        {
            { KeyBindEnum.BottomLeft, "BottomLeft" },
            { KeyBindEnum.Bottom, "Bottom" },
            { KeyBindEnum.BottomRight, "BottomRight" },
            { KeyBindEnum.Left, "Left" },
            { KeyBindEnum.Mid, "Mid" },
            { KeyBindEnum.Right, "Right" },
            { KeyBindEnum.TopLeft, "TopLeft" },
            { KeyBindEnum.Top, "Top" },
            { KeyBindEnum.TopRight, "TopRight" },
            { KeyBindEnum.SwapMonitor, "SwapMonitor" }
        };

        public static void SetSetting(string value, KeyBindEnum keyBind)
        {
            var propertyToSet = KeyBindToSettingMap[keyBind];
            Properties.Settings.Default[propertyToSet] = value;
            Properties.Settings.Default.Save();
        }
    }
}
