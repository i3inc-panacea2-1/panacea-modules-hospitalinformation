using Panacea.Core;
using Panacea.Modularity.UiManager;
using Panacea.Modularity.UiManager.Extensions;
using Panacea.Modules.HospitalInformation.Models;
using Panacea.Modules.HospitalInformation.ViewModels;
using System;
using System.Threading.Tasks;

namespace Panacea.Modules.HospitalInformation
{
    public class HospitalInformationPlugin : ICallablePlugin
    {
        private readonly PanaceaServices _core;
        public static bool IntroVideoPlayed;
        public static HospitalData GlobalSettings;

        public HospitalInformationPlugin(PanaceaServices core)
        {
            _core = core;
        }

        public Task BeginInit()
        {
            _core.UserService.UserLoggedOut += UserService_UserChanged;
            _core.UserService.UserLoggedIn += UserService_UserChanged;
            return Task.CompletedTask;
        }

        private Task UserService_UserChanged(IUser user)
        {
            IntroVideoPlayed = false;
            return Task.CompletedTask;
        }

        public void Call()
        {
            _core
                .GetUiManager()
                .Navigate(new HospitalInformationListViewModel(_core));
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

        object _tile;
        private async Task GetHospitalSettings()
        {
            try
            {
                var response =
                    await _core.HttpClient.GetObjectAsync<HospitalData>("hospitalinfo/get_hospitalinfo/");
                if (response.Success)
                {
                    GlobalSettings = response.Result;
                    if (!string.IsNullOrEmpty(GlobalSettings.FrontImgThumbnail?.Image) && _tile == null)
                    {
                        //todo _tile = new TileLogo(GlobalSettings.FrontImgThumbnail?.Image);
                        //todo MainButton?.Frames.Add(_tile);
                    }
                }
            }
            catch
            {
            }
        }
    }
}
