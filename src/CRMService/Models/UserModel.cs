using CRMService.Helpers.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMService.Models
{
    public class UserModel
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }       
        public IList<string> Roles { get; set; }
        public IList<System.Security.Claims.Claim> Claims { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
