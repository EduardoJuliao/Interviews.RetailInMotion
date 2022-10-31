using Interviews.RetailInMotion.Domain.Enums;

namespace Interviews.RetailInMotion.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public DateTimeOffset? LastUpdatedDate { get; set; }
        public DateTimeOffset? CanceledDate { get; set; }
        public OrderStatus? Status;
        public bool BillingAddressSameAsDelivery { get; set; }

        public IList<OrderAddress> OrderAddresses { get; set; } = new List<OrderAddress>();
        public IList<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

        public double TotalPrice => OrderProducts.Any()
            ? OrderProducts
                .Select(x => x.Product.Price * x.Quantity)
                .Sum()
            : 0d;
    }
}
