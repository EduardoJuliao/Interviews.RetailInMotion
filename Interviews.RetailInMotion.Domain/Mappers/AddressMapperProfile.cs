using AutoMapper;
using Interviews.RetailInMotion.Domain.Entities;
using Interviews.RetailInMotion.Domain.Models;

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
