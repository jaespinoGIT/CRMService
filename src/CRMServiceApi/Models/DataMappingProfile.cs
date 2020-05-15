using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CRMService.Core.Domain.Entities;

namespace CRMService.Models
{
    public class DataMappingProfile : Profile
    {
        public DataMappingProfile()
        {
            CreateMap<User, UserModel>()
                .ReverseMap();

            CreateMap<Customer, CustomerModel>()
                    .ReverseMap();

            CreateMap<Role, RoleModel>();

            CreateMap<UserRole, UserRoleModel>()
                .ReverseMap();
        }
    }
}
