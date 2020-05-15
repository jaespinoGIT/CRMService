using CRMService.Helpers.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMService.Models.Binding
{
    public class ChangePasswordBindingModel
    {

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        [JsonConverter(typeof(NoConverter))]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        [JsonConverter(typeof(NoConverter))]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        [JsonConverter(typeof(NoConverter))]
        public string ConfirmPassword { get; set; }

    }
}
