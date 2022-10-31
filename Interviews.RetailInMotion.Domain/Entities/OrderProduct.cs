namespace Interviews.RetailInMotion.Domain.Entities
{
    public class OrderProduct
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Product ProductItem { get; set; } = new Product();
        public int Quantity { get; set; }
    }
}
