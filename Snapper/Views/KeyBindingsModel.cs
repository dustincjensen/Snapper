using System;
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
        private GlobalHotKey _hotKey;
        private ModifierKeys _modifierKeys = ModifierKeys.None;

        public KeyBindingsModel(string label, Action<KeyBindEnum> hotKeyAction, KeyBindEnum purpose)
        {
            Label = label;
            _hotKeyAction = hotKeyAction;
            _purpose = purpose;

            var settingsKeyBind = KeyBindSettings.GetSetting(purpose);
            if (settingsKeyBind != null && settingsKeyBind.Item2 != Key.None)
            {
                RegisterHotKey(settingsKeyBind.Item1, settingsKeyBind.Item2);
                Display = GetDisplay(settingsKeyBind.Item1, settingsKeyBind.Item2);
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

        private static string GetDisplay(ModifierKeys modifierKeys, Key key)
        {
            var modifierDisplay = modifierKeys != ModifierKeys.None 
                ? modifierKeys.ToString().Replace(",", "+").Replace(" ", "") + "+" 
                : "";

            var keyDisplay = key != Key.None ? key.ToString() : "";

            return $"{modifierDisplay}{keyDisplay}";
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
            if (GLOBAL_EDIT_MODE)
            {
                return;
            }

            EditingMode = true;
            if (_hotKey != null)
            {
                KeyBindSettings.SetSetting("", _purpose);
                _hotKey.UnregisterHotKey();
                _hotKey.HotKeyPressed -= HotKeyOnHotKeyPressed;
            }
            Display = "";
        }

        public void HandleKeyModifiers()
        {
            if (!EditingMode)
            {
                return;
            }

            _modifierKeys = Keyboard.Modifiers;
            Display = GetDisplay(_modifierKeys, Key.None);
        }

        public void HandleKeyBind(KeyEventArgs e)
        {
            if (!EditingMode)
            {
                return;
            }

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
                RegisterHotKey(_modifierKeys, key);
                Display = GetDisplay(_modifierKeys, key);
                KeyBindSettings.SetSetting(_modifierKeys + "," + key, _purpose);
            }
            catch (ApplicationException)
            {
                Display = "Error: Taken";
                return;
            }

            EditingMode = false;
        }

        private void RegisterHotKey(ModifierKeys modifierKeys, Key key)
        {
            _hotKey = new GlobalHotKey(modifierKeys, key, System.Windows.Application.Current.MainWindow);
            _hotKey.HotKeyPressed += HotKeyOnHotKeyPressed;
        }        

        private void HotKeyOnHotKeyPressed(GlobalHotKey globalHotKey)
        {
            if (!GLOBAL_EDIT_MODE)
            {
                _hotKeyAction.Invoke(_purpose);
            }
        }
    }
}
