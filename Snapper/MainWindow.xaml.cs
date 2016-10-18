using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Snapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        protected override void OnStateChanged(EventArgs args)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
            }
            base.OnStateChanged(args);
        }
        

        private Hotkeys.GlobalHotKey _numpad1_HotKey;
        private Hotkeys.GlobalHotKey _numpad2_HotKey;
        private Hotkeys.GlobalHotKey _numpad3_HotKey;
        private Hotkeys.GlobalHotKey _numpad4_HotKey;
        private Hotkeys.GlobalHotKey _numpad5_HotKey;
        private Hotkeys.GlobalHotKey _numpad6_HotKey;
        private Hotkeys.GlobalHotKey _numpad7_HotKey;
        private Hotkeys.GlobalHotKey _numpad8_HotKey;
        private Hotkeys.GlobalHotKey _numpad9_HotKey;
        private Hotkeys.GlobalHotKey _numpad0_HotKey;

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _LoadHotkeys();
            _SetupNotifyIcon();

            WindowState = WindowState.Minimized;
        }

        private void _LoadHotkeys()
        {
            _numpad1_HotKey = new Hotkeys.GlobalHotKey(ModifierKeys.Control | ModifierKeys.Alt, Key.NumPad1, this);
            _numpad2_HotKey = new Hotkeys.GlobalHotKey(ModifierKeys.Control | ModifierKeys.Alt, Key.NumPad2, this);
            _numpad3_HotKey = new Hotkeys.GlobalHotKey(ModifierKeys.Control | ModifierKeys.Alt, Key.NumPad3, this);
            _numpad4_HotKey = new Hotkeys.GlobalHotKey(ModifierKeys.Control | ModifierKeys.Alt, Key.NumPad4, this);
            _numpad5_HotKey = new Hotkeys.GlobalHotKey(ModifierKeys.Control | ModifierKeys.Alt, Key.NumPad5, this);
            _numpad6_HotKey = new Hotkeys.GlobalHotKey(ModifierKeys.Control | ModifierKeys.Alt, Key.NumPad6, this);
            _numpad7_HotKey = new Hotkeys.GlobalHotKey(ModifierKeys.Control | ModifierKeys.Alt, Key.NumPad7, this);
            _numpad8_HotKey = new Hotkeys.GlobalHotKey(ModifierKeys.Control | ModifierKeys.Alt, Key.NumPad8, this);
            _numpad9_HotKey = new Hotkeys.GlobalHotKey(ModifierKeys.Control | ModifierKeys.Alt, Key.NumPad9, this);
            _numpad1_HotKey.HotKeyPressed += _MoveWindowOnHotKeyPressed;
            _numpad2_HotKey.HotKeyPressed += _MoveWindowOnHotKeyPressed;
            _numpad3_HotKey.HotKeyPressed += _MoveWindowOnHotKeyPressed;
            _numpad4_HotKey.HotKeyPressed += _MoveWindowOnHotKeyPressed;
            _numpad5_HotKey.HotKeyPressed += _MoveWindowOnHotKeyPressed;
            _numpad6_HotKey.HotKeyPressed += _MoveWindowOnHotKeyPressed;
            _numpad7_HotKey.HotKeyPressed += _MoveWindowOnHotKeyPressed;
            _numpad8_HotKey.HotKeyPressed += _MoveWindowOnHotKeyPressed;
            _numpad9_HotKey.HotKeyPressed += _MoveWindowOnHotKeyPressed;

            _numpad0_HotKey = new Hotkeys.GlobalHotKey(ModifierKeys.Control | ModifierKeys.Alt, Key.NumPad0, this);
            _numpad0_HotKey.HotKeyPressed += _ToggleWindowMonitorOnHotKeyPressed;
        }

        private void _ToggleWindowMonitorOnHotKeyPressed(Hotkeys.GlobalHotKey globalHotKey)
        {
            MoveActiveWindow.ToggleWindowMonitor();
        }

        private void _MoveWindowOnHotKeyPressed(Hotkeys.GlobalHotKey globalHotKey)
        {
            MoveActiveWindow.MoveWindow(globalHotKey);
        }

        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/system.windows.forms.notifyicon.contextmenu(v=vs.110).aspx
        /// </summary>
        private void _SetupNotifyIcon()
        {
            var contextMenu = new System.Windows.Forms.ContextMenu();
            var menuItem = new System.Windows.Forms.MenuItem();

            // Initialize contextMenu1
            contextMenu.MenuItems.AddRange(new[] { menuItem });

            // Initialize menuItem1
            menuItem.Index = 0;
            menuItem.Text = "O&pen";
            menuItem.Click += delegate
            {
                Show();
                WindowState = WindowState.Normal;
            };

            var notificationIcon = new NotifyIcon
            {
                Icon = new Icon("img/Window.ico"),
                Visible = true,
                ContextMenu = contextMenu
            };

            notificationIcon.DoubleClick += delegate
            {
                Show();
                WindowState = WindowState.Normal;
            };
        }
    }
}
