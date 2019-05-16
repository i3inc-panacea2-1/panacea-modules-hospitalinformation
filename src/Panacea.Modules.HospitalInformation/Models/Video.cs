using System.Runtime.Serialization;
using System;
using Panacea.Models;

namespace Panacea.Modules.HospitalInformation.Models
{
    [DataContract]
    public class Video : ServerItem
    {
        [DataMember(Name = "videoType")]
        public String VideoType { get; set; }

        [DataMember(Name = "url")]
        public String Url { get; set; }
    }
}
