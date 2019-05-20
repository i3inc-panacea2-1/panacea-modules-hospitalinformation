using Panacea.Controls;
using Panacea.Core;
using Panacea.Mvvm;
using Panacea.Modularity.MediaPlayerContainer;
using Panacea.Modularity.MediaPlayerContainer.Extensions;
using Panacea.Modularity.UiManager.Extensions;
using Panacea.Modules.HospitalInformation.Models;
using Panacea.Modules.HospitalInformation.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Panacea.Modularity.Media.Channels;
using System.Windows.Media;

namespace Panacea.Modules.HospitalInformation.ViewModels
{
    [View(typeof(HospitalInformationList))]
    class HospitalInformationListViewModel : ViewModelBase
    {
        private HospitalData _settings;
        public HospitalData Settings
        {
            get => _settings;
            set
            {
                _settings = value;
                OnPropertyChanged();
            }
        }

        bool _videoPlayed;
        ObservableCollection<InfoCategory> _tiles;
        public ObservableCollection<InfoCategory> Tiles
        {
            get => _tiles;
            set
            {
                _tiles = value;
                OnPropertyChanged();
            }
        }

        private readonly PanaceaServices _core;
        string _logo;
        public string Logo
        {
            get => _logo;
            set
            {
                _logo = value;
                OnPropertyChanged();
            }
        }

        public string CategoriesText { get; protected set; }

  
        public ICommand LoadedCommand { get; set; }

        public ICommand OpenCommand { get; set; }

        System.Windows.Media.Brush _color;
        public System.Windows.Media.Brush Color
        {
            get => _color;
            set
            {
                _color = value;
                OnPropertyChanged();
            }
        }

        public Visibility MapVisibility { get; set; }
        public Visibility ContactVisibility { get; set; }

        public HospitalInformationListViewModel(
            PanaceaServices core, 
            HospitalData settings,
            List<InfoCategory> categories,
            Brush color)
        {
            _core = core;
            _settings = settings;
            Color = color;
            Tiles = new ObservableCollection<InfoCategory>(categories);
            MapVisibility = !string.IsNullOrEmpty(_settings.Lat) && !string.IsNullOrEmpty(_settings.Lng) ? Visibility.Visible : Visibility.Collapsed;
            ContactVisibility = _settings.IncludeContactUsTile ? Visibility.Visible : Visibility.Collapsed;
            OpenCommand = new RelayCommand(async (args) => await GetInfoPagesFromServer(args as InfoCategory));
            OpenMapCommand = new RelayCommand(args =>
            {
                _core
                .GetUiManager()
                .Navigate(
                    new MapControlViewModel(
                        Convert.ToDouble(_settings.Lat, CultureInfo.InvariantCulture),
                        Convert.ToDouble(_settings.Lng, CultureInfo.InvariantCulture)));
            });
            
            OpenContactCommand = new RelayCommand(args =>
            {
                //todo _com.RaiseEvent("ShowContact", null, null);
            });
        }

        public override void Activate()
        {
            if (!string.IsNullOrEmpty(_settings.IntroductionVideo.Url) && !_videoPlayed)
            {
                _videoPlayed = true;
                var url = _core.HttpClient.RelativeToAbsoluteUri(_settings.IntroductionVideo.Url);
                var res = _core
                    .GetMediaPlayerContainer()
                    .Play(new MediaRequest(new IptvMedia { URL = url }));
                res.Stopped += Res_Stopped;
                res.Ended += Res_Stopped;
                res.Error += Res_Stopped;
            }
        }

        private void Res_Stopped(object sender, EventArgs e)
        {
            _core.GetUiManager().GoBack();
        }

        public ICommand OpenMapCommand { get; set; }

        public ICommand OpenContactCommand { get; set; }


        private async Task GetInfoPagesFromServer(InfoCategory cat)
        {
            await _core.GetUiManager().DoWhileBusy(async () =>
            {
                try
                {
                    var response =
                       await _core.HttpClient.GetObjectAsync<List<InfoPage>>("hospitalinfo/get_infopages/" + cat.Id + "/");
                    if (!response.Success) return;
                    var pages = response.Result;

                    if (pages.Count > 0)
                    {
                        var vm = new HospitalInformationDetailsViewModel(_core, _settings, cat, pages);
                        _core.GetUiManager().Navigate(vm);
                    }
                }
                catch
                {
                }
            });

        }

        void OpenPages(InfoCategory ic, List<InfoPage> pages)
        {

            //todo var ipc = new InfoPagesContext(_window, _com, _server, _player, _websocket, ic, pages, HospitalInformationPage.GlobalSettings);
            //Host.ShowPopup(ipc, "", Shared.Controls.Popups.PopupType.None);
            //todo _window.ThemeManager.ShowPopup(ipc);
        }
    }
}
