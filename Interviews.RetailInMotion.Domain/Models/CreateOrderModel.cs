using Interviews.RetailInMotion.Domain.Entities;
using Interviews.RetailInMotion.Domain.Enums;

namespace Interviews.RetailInMotion.Domain.Models
{
    public class CreateOrderModel
    {
        public OrderStatus Status = OrderStatus.Created;

        public DateTimeOffset CreationDate { get; set; }
        public DateTimeOffset LastUpdated { get; set; }

        public bool BillingAddressSameAsDelivery { get; set; }
        public virtual Address DeliveryAddress { get; set; } = new Address();
        public virtual Address BillingAddress { get; set; } = new Address();
    }
}
