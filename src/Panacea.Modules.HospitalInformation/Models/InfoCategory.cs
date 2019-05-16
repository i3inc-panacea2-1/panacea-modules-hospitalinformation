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

    public class MapInfoCategory : InfoCategory
    {
        public MapInfoCategory(double lat, double lng)
        {
            Name = "Find us";
            Content = new MapOnly(lat, lng);
            TSize = 2;
        }

        public Control Content { get; set; }
    }

    public class ContactInfoCategory: InfoCategory
    {
        public ContactInfoCategory()
        {
            Name = "Contact us";
            Content = new Contact();
            TSize = 2;
            Img = "pack://application:,,,/UserPlugins.HospitalInformation;component/Pictures/mail.png";
        }

        public Control Content { get; set; }
    }

}
