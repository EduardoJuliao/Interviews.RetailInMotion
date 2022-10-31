using Interviews.RetailInMotion.Domain.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interviews.RetailInMotion.Domain.Tests.EntityTests
{
    [Category("Entity")]
    internal class OrderEntityTest
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void CalculateFullPriceCorrectly()
        {
            var order = new Order();
            order.OrderProducts.Add(new OrderProduct()
            {
                ProductItem = new Product() { Price = 5},
                Quantity = 1
            });

            order.OrderProducts.Add(new OrderProduct()
            {
                ProductItem = new Product() { Price = 2.50 },
                Quantity = 3
            });

            Assert.AreEqual(12.50, order.TotalPrice, 
                "Incorrect Total Price in order! Expected {totalPrice} received {expectedTotalPrice}",
                order.TotalPrice,
                12.50
                );
        }
    }
}
