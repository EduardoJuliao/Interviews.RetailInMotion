namespace Interviews.RetailInMotion.Domain.Entities
{
    public class Product
    {
        public const short NameMaxLength = 256;

        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public double Price { get; set; }

        public Guid StockId { get; set; }
        public Stock Stock { get; set; }

        public IList<OrderProduct> OrderProduct { get; set; } = new List<OrderProduct>();
    }
}
