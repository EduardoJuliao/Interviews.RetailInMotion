using AutoMapper;
using Interviews.RetailInMotion.Domain.Entities;
using Interviews.RetailInMotion.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interviews.RetailInMotion.Domain.Mappers
{
    public class AddressMapperProfile: Profile
    {
        public AddressMapperProfile()
        {
            CreateMap<CreateAddressModel, Address>();
        }
    }
}
