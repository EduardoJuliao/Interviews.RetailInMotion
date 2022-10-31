﻿using Interviews.RetailInMotion.Domain.Enums;
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
        public string Street { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public AddressType AddressType { get; set; }
        public IList<OrderAddress> OrderAddresses { get; set; } = new List<OrderAddress>();
    }
}
