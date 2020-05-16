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
    public class EntitiesMappingProfile : Profile
    {
        public EntitiesMappingProfile()
        {
            CreateMap<Customer, CustomerModel>()
                    .ReverseMap().ForMember(um => um.CustomerId, opt => opt.Ignore())
                    .ForMember(um => um.CustomerAudits, opt => opt.Ignore())
                    .ForMember(um => um.Photo, opt => opt.Ignore());

            CreateMap<CustomerAudit, CustomerAuditModel>()
                .ReverseMap();

            CreateMap<User, UserModel>()           
              .ReverseMap();
        }
    }
}
