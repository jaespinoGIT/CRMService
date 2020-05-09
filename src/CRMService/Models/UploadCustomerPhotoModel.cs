using CRMService.Serialization;
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
        public string Description { get; set; }

        [JsonConverter(typeof(Base64FileJsonConverter))]
        public byte[] ImageData { get; set; }
    }
}
