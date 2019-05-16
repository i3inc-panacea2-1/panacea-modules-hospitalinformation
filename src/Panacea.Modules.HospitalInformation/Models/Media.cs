using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using Panacea.Models;

namespace Panacea.Modules.HospitalInformation.Models
{
    [DataContract]
    public class Media : ServerItem
    {
        public Media()
        {
            
        }
        [DataMember(Name = "mediaType")]
        public String MediaType { get; set; }

        [DataMember(Name = "urls")]
        public List<String> Urls { get; set; }

        private List<String> _files;

        [DataMember(Name = "files")]
        public List<String> Files
        {
            get { return _files; }
            set
            {
                _files = value;
                OnPropertyChanged("Files");
            }
        }

        [DataMember(Name = "form")]
        public String Form { get; set; }
    }
}
