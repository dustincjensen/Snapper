using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Snapper.Views;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace Snapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private List<KeyBindings> _keyBindingsList;

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
        
        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _LoadHotkeys();
            _SetupNotifyIcon();

            //WindowState = WindowState.Minimized;
        }

        public List<KeyBindings> KeyBindingsList
        {
            get { return _keyBindingsList; }
            private set
            {
                _keyBindingsList = value;
                OnPropertyChanged();
            }
        }

        private void _LoadHotkeys()
        {           
            KeyBindingsList = new List<KeyBindings>
            {
                new KeyBindings {Label = "Swap Monitors", HotKeyAction = ToggleWindowMonitorOnHotKeyPressed, Purpose = KeyBindEnum.SwapMonitor},
                new KeyBindings {Label = "Bottom Left", HotKeyAction = MoveWindowOnHotKeyPressed, Purpose = KeyBindEnum.BottomLeft},
                new KeyBindings {Label = "Bottom", HotKeyAction = MoveWindowOnHotKeyPressed, Purpose = KeyBindEnum.Bottom},
                new KeyBindings {Label = "Bottom Right", HotKeyAction = MoveWindowOnHotKeyPressed, Purpose = KeyBindEnum.BottomRight},
                new KeyBindings {Label = "Left", HotKeyAction = MoveWindowOnHotKeyPressed, Purpose = KeyBindEnum.Left},
                new KeyBindings {Label = "Mid", HotKeyAction = MoveWindowOnHotKeyPressed, Purpose = KeyBindEnum.Mid},
                new KeyBindings {Label = "Right", HotKeyAction = MoveWindowOnHotKeyPressed, Purpose = KeyBindEnum.Right},
                new KeyBindings {Label = "Top Left", HotKeyAction = MoveWindowOnHotKeyPressed, Purpose = KeyBindEnum.TopLeft},
                new KeyBindings {Label = "Top", HotKeyAction = MoveWindowOnHotKeyPressed, Purpose = KeyBindEnum.Top},
                new KeyBindings {Label = "Top Right", HotKeyAction = MoveWindowOnHotKeyPressed, Purpose = KeyBindEnum.TopRight}
            };
        }

        public void ToggleWindowMonitorOnHotKeyPressed(KeyBindEnum purpose)
        {
            MoveActiveWindow.ToggleWindowMonitor();
        }

        public void MoveWindowOnHotKeyPressed(KeyBindEnum purpose)
        {
            MoveActiveWindow.MoveWindow(purpose);
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
