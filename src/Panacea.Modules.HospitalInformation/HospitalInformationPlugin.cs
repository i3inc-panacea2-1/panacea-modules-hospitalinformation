using Panacea.Core;
using Panacea.Modularity;
using Panacea.Modularity.UiManager;
using Panacea.Modularity.UiManager.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            throw new NotImplementedException();
        }

        public Task EndInit()
        {
            throw new NotImplementedException();
        }

        public Task Shutdown()
        {
            throw new NotImplementedException();
        }

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
                        _tile = new TileLogo(GlobalSettings.FrontImgThumbnail?.Image);
                        MainButton?.Frames.Add(_tile);
                    }
                }
            }
            catch
            {
            }
        }
    }
}
