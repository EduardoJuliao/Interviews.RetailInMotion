namespace Interviews.RetailInMotion.Domain.Entities
{
    public class OrderAddress
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public Guid AddressId { get; set; }
        public Address Address { get; set; }
    }
}
