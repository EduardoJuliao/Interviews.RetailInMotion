using Interviews.RetailInMotion.Domain.Entities;

namespace Interviews.RetailInMotion.Domain.Interfaces.Factories
{
    public interface IOrderFactory
    {
        IOrderFactory BasedOnOrder(Order order);
        IOrderFactory AddProduct(Product product, int quantity);
        IOrderFactory AddDeliveryAddress(Address address);
        IOrderFactory AddBillingAddress(Address address);
        IOrderFactory BillingAddressSameAsDelivery(bool isSame);
        Order Build();
    }
}
