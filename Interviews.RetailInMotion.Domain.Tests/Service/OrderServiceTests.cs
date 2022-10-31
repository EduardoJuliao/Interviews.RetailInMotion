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
        private IStockRepository _stockRepository;
        private IOrderService _orderService;
        private IStockService _stockService;

        private readonly Guid DvdProductId = new Guid("8073a0c8-02eb-44e4-a6ae-2e29294bd0b1");
        private readonly Guid BookProductId = new Guid("e39adf2e-fd84-4fde-bba1-765e10ce8fef");
        private readonly Guid CdProductId = new Guid("fd9272fd-c6e0-4013-a862-145e1d3ef4d3");

        [SetUp]
        public void SetUp()
        {
            _context = EntityFrameworkHelper.CreateDatabase();
            _orderRepository = new OrderRepository(_context);
            _stockRepository = new StockRepository(_context);

            _stockService = new StockService(
                Substitute.For<ILogger<StockService>>(),
                _stockRepository
                );

            _orderService = new OrderService(
                Substitute.For<ILogger<OrderService>>(),
                new OrderFactory(),
                _orderRepository,
                _stockService);

            CreateStubProducts();
        }

        private void CreateStubProducts()
        {
            var dvd = _context.Products.Add(
               new Product
               {
                   Id = DvdProductId,
                   Name = "DVD",
                   Price = 10,
               });

            var book = _context.Products.Add(
               new Product
               {
                   Id = BookProductId,
                   Name = "Book",
                   Price = 5,
               });

            var cd = _context.Products.Add(
                new Product
                {
                    Id = CdProductId,
                    Name = "CD",
                    Price = 2.50,
                });

            _context.Stock.Add(new Stock
            {
                Product = dvd.Entity,
                QuantityAvailable = 50
            });

            _context.Stock.Add(new Stock
            {
                Product = book.Entity,
                QuantityAvailable = 20
            });

            _context.Stock.Add(new Stock
            {
                Product = cd.Entity,
                QuantityAvailable = 1000
            });

            _context.SaveChanges();
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
                Products = new List<CreateOrderProductModel>
                {
                    new CreateOrderProductModel
                    {
                        ProductId = DvdProductId,
                        Quantity = 10
                    }
                },
                DeliveryAddress = new Address
                {
                    PostalCode = "Postal Code 1",
                    Street = "Street 1"
                },
                BillingAddress = new Address
                {
                    PostalCode = "Postal Code 2",
                    Street = "Street 2"
                },
            });

            Assert.AreNotEqual(order.Id, Guid.Empty);
            Assert.AreEqual(OrderStatus.Created, order.Status);
            Assert.AreEqual(2, order.OrderAddresses.Count);
            Assert.AreEqual(1, order.OrderAddresses.Count(x => x.Address.AddressType == AddressType.Delivery));
            Assert.AreEqual(1, order.OrderAddresses.Count(x => x.Address.AddressType == AddressType.Billing));
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
        public async Task ProductsInOrderAreRemovedFromStock()
        {
            var orderToCreate = new CreateOrderModel
            {
                CreationDate = DateTimeOffset.UtcNow,
                Products = new List<CreateOrderProductModel>
                {
                    new CreateOrderProductModel
                    {
                        ProductId = DvdProductId,
                        Quantity = 10
                    }
                },
                DeliveryAddress = new Address
                {
                    PostalCode = "Postal Code 1",
                    Street = "Street 1"
                },
                BillingAddress = new Address
                {
                    PostalCode = "Postal Code 2",
                    Street = "Street 2"
                },
            };

            await _orderService.CreateOrder(orderToCreate);

            Assert.AreEqual(40, _context.Stock.Single(x => x.ProductId == DvdProductId).QuantityAvailable);
        }

        [Test]
        public void ThrowExceptionIfStockDoesntHaveRequiredQuantityOfProducts()
        {
            var orderToCreate = new CreateOrderModel
            {
                CreationDate = DateTimeOffset.UtcNow,
                Products = new List<CreateOrderProductModel>
                {
                    new CreateOrderProductModel
                    {
                        ProductId = DvdProductId,
                        Quantity = 999
                    }
                },
                DeliveryAddress = new Address
                {
                    PostalCode = "Postal Code 1",
                    Street = "Street 1"
                },
                BillingAddress = new Address
                {
                    PostalCode = "Postal Code 2",
                    Street = "Street 2"
                },
            };

            Assert.ThrowsAsync<Exception>(async () => await _orderService.CreateOrder(orderToCreate));
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

        [Test]
        public async Task CanUpdateProducts()
        {
            var order = await _orderService.CreateOrder(new CreateOrderModel
            {
                CreationDate = DateTimeOffset.UtcNow,
                Products = new List<CreateOrderProductModel>
                {
                    new CreateOrderProductModel
                    {
                        ProductId = DvdProductId,
                        Quantity = 1
                    },
                    new CreateOrderProductModel
                    {
                        ProductId = CdProductId,
                        Quantity = 1
                    }
                },
                DeliveryAddress = new Address
                {
                    PostalCode = "Postal Code 1",
                    Street = "Street 1"
                },
                BillingAddress = new Address
                {
                    PostalCode = "Postal Code 2",
                    Street = "Street 2"
                },
            });

            await _orderRepository.UpdateOrder(order);

            //Update Dvd to 5, remove Cd and add Book

            var updatedProducts = new List<CreateOrderProductModel>
            {
                new CreateOrderProductModel { ProductId = DvdProductId, Quantity = 5 },
                new CreateOrderProductModel { ProductId = BookProductId, Quantity = 4 },
            };

            var updatedOrder = await _orderService.UpdateOrderProducts(order.Id, updatedProducts);

            Assert.AreEqual(2, updatedOrder.OrderProducts.Count);
            Assert.AreEqual(5, updatedOrder.OrderProducts.Single(x => x.ProductId == DvdProductId).Quantity);
            Assert.AreEqual(4, updatedOrder.OrderProducts.Single(x => x.ProductId == BookProductId).Quantity);
        }
    }
}
