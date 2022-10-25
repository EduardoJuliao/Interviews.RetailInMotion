using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interviews.RetailInMotion.Domain.Entities
{
    public class Stock
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }

        public virtual Item Item { get; set; } = new Item();

        public int QuantityAvailable { get; set; }
    }
}
