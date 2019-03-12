using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService
{
    // prdstavlja base klasu iz koje poticu View i ViewModel 
    // pomocu INotifyPropertyChanged obavestava se View i ViewModel da je property promenjen
    public class BindableBase : INotifyPropertyChanged
    {
        protected virtual void SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(member, val)) return;

            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            // metoda koja rukuje dogadjajem kada se vrednost property-a promenila
            // ko je izazvao ovaj dogadjaj - BindableBase
            // napravi novu instancu PropertyChangedEventArgs klase sa property-jem koji je promenjen
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
