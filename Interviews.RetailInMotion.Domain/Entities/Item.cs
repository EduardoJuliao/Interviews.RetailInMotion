using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interviews.RetailInMotion.Domain.Entities
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public double Price { get; set; }
    }
}
