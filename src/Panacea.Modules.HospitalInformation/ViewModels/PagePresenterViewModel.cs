using Panacea.Modules.HospitalInformation.Models;
using Panacea.Modules.HospitalInformation.Views;
using Panacea.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panacea.Modules.HospitalInformation.ViewModels
{
    [View(typeof(PagePresenter))]
    public class PagePresenterViewModel:ViewModelBase
    {
        public InfoPage Page { get; set; }
        public PagePresenterViewModel(InfoPage ip)
        {
            Page = ip;
           
        }
    }
}
