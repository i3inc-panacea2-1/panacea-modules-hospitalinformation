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
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Panacea.Modularity.Media.Channels;

namespace Panacea.Modules.HospitalInformation.ViewModels
{
    [View(typeof(HospitalInformationList))]
    class HospitalInformationListViewModel : ViewModelBase
    {

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

        public string DisplayName { get; protected set; }

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

        public HospitalInformationListViewModel(PanaceaServices core)
        {
            _core = core;
            _logo = HospitalInformationPlugin.GlobalSettings.Img;
            CategoriesText = HospitalInformationPlugin.GlobalSettings.CategoriesText;
            DisplayName = HospitalInformationPlugin.GlobalSettings.DisplayName;
            _core.HttpClient.DownloadDataAsync(HospitalInformationPlugin.GlobalSettings.ImgThumbnail.Image)
                .ContinueWith(task =>
                {
                    try
                    {
                        var data = task.Result;
                        using (var ms = new MemoryStream(data))
                        {
                            var bmp = new Bitmap(ms);
                            PictureAnalysis.GetMostUsedColor(bmp);
                            var c = PictureAnalysis.TenMostUsedColors.FirstOrDefault(col => Math.Abs(col.R - col.G) > 30 ||
                                Math.Abs(col.G - col.B) > 30 || Math.Abs(col.B - col.R) > 30);
                            if (c == null)
                            {
                                c = PictureAnalysis.MostUsedColor;
                            }
                            Color = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(c.R, c.G, c.B));
                        }
                    }
                    catch { }

                }, TaskContinuationOptions.ExecuteSynchronously);
            
            OpenCommand = new RelayCommand(async (args) => await GetInfoPagesFromServer(args as InfoCategory));
            OpenMapCommand = new RelayCommand(args =>
            {
                //todo
                //_window.ThemeManager.Navigate(new MapOnly(Convert.ToDouble(HospitalInformationPlugin.GlobalSettings.Lat, CultureInfo.InvariantCulture),
                //    Convert.ToDouble(HospitalInformationPlugin.GlobalSettings.Lng, CultureInfo.InvariantCulture)));
            });
            MapVisibility = !string.IsNullOrEmpty(HospitalInformationPlugin.GlobalSettings.Lat) && !string.IsNullOrEmpty(HospitalInformationPlugin.GlobalSettings.Lng) ? Visibility.Visible : Visibility.Collapsed;
            ContactVisibility = HospitalInformationPlugin.GlobalSettings.IncludeContactUsTile ? Visibility.Visible : Visibility.Collapsed;
            OpenContactCommand = new RelayCommand(args =>
            {
                //todo _com.RaiseEvent("ShowContact", null, null);
            });
        }

        bool _loaded = false;
        public override async void Activate()
        {
            if (_loaded) return;
            _loaded = true;
            await GetDefaultInfoCategoriesFromServer();
        }


        private async Task GetDefaultInfoCategoriesFromServer()
        {
            try
            {
                ServerResponse<List<InfoCategory>> response =
                    await _core.HttpClient.GetObjectAsync<List<InfoCategory>>("hospitalinfo/get_infocategories/");
                if (response.Success)
                {
                    Tiles = new ObservableCollection<InfoCategory>(response.Result);

                    if (!string.IsNullOrEmpty(HospitalInformationPlugin.GlobalSettings.Lat) && !String.IsNullOrEmpty(HospitalInformationPlugin.GlobalSettings.Lng))
                    {
                        var lat = Convert.ToDouble(HospitalInformationPlugin.GlobalSettings.Lat, CultureInfo.InvariantCulture);
                        var lng = Convert.ToDouble(HospitalInformationPlugin.GlobalSettings.Lng, CultureInfo.InvariantCulture);
                    }
                    if (!string.IsNullOrEmpty(HospitalInformationPlugin.GlobalSettings.IntroductionVideo.Url) && !_videoPlayed)
                    {
                        _videoPlayed = true;
                        var url = _core.HttpClient.RelativeToAbsoluteUri(HospitalInformationPlugin.GlobalSettings.IntroductionVideo.Url);
                        _core
                            .GetMediaPlayerContainer()
                            .Play(new MediaRequest(new IptvMedia { URL = url }));
                    }
                }
            }
            catch
            {
            }

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
                        var vm = new HospitalInformationDetailsViewModel(_core, cat, pages);
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
