using Interviews.RetailInMotion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interviews.RetailInMotion.Domain.Models
{
    public class UpdateAddressModel
    {
        public virtual Address DeliveryAddress { get; set; }
        public virtual Address BillingAddress { get; set; }
        public bool BillingAddressSameAsDelivery { get; set; }
    }
}
