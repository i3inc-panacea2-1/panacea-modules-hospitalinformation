using Microsoft.Maps.MapControl.WPF;
using Panacea.Modules.HospitalInformation.Views;
using Panacea.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panacea.Modules.HospitalInformation.ViewModels
{
    [View(typeof(MapControl))]
    public class MapControlViewModel:ViewModelBase
    {
        public Location Center { get; set; }
        public MapControlViewModel(double lat, double lng)
        {
            Center = new Location(lat, lng);
        }
    }
}
