namespace Interviews.RetailInMotion.Domain.EventArgs.Order
{
    public class OrderCanceledEventArgs
    {
        public OrderCanceledEventArgs(Entities.Order cancelledOrder)
        {
            Order = cancelledOrder;
        }

        public Entities.Order Order { get; }
    }
}
