using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Snapper.Views
{
    /// <summary>
    /// Interaction logic for KeyBindings.xaml
    /// </summary>
    public partial class KeyBindings : UserControl
    {
        private KeyBindingsModel _vm;

        public KeyBindings()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                _vm = new KeyBindingsModel(Label, HotKeyAction);
                DataContext = _vm;
            };
        }

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty = 
            DependencyProperty.Register("Label", typeof(string), typeof(KeyBindings));

        public Action<Hotkeys.GlobalHotKey> HotKeyAction
        {
            get { return (Action<Hotkeys.GlobalHotKey>)GetValue(HotKeyActionProperty); }
            set { SetValue(HotKeyActionProperty, value); }
        }

        public static readonly DependencyProperty HotKeyActionProperty =
            DependencyProperty.Register("HotKeyAction", typeof(Action<Hotkeys.GlobalHotKey>), typeof(KeyBindings));

        public void OnClick(object sender, RoutedEventArgs e)
        {
            _vm.HandleClick();
        }

        public void KeyModifiers(object sender, KeyEventArgs e)
        {
            _vm.HandleKeyModifiers();   
        }

        public void KeyBind(object sender, KeyEventArgs e)
        {
            _vm.HandleKeyBind(e);
        }
    }
}
