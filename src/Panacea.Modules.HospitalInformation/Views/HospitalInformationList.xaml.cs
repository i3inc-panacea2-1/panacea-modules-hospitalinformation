using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using PanaceaLib;
using PanaceaLib.Controls;
using PanaceaLib.Interfaces;
using PanaceaLib.Media;
using PanaceaLib.Multilinguality;
using UserPlugins.HospitalInformation.Controls;
using UserPlugins.HospitalInformation.Models;
using UserPlugins.HospitalInformation.templates;

namespace Panacea.Modules.HospitalInformation.Views
{
    /// <summary>
    ///     Interaction logic for UserControl1.xaml
    /// </summary>
    [Animatable]
    public partial class HospitalInformationList : PanaceaLib.Controls.PanaceaPage
    {

        public HospitalInformationList()
        {
            InitializeComponent();
            Utils.MakeScrollViewerScroll(Scroller);
        }

#if DEBUG
        ~HospitalInformationList()
        {
            Console.WriteLine("~HospitalInformationList");
        }
#endif
    }
}