namespace Interviews.RetailInMotion.Domain.EventArgs.Order
{
    public class OrderCreatedEventArgs
    {
        public OrderCreatedEventArgs(Entities.Order cancelledOrder)
        {
            Order = cancelledOrder;
        }

        public Entities.Order Order { get; }
    }
}
