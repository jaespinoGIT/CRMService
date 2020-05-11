﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMService.Models
{
    public class UserModel
    {        
        public int UserId { get; set; }
        
        [StringLength(255, MinimumLength = 3)]
        public string Name { get; set; }
      
        [StringLength(20, MinimumLength = 5)]
        public string Login { get; set; }
        [Required]
        public bool Active { get; set; }        
        public ICollection<UserRoleModel> UserRoles { get; set; }
    }
}