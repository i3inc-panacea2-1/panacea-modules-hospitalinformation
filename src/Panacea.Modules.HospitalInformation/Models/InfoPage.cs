using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using Panacea.Models;
using Panacea.Multilinguality;

namespace Panacea.Modules.HospitalInformation.Models
{
    [DataContract]
    public class InfoPage : ServerItem
    {
        [DataMember(Name = "pageType")]
        public String PageType { get; set; }

        [IsTranslatable]
        [DataMember(Name = "content")]
        public string Content {
            get => GetTranslation();
            set => SetTranslation(value);
        }

        [DataMember(Name = "url")]
        public String Url { get; set; }

        [DataMember(Name = "media")]
        public List<Media> Media { get; set; }

        [DataMember(Name = "introductionVideo")]
        public Video IntroductionVideo { get; set; }
        
    }
}
