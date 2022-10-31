namespace Interviews.RetailInMotion.Domain.Entities
{
    public class OrderProduct
    {
        public int Quantity { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
