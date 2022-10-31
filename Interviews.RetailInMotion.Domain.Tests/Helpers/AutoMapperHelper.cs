using AutoMapper;
using Interviews.RetailInMotion.Domain.Mappers;

namespace Interviews.RetailInMotion.Domain.Tests.Helpers
{
    public static class AutoMapperHelper
    {
        public static IMapper CreateMapper()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<OrderMapperProfile>();
                cfg.AddProfile<AddressMapperProfile>();
            }).CreateMapper();
        }
    }
}
