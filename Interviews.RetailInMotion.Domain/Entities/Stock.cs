namespace Interviews.RetailInMotion.Domain.Entities
{
    public class Stock
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; } = new Product();

        public int QuantityAvailable { get; set; }
    }
}
