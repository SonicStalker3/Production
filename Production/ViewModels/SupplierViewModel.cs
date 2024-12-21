using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Production.DB;

namespace Production.ViewModels
{
    public class SupplierViewModel : INotifyPropertyChanged
    {
        private bool _isSelected;

        public Supplier Supplier { get; set; }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public SupplierViewModel(Supplier supplier)
        {
            Supplier = supplier;
            IsSelected = false; // Изначально не выбрано
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
