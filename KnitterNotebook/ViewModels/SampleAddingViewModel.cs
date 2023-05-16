using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.ViewModels
{
    class SampleAddingViewModel : BaseViewModel
    {
        public SampleAddingViewModel()
        {
        }

        private string _yarnName = string.Empty;
        public string YarnName
        {
            get { return _yarnName; }
            set { _yarnName = value; OnPropertyChanged(); }
        }

        private int _loopsQuantity;
        public int LoopsQuantity 
        { 
            get { return _loopsQuantity; } 
            set { _loopsQuantity = value; OnPropertyChanged(); }
        }

        private int _rowsQuantity;
        public int RowsQuantity 
        {
            get { return _rowsQuantity; }
            set { _rowsQuantity = value; OnPropertyChanged(); }
        }

        private int _needleSize;
        public int NeedleSize
        {
            get { return _needleSize; }
            set { _needleSize = value; OnPropertyChanged(); }
        }

        public string _needleSizeUnit = string.Empty;
        public string NeedleSizeUnit
        { 
            get { return _needleSizeUnit; } 
            set { _needleSizeUnit = value; OnPropertyChanged(); } 
        }
    }
}
