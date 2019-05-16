using Panacea.Models;
using Panacea.Multilinguality;
using System.Runtime.Serialization;
using System.Windows.Controls;

namespace Panacea.Modules.HospitalInformation.Models
{
    [DataContract]
    public class InfoCategory : ServerItem
    {

        [IsTranslatable]
        [DataMember(Name = "description")]
        public string Description {
            get => GetTranslation();
            set => SetTranslation(value);
        }

        [DataMember(Name = "tSize")]
        public int TSize { get; set; }

        [DataMember(Name = "img")]
        public string Img { get; set; }

    }

}
