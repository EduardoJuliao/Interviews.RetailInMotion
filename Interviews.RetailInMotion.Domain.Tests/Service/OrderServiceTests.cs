using Microsoft.Extensions.Logging;
using Interviews.RetailInMotion.Domain.Interfaces.Services;
using Interviews.RetailInMotion.Domain.Services;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interviews.RetailInMotion.Repository;
using Interviews.RetailInMotion.Domain.Tests.Helpers;
using Interviews.RetailInMotion.Domain.Interfaces.Repositories;
using Interviews.RetailInMotion.Domain.Entities;
using Interviews.RetailInMotion.Domain.Models;
using Interviews.RetailInMotion.Domain.Factories;
using Interviews.RetailInMotion.Domain.Enums;

namespace Interviews.RetailInMotion.Domain.Tests.Service
{
    [Category("Service")]
    public class OrderServiceTests
    {
        private ApplicationDbContext _context;
        private IOrderRepository _orderRepository;
        private IOrderService _orderService;

        [SetUp]
        public void SetUp()
        {
            _context = EntityFrameworkHelper.CreateDatabase();
            _orderRepository = new OrderRepository(_context);

            _orderService = new OrderService(
                Substitute.For<ILogger<OrderService>>(),
                new OrderFactory(),
                _orderRepository);
        }

        [Test]
        public async Task GetLatestOrdersCreated()
        {
            var firstDate = DateTimeOffset.Now;

            _context.Orders.AddRange(
                Enumerable.Range(0, 50).Select(x => new Order
                {
                    CreationDate = firstDate.AddDays(x)
                }));

            await _context.SaveChangesAsync();

            var orders = await _orderService.GetOrders();

            Assert.That(orders.Count, Is.EqualTo(20));
            Assert.That(orders.First().CreationDate, Is.EqualTo(firstDate.AddDays(49)));
        }

        [Test]
        public async Task CanCreateOrder()
        {
            var order = await _orderService.CreateOrder(new CreateOrderModel
            {
                CreationDate = DateTimeOffset.UtcNow,
                DeliveryAddress = new Address
                {
                    PostalCode = "Postal Code 1",
                    Street = "Street 1"
                },
                BillingAddress = new Address
                {
                    PostalCode = "Postal Code 2",
                    Street = "Street 2"
                }
            });

            Assert.AreNotEqual(order.Id, Guid.Empty);
            Assert.AreEqual(OrderStatus.Created, order.Status);
        }

        [Test]
        public async Task OnlyOneAddressIsSavedWhenBillingAddressIsTheSameAsDelivery()
        {
            var orderToCreate = new CreateOrderModel
            {
                CreationDate = DateTimeOffset.UtcNow,
                BillingAddressSameAsDelivery = true,
                DeliveryAddress = new Address
                {
                    PostalCode = "Postal Code 1",
                    Street = "Street 1"
                },
                BillingAddress = new Address
                {
                    PostalCode = "Postal Code 2",
                    Street = "Street 2"
                }
            };

            var order = await _orderService.CreateOrder(orderToCreate);

            Assert.AreNotEqual(order.Id, Guid.Empty);
            Assert.True(order.OrderAddresses.Count() == 1);
            Assert.AreEqual(order.OrderAddresses[0].Address.Street, orderToCreate.DeliveryAddress.Street);
            Assert.AreEqual(order.OrderAddresses[0].Address.PostalCode, orderToCreate.DeliveryAddress.PostalCode);
            Assert.AreEqual(order.OrderAddresses[0].Address.AddressType, AddressType.Same);
        }

        [Test]
        public async Task CanUpdateDeliveryAddress()
        {
            var order = new Order
            {
                CreationDate = DateTimeOffset.UtcNow,
                BillingAddressSameAsDelivery = false,
                Status = OrderStatus.Created,
                OrderAddresses = new List<OrderAddress>
                   {
                       new OrderAddress
                       {
                           Address = new Address
                           {
                               PostalCode = "Postal Code 1",
                               Street = "Street 1"
                           }
                       },
                       new OrderAddress
                       {
                           Address = new Address
                           {
                               PostalCode = "Postal Code 2",
                               Street = "Street 2"
                           }
                       }
                   }
            };

            await _orderRepository.CreateOrder(order);

            var updatedAddress = new UpdateAddressModel
            {
                DeliveryAddress = new Address
                {
                    PostalCode = "Updated Postal Code 1",
                    Street = "Updated Street 1"
                },
                BillingAddress = new Address
                {
                    PostalCode = "Updated Postal Code 2",
                    Street = "Updated Street 2"
                }
            };

            var updateOrder = await _orderService.UpdateAddress(order.Id, updatedAddress);

            Assert.AreEqual("Updated Postal Code 1",
                updateOrder.OrderAddresses.Single(x => x.Address.AddressType == AddressType.Delivery).Address.PostalCode);
            Assert.AreEqual("Updated Street 1",
                updateOrder.OrderAddresses.Single(x => x.Address.AddressType == AddressType.Delivery).Address.Street);

            Assert.AreEqual("Updated Postal Code 2",
                updateOrder.OrderAddresses.Single(x => x.Address.AddressType == AddressType.Billing).Address.PostalCode);
            Assert.AreEqual("Updated Street 2",
                updateOrder.OrderAddresses.Single(x => x.Address.AddressType == AddressType.Billing).Address.Street);
        }

        [Test]
        public async Task UpdateDeliveryAddressAsTheSameAsBilling()
        {
            var order = new Order
            {
                CreationDate = DateTimeOffset.UtcNow,
                BillingAddressSameAsDelivery = false,
                Status = OrderStatus.Created,
                OrderAddresses = new List<OrderAddress>
                   {
                       new OrderAddress
                       {
                           Address = new Address
                           {
                               PostalCode = "Postal Code 1",
                               Street = "Street 1",
                               AddressType = AddressType.Delivery
                           }
                       },
                       new OrderAddress
                       {
                           Address = new Address
                           {
                               PostalCode = "Postal Code 2",
                               Street = "Street 2",
                               AddressType = AddressType.Billing
                           }
                       }
                   }
            };

            await _orderRepository.CreateOrder(order);

            var updatedAddress = new UpdateAddressModel
            {
                BillingAddressSameAsDelivery = true
            };

            var updateOrder = await _orderService.UpdateAddress(order.Id, updatedAddress);

            Assert.AreEqual(1, updateOrder.OrderAddresses.Count);
            Assert.AreEqual(AddressType.Same, updateOrder.OrderAddresses[0].Address.AddressType);
            Assert.AreEqual("Street 1", updateOrder.OrderAddresses[0].Address.Street);
            Assert.AreEqual("Postal Code 1", updateOrder.OrderAddresses[0].Address.PostalCode);
        }

        [TestCase(OrderStatus.WaitingDeparture)]
        [TestCase(OrderStatus.Created)]
        public async Task CanCancelOrderInCorrectStatus(OrderStatus orderStatus)
        {
            var order = new Order
            {
                Status = orderStatus
            };

            await _orderRepository.CreateOrder(order);

            var canceledOrder = await _orderService.CancelOrder(order.Id);

            Assert.AreEqual(OrderStatus.Canceled, canceledOrder.Status);
            Assert.AreNotEqual(null, canceledOrder.CanceledDate);
        }

        [TestCase(OrderStatus.InTrasit)]
        [TestCase(OrderStatus.Canceled)]
        [TestCase(OrderStatus.Delivered)]
        [TestCase(OrderStatus.InTransitReturn)]
        [TestCase(OrderStatus.WaitingForReturn)]
        [TestCase(OrderStatus.Returned)]
        public async Task CannotCanceledOrderInFollowingStatus(OrderStatus orderStatus)
        {
            var order = new Order
            {
                Status = orderStatus
            };

            await _orderRepository.CreateOrder(order);

            Assert.ThrowsAsync<Exception>(async () => await _orderService.CancelOrder(order.Id));
        }
    }
}
