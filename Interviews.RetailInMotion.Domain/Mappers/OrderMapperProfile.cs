using AutoMapper;
using Interviews.RetailInMotion.Domain.Entities;
using Interviews.RetailInMotion.Domain.Models;

namespace Interviews.RetailInMotion.Domain.Mappers
{
    public class OrderMapperProfile : Profile
    {
        public OrderMapperProfile()
        {
            CreateMap<CreateOrderModel, Order>();
        }
    }
}
