﻿using Panacea.Core;
using Panacea.Modularity.UiManager;
using Panacea.Modules.HospitalInformation.Models;
using Panacea.Modules.HospitalInformation.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Panacea.Modules.HospitalInformation
{
    public class HospitalInformationPlugin : ICallablePlugin,ILiveTilesPlugin
    {
        private readonly PanaceaServices _core;
        public static bool IntroVideoPlayed;
        private HospitalData _settings;
        List<InfoCategory> _categories;
        System.Windows.Media.Brush _color;

        public HospitalInformationPlugin(PanaceaServices core)
        {
            _core = core;
        }

        public async Task BeginInit()
        {
            if (_core.UserService != null)
            {
                _core.UserService.UserLoggedOut += UserService_UserChanged;
                _core.UserService.UserLoggedIn += UserService_UserChanged;
            }
            try
            {
                await GetHospitalSettingsAsync();
            }
            catch { }

        }

        private Task UserService_UserChanged(IUser user)
        {
            IntroVideoPlayed = false;
            return Task.CompletedTask;
        }

        public async void Call()
        {
            if (_core.TryGetUiManager(out IUiManager ui))
            {
                await ui.DoWhileBusy(async () =>
                {
                   
                    try
                    {
                        if (_settings == null)
                        {
                            await GetHospitalSettingsAsync();
                        }
                        await GetCategoriesAsync();
                        await GetPrimaryColorFromImageAsync();
                        ui.Navigate(new HospitalInformationListViewModel(_core, _settings, _categories, _color));
                    }
                    catch (Exception ex)
                    {
                        _settings = null;
                    }
                });
            }
        }

        public void Dispose()
        {

        }

        public Task EndInit()
        {
            return Task.CompletedTask;
        }

        public Task Shutdown()
        {
            return Task.CompletedTask;
        }

        private async Task GetCategoriesAsync()
        {
            try
            {
                var response =
                    await _core.HttpClient.GetObjectAsync<List<InfoCategory>>("hospitalinfo/get_infocategories/");
                if (response.Success)
                {
                    _categories = response.Result;
                }
            }
            catch
            {
            }
        }

        private async Task GetPrimaryColorFromImageAsync()
        {
            var data = await _core.HttpClient.DownloadDataAsync(_settings.ImgThumbnail.Image);
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
                _color = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(c.R, c.G, c.B));
            }
        }

        TileLogoViewModel _tile;

        public ObservableCollection<LiveTileFrame> Frames { get; private set; } = new ObservableCollection<LiveTileFrame>();

        private async Task GetHospitalSettingsAsync()
        {
            try
            {
                var response =
                    await _core.HttpClient.GetObjectAsync<HospitalData>("hospitalinfo/get_hospitalinfo/");
                Frames.Clear();
                if (response.Success)
                {
                    _settings = response.Result;
                    if (!string.IsNullOrEmpty(_settings.FrontImgThumbnail?.Image) /*&& _tile == null*/)
                    {
                        _tile = new TileLogoViewModel(_settings.FrontImgThumbnail?.Image);
                        Frames.Add(new LiveTileFrame(_tile, 8000));
                    }
                }
            }
            catch { }
        }
    }
}
