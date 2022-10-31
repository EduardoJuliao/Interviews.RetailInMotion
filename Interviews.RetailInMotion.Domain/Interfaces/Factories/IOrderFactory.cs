using Interviews.RetailInMotion.Domain.Entities;
using Interviews.RetailInMotion.Domain.Models;

namespace Interviews.RetailInMotion.Domain.Interfaces.Factories
{
    public interface IOrderFactory
    {
        IOrderFactory BasedOnOrder(Order order);
        IOrderFactory AddProducts(List<CreateOrderProductModel> products);
        IOrderFactory AddDeliveryAddress(Address address);
        IOrderFactory AddBillingAddress(Address address);
        IOrderFactory BillingAddressSameAsDelivery(bool isSame);
        Order Build();
    }
}
