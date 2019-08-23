using Panacea.Modules.HospitalInformation.Views;
using Panacea.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panacea.Modules.HospitalInformation.ViewModels
{
    [View(typeof(TileLogo))]
    class TileLogoViewModel:ViewModelBase
    {
        public string Url { get; set; }
        public TileLogoViewModel(string url)
        {
            Url = url;
        }
    }
}
