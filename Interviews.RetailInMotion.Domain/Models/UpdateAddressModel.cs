using Interviews.RetailInMotion.Domain.Entities;

namespace Interviews.RetailInMotion.Domain.Models
{
    public class UpdateAddressModel
    {
        public virtual Address DeliveryAddress { get; set; }
        public virtual Address BillingAddress { get; set; }
        public bool BillingAddressSameAsDelivery { get; set; }
    }
}
