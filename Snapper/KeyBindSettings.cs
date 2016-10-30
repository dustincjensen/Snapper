using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Snapper
{    
    public static class KeyBindSettings
    {
        public static Tuple<ModifierKeys, Key> GetSetting(KeyBindEnum purpose)
        {
            var keyCombo = (string)Properties.Settings.Default[purpose.ToString()];        
            var keyStrings = keyCombo.Split(',');
            var possibleKey = Key.None;
            var modifierKeys = ModifierKeys.None;

            foreach (var keyString in keyStrings)
            {
                Key tmpKey;
                ModifierKeys tmpModifier;

                Enum.TryParse(keyString, out tmpKey);
                Enum.TryParse(keyString, out tmpModifier);

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
                ? _GetDefaultKeyBind(purpose)
                // Or the specified one in the settings file                
                : new Tuple<ModifierKeys, Key>(modifierKeys, possibleKey);
        }

        private static Tuple<ModifierKeys, Key> _GetDefaultKeyBind(KeyBindEnum purpose)
        {
            Key key;

            switch (purpose)
            {
                case KeyBindEnum.BottomLeft:
                    key = Key.NumPad1;
                    break;
                case KeyBindEnum.Bottom:
                    key = Key.NumPad2;
                    break;
                case KeyBindEnum.BottomRight:
                    key = Key.NumPad3;
                    break;
                case KeyBindEnum.Left:
                    key = Key.NumPad4;
                    break;
                case KeyBindEnum.Mid:
                    key = Key.NumPad5;
                    break;
                case KeyBindEnum.Right:
                    key = Key.NumPad6;
                    break;
                case KeyBindEnum.TopLeft:
                    key = Key.NumPad7;
                    break;
                case KeyBindEnum.Top:
                    key = Key.NumPad8;
                    break;
                case KeyBindEnum.TopRight:
                    key = Key.NumPad9;
                    break;
                case KeyBindEnum.SwapMonitor:
                    key = Key.NumPad0;
                    break;
                default:
                    key = Key.NumPad0;
                    break;
            }

            return new Tuple<ModifierKeys, Key>(ModifierKeys.Control | ModifierKeys.Alt, key);
        }

        public static void SetSetting(string value, KeyBindEnum purpose)
        {
            switch (purpose)
            {
                case KeyBindEnum.BottomLeft:
                    Properties.Settings.Default.BottomLeft = value;
                    break;
                case KeyBindEnum.Bottom:
                    Properties.Settings.Default.Bottom = value;
                    break;
                case KeyBindEnum.BottomRight:
                    Properties.Settings.Default.BottomRight = value;
                    break;
                case KeyBindEnum.Left:
                    Properties.Settings.Default.Left = value;
                    break;
                case KeyBindEnum.Mid:
                    Properties.Settings.Default.Mid = value;
                    break;
                case KeyBindEnum.Right:
                    Properties.Settings.Default.Right = value;
                    break;
                case KeyBindEnum.TopLeft:
                    Properties.Settings.Default.TopLeft = value;
                    break;
                case KeyBindEnum.Top:
                    Properties.Settings.Default.Top = value;
                    break;
                case KeyBindEnum.TopRight:
                    Properties.Settings.Default.TopRight = value;
                    break;
                case KeyBindEnum.SwapMonitor:
                    Properties.Settings.Default.SwapMonitor = value;
                    break;
            }
            Properties.Settings.Default.Save();
        }
    }
}
