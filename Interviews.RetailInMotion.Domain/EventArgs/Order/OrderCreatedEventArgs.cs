using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
