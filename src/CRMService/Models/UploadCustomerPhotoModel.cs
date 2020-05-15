using CRMService.Helpers.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMService.Models
{
    public class UploadCustomerPhotoModel
    {    
        [JsonConverter(typeof(Base64FileJsonConverter))]
        public byte[] Photo { get; set; }
    }
}
