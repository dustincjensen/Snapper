using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Snapper.Views
{
    /// <summary>
    /// Interaction logic for KeyBindings.xaml
    /// </summary>
    public partial class KeyBindings : UserControl
    {
        private KeyBindingsModel _viewModel;

        public KeyBindings()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                _viewModel = new KeyBindingsModel(Label, HotKeyAction, Purpose);
                DataContext = _viewModel;
            };
        }

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty = 
            DependencyProperty.Register("Label", typeof(string), typeof(KeyBindings));

        public Action<KeyBindEnum> HotKeyAction
        {
            get { return (Action<KeyBindEnum>)GetValue(HotKeyActionProperty); }
            set { SetValue(HotKeyActionProperty, value); }
        }

        public static readonly DependencyProperty HotKeyActionProperty =
            DependencyProperty.Register("HotKeyAction", typeof(Action<KeyBindEnum>), typeof(KeyBindings));

        public KeyBindEnum Purpose
        {
            get { return (KeyBindEnum) GetValue(PurposeProperty); }
            set { SetValue(PurposeProperty, value); }
        }

        public static readonly DependencyProperty PurposeProperty = 
            DependencyProperty.Register("Purpose", typeof(KeyBindEnum), typeof(KeyBindings));

        public void OnClick(object sender, RoutedEventArgs e)
        {
            _viewModel.HandleClick();
        }

        public void KeyModifiers(object sender, KeyEventArgs e)
        {
            _viewModel.HandleKeyModifiers();   
        }

        public void KeyBind(object sender, KeyEventArgs e)
        {
            _viewModel.HandleKeyBind(e);
        }
    }
}
