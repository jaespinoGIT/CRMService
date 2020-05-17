using CRMService.Core.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMService.Models.Helpers
{
    public interface IModelFactory
    {
        UserModel Create(User appUser);

        RoleModel Create(IdentityRole appRole);
    }
}
