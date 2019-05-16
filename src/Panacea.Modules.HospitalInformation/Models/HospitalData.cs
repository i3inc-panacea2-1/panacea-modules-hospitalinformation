using System.Runtime.Serialization;
using System;
using Panacea.Multilinguality;
using Panacea.Models;

namespace Panacea.Modules.HospitalInformation.Models
{
    [DataContract]
    public class HospitalData : ServerItem
    {
        [DataMember(Name = "hospital")]
        public String Hospital { get; set; }

        [DataMember(Name = "introductionVideo")]
        public Video IntroductionVideo { get; set; }

        [IsTranslatable]
        [DataMember(Name = "displayName")]
        public string DisplayName {
            get => GetTranslation();
            set => SetTranslation(value);
        }

        [IsTranslatable]
        [DataMember(Name = "categoriesText")]
        public string CategoriesText {
            get => GetTranslation();
            set => SetTranslation(value);
        }

        [DataMember(Name = "img")]
        public String Img { get; set; }

        [DataMember(Name = "front_img")]
        public String FrontImg { get; set; }

        [DataMember(Name = "includeContactUsTile")]
        public bool IncludeContactUsTile { get; set; }

        [DataMember(Name = "front_img_thumbnail")]
        public Thumbnail FrontImgThumbnail { get; set; }

        [DataMember(Name = "lng")]
        public string Lng { get; set; }

        [DataMember(Name = "lat")]
        public string Lat { get; set; }

        
    }
}
