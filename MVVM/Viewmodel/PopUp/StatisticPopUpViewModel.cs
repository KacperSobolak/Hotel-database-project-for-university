using System.Collections.ObjectModel;
using Hotel.Core;

namespace Hotel.MVVM.Viewmodel.PopUp
{
    internal class StatisticPopUpViewModel : ViewModel
    {
        private ObservableCollection<KeyValuePair<string, int>> _keyValuePairs;

        public ObservableCollection<KeyValuePair<string, int>> KeyValuePairs
        {
            get => _keyValuePairs;
            set
            {
                _keyValuePairs = value;
                OnPropertyChanged();
            }
        }
        public StatisticPopUpViewModel(IEnumerable<KeyValuePair<string, int>> keyValuePairs)
        {
            KeyValuePairs = new ObservableCollection<KeyValuePair<string, int>>(keyValuePairs);
        }
    }
}
