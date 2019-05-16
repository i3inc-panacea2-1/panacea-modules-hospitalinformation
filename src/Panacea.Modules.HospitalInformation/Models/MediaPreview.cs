using Panacea.Multilinguality;
using System;
using System.ComponentModel;

namespace Panacea.Modules.HospitalInformation.Models
{
    public class MediaPreview : PropertyChangedBase
    {
        public MediaPreview() { }

        public MediaPreview(string title, string previewImage, Media m)
        {
            _title = title;
            previewImg = previewImage;
            media = m;
        }

        private string _title;
        public string title
        {
            get
            {
                return this._title;
            }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _previewImg;
        public string previewImg
        {
            get
            {
                return _previewImg;
            }
            set
            {
                if (value != _previewImg)
                {
                    _previewImg = value;
                    OnPropertyChanged();
                }
            }
        }

        private Media _media;
        public Media media
        {
            get
            {
                return _media;
            }
            set
            {
                if (value != _media)
                {
                    _media = value;
                    OnPropertyChanged();
                }
            }
        }

      
    }
}
