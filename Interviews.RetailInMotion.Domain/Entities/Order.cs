using Interviews.RetailInMotion.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interviews.RetailInMotion.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public DateTimeOffset LastUpdated { get; set; }
        public OrderStatus Status;

        public bool DeliveryAddressSameAsBilling { get; set; }
        public virtual Address DeliveryAddress { get; set; } = new Address();
        public virtual Address BillingAddress { get; set; } = new Address();

        public virtual List<OrderProduct> OrderItems { get; private set; } = new List<OrderProduct>();
        public double TotalPrice=> OrderItems.Any()
            ? OrderItems
                .Select(x => x.Item.Price * x.Quantity)
                .Sum()
            : 0d;
    }
}
