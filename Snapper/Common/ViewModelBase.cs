using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Snapper.Common
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        protected ViewModelBase() {}

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
