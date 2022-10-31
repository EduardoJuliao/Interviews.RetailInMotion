using AutoMapper;
using Interviews.RetailInMotion.Domain.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
