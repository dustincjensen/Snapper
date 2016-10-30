using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Hotkeys;
using Snapper.Common;

namespace Snapper.Views
{
    public class KeyBindingsModel : ViewModelBase
    {
        private static bool GLOBAL_EDIT_MODE;
        private readonly Action<KeyBindEnum> _hotKeyAction;
        private readonly KeyBindEnum _purpose;
        private string _display;
        private bool _editingMode;
        private Hotkeys.GlobalHotKey _hotKey;
        private ModifierKeys _modifierKeys = ModifierKeys.None;

        public KeyBindingsModel(string label, Action<KeyBindEnum> hotKeyAction, KeyBindEnum purpose)
        {
            Label = label;
            _hotKeyAction = hotKeyAction;
            _purpose = purpose;

            var settingsKeyBind = KeyBindSettings.GetSetting(purpose);
            if (settingsKeyBind != null && settingsKeyBind.Item2 != Key.None)
            {
                _RegisterHotKey(settingsKeyBind.Item1, settingsKeyBind.Item2);
                Display = _GetDisplay(settingsKeyBind.Item1, settingsKeyBind.Item2);
            }
        }

        public string Label { get; set; } 

        public string Display
        {
            get { return _display; }
            set
            {
                _display = value;
                RaisePropertyChanged();
            }
        }

        private static string _GetDisplay(ModifierKeys modifierKeys, Key key)
        {
            return
                (modifierKeys != ModifierKeys.None ? modifierKeys.ToString().Replace(",", "+").Replace(" ", "") + "+" : "") +
                (key != Key.None ? key.ToString() : "");
        }

        public bool EditingMode
        {
            get { return _editingMode; }
            set
            {
                _editingMode = GLOBAL_EDIT_MODE = value;
                RaisePropertyChanged();
            }
        }

        public void HandleClick()
        {
            if (GLOBAL_EDIT_MODE) return;

            EditingMode = true;
            if (_hotKey != null)
            {
                KeyBindSettings.SetSetting("", _purpose);
                _hotKey.UnregisterHotKey();
                _hotKey.HotKeyPressed -= _HotKeyOnHotKeyPressed;
            }
            Display = "";
        }

        public void HandleKeyModifiers()
        {
            if (!EditingMode) return;
            _modifierKeys = Keyboard.Modifiers;
            Display = _GetDisplay(_modifierKeys, Key.None);
        }

        public void HandleKeyBind(KeyEventArgs e)
        {
            if (!EditingMode) return;

            // key is your real pressed key
            var key = e.Key != Key.System ? e.Key : e.SystemKey;

            if (key == Key.LeftCtrl || key == Key.RightCtrl ||
                key == Key.LeftAlt || key == Key.RightAlt ||
                key == Key.LeftShift || key == Key.RightShift)
            {
                return;
            }
           
            try
            {                
                _RegisterHotKey(_modifierKeys, key);
                Display = _GetDisplay(_modifierKeys, key);
                KeyBindSettings.SetSetting(_modifierKeys + "," + key, _purpose);
            }
            catch (ApplicationException)
            {
                Display = "Error: Taken";
                return;
            }

            EditingMode = false;
        }

        private void _RegisterHotKey(ModifierKeys modifierKeys, Key key)
        {
            _hotKey = new GlobalHotKey(modifierKeys, key, System.Windows.Application.Current.MainWindow);
            _hotKey.HotKeyPressed += _HotKeyOnHotKeyPressed;
        }        

        private void _HotKeyOnHotKeyPressed(GlobalHotKey globalHotKey)
        {
            if (!GLOBAL_EDIT_MODE)
            {
                _hotKeyAction.Invoke(_purpose);
            }
        }
    }
}
