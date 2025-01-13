using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Core
{
    public abstract class ViewModel : ObservableObject
    {
        public virtual void OnEnter() { }
    }
}
