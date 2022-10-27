using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interviews.RetailInMotion.Domain.Entities
{
    public class Address
    {
        public const int StreetMaxLenght = 256;

        public Guid Id { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
    }
}
