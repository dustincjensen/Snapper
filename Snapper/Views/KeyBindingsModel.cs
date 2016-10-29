using System;
using System.Collections.Generic;
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
        private readonly Action<Hotkeys.GlobalHotKey> _hotKeyAction;

        private string _display;
        private bool _editingMode;
        private Hotkeys.GlobalHotKey _hotKey;
        private ModifierKeys _modifierKeys = ModifierKeys.None;        

        public KeyBindingsModel(string label, Action<Hotkeys.GlobalHotKey> hotKeyAction)
        {
            Label = label;
            _hotKeyAction = hotKeyAction;
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
                _hotKey.UnregisterHotKey();
                _hotKey.HotKeyPressed -= HotKeyOnHotKeyPressed;
            }
            Display = "";
        }

        public void HandleKeyModifiers()
        {
            if (!EditingMode) return;

            Display = "";
            _modifierKeys = ModifierKeys.None;

            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                _modifierKeys = _modifierKeys | ModifierKeys.Control;
                Display = "Ctrl";
            }

            if ((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
            {
                _modifierKeys = _modifierKeys | ModifierKeys.Alt;
                Display = String.IsNullOrWhiteSpace(Display)
                    ? "Alt" : Display + "+Alt";
            }

            if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
            {
                _modifierKeys = _modifierKeys | ModifierKeys.Shift;
                Display = String.IsNullOrWhiteSpace(Display)
                    ? "Shift" : Display + "+Shift";
            }
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

            Display = !String.IsNullOrWhiteSpace(Display)
                ? Display + "+" + key : key.ToString();

            try
            {
                _hotKey = new Hotkeys.GlobalHotKey(_modifierKeys, key, System.Windows.Application.Current.MainWindow);
                _hotKey.HotKeyPressed += HotKeyOnHotKeyPressed;
            }
            catch (ApplicationException)
            {
                Display = "Error: Taken";
                return;
            }

            EditingMode = false;
        }

        private void HotKeyOnHotKeyPressed(GlobalHotKey globalHotKey)
        {
            if (!GLOBAL_EDIT_MODE)
            {
                _hotKeyAction.Invoke(globalHotKey);
            }
        }
    }
}
