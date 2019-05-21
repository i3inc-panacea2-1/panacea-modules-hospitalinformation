using Panacea.Controls;
using Panacea.Core;
using Panacea.Mvvm;
using Panacea.Modularity.MediaPlayerContainer;
using Panacea.Modules.HospitalInformation.Models;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using Panacea.Modularity.Media.Channels;
using Panacea.Modules.HospitalInformation.Views;
using Panacea.Modularity.WebBrowsing;
using Panacea.Modularity.UiManager;

namespace Panacea.Modules.HospitalInformation.ViewModels
{
    [View(typeof(HospitalInformationDetails))]
    public class HospitalInformationDetailsViewModel : ViewModelBase
    {
        private readonly PanaceaServices _core;
        private readonly InfoCategory _category;
        private readonly List<InfoPage> _pages;

        private readonly Brush _brush;

        public InfoCategory Category { get => _category; }

        public List<InfoPage> Pages { get => _pages; }

        public HospitalData Settings { get; set; }

        public HospitalInformationDetailsViewModel(
            PanaceaServices core,
            HospitalData settings,
            InfoCategory category,
            List<InfoPage> pages)
        {

            _core = core;
            _category = category;
            _pages = pages;
            Settings = settings;
            OpenCommand = new RelayCommand(args =>
            {
                BindPage(args as InfoPage);
            });
        }

        public ICommand OpenCommand { get; set; }

        private void BindPage(InfoPage ip)
        {
            //todo _socket.PopularNotify("HospitalInformation", "InfoPage", ip.Id);
            if (ip.PageType.Equals("url"))
            {
                _core.GetWebBrowser().OpenUnmanaged(ip.Url);
            }
            else if (ip.PageType.Equals("media"))
            {
                var url = _core.HttpClient.RelativeToAbsoluteUri(ip.Url);
                if (_core.TryGetMediaPlayerContainer(out IMediaPlayerContainer player))
                {
                    player.Play(new MediaRequest(new IptvMedia() { URL = url })
                    {
                        MediaPlayerPosition = MediaPlayerPosition.Standalone

                    });
                }

            }
            else if (!string.IsNullOrEmpty(ip.Content))
            {
                OpenContent(_category, ip);
            }
            else if (ip.Media.Count > 0)
            {
                OpenMediaPreview(null, TakeCareOfPreviews(ip.Media)[0]);
            }
        }

        void OpenContent(InfoCategory category, InfoPage page)
        {
            if (_core.TryGetUiManager(out IUiManager ui))
            {
                ui.Navigate(new PagePresenterViewModel(page), true);
            }
        }

        private void OpenMediaPreview(object sender, MediaPreview mp)
        {
            switch (mp.media.MediaType)
            {
                case "photo":
                    //todo 
                    //if (mp.media.Urls.Count > 0)
                    //    _core.GetUiManager().Navigate(new SingleImage(mp.media.Urls[0]));
                    //else if (mp.media.Files.Count > 0)
                    //    _core.GetUiManager().Navigate(new SingleImage(mp.media.Files[0]));
                    break;
                case "pdf":
                    //todo if (mp.media.Files.Count > 0)
                    //todo _com.RaiseEvent("books-openbook", sender, new Dictionary<string, dynamic>() { { "pdfBook", mp.media.Files[0] }, { "plugin", "HospitalInformation" } });
                    break;
                case "gallery":
                    List<string> photoPaths = new List<string>();
                    if (mp.media.Files.Count > 0)
                        photoPaths.AddRange(mp.media.Files);
                    if (mp.media.Urls.Count > 0)
                        photoPaths.AddRange(mp.media.Urls);
                    //todo _com.RaiseEvent("PhotoGallery-OpenPhotos", sender, new Dictionary<string, dynamic>() { { "GalleryPhotos", photoPaths } });
                    break;
                case "video":
                    if ((mp.media.Files.Count > 0 && !string.IsNullOrEmpty(mp.media.Files[0])) || (mp.media.Urls.Count > 0 && !string.IsNullOrEmpty(mp.media.Urls[0])))
                    {
                        IptvMedia media = null;
                        if (mp.media.Files.Count > 0 && !string.IsNullOrEmpty(mp.media.Files[0]))
                        {
                            media = new IptvMedia() { URL = mp.media.Files[0] };
                        }
                        else if (mp.media.Urls.Count > 0 && !string.IsNullOrEmpty(mp.media.Urls[0]))
                        {
                            media = new IptvMedia() { URL = mp.media.Urls[0] };
                        }
                        if (media != null && _core.TryGetMediaPlayerContainer(out IMediaPlayerContainer player))
                        {
                            player.Play(new MediaRequest(media)
                            {
                                FullscreenMode = FullscreenMode.FullscreenOnly
                            });
                        }
                    }
                    break;
                case "url":
                    if (mp.media.Urls.Count > 0)
                    {
                        //todo 
                        //if (!string.IsNullOrEmpty(mp.media.Urls[0]))
                        //     _core.GetUiManager()_com.OpenUri("chromium:?url=" + mp.media.Urls[0]);
                    }
                    break;

            }
        }

        private List<MediaPreview> TakeCareOfPreviews(List<Media> medias)
        {
            var previews = new List<MediaPreview>();
            foreach (var m in medias)
            {
                switch (m.MediaType)
                {
                    case "photo":
                        previews.Add(new MediaPreview(m.Name, "Pictures/gallery_256.png", m));
                        break;
                    case "gallery":
                        previews.Add(new MediaPreview(m.Name, m.Files[0], m));
                        break;
                    case "url":
                        previews.Add(new MediaPreview(m.Name, "Pictures/webpage_256.png", m));
                        break;
                    case "video":
                        previews.Add(new MediaPreview(m.Name, "Pictures/file_type_video_256.png", m));
                        break;
                    case "pdf":
                        previews.Add(new MediaPreview(m.Name, "Pictures/file_format_pdf.png", m));
                        break;
                }
            }
            return previews;
        }
    }
}
