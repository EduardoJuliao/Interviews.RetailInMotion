namespace Interviews.RetailInMotion.Domain.Enums
{
    public enum OrderStatus
    {
        Created = 1,
        WaitingDeparture = 2,
        InTrasit = 3,
        Delivered = 4,
        WaitingForReturn = 5,
        InTransitReturn = 6,
        Returned = 7,
        Canceled = 8
    }
}
