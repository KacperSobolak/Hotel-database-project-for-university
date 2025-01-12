using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.Core;

namespace Hotel.MVVM.Viewmodel.PopUp
{
    internal class ShowAmenitiesViewModel : ViewModel
    {
        public ObservableCollection<AmenityItem> AmenitiesCollection { get; set; }

        public ShowAmenitiesViewModel(IEnumerable<AmenityItem> amenityItems)
        {
            AmenitiesCollection = new ObservableCollection<AmenityItem>(amenityItems);
        }
    }
}
