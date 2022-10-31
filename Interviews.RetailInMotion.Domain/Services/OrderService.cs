﻿using AutoMapper;
using Interviews.RetailInMotion.Domain.Entities;
using Interviews.RetailInMotion.Domain.Enums;
using Interviews.RetailInMotion.Domain.EventArgs.Order;
using Interviews.RetailInMotion.Domain.Interfaces.Factories;
using Interviews.RetailInMotion.Domain.Interfaces.Repositories;
using Interviews.RetailInMotion.Domain.Interfaces.Services;
using Interviews.RetailInMotion.Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interviews.RetailInMotion.Domain.Services
{
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IOrderFactory _orderFactory;
        private readonly IOrderRepository _orderRepository;

        public delegate void OrderCanceledEventHandler(object sender, OrderCanceledEventArgs e);
        public event OrderCanceledEventHandler OrderCanceledEvent;

        public delegate void OrderCreatedEventHandler(object sender, OrderCreatedEventArgs e);
        public event OrderCreatedEventHandler OrderCreatedEvent;

        public OrderService(
            ILogger<OrderService> logger,
            IOrderFactory orderFactory,
            IOrderRepository orderRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _orderFactory = orderFactory ?? throw new ArgumentNullException(nameof(orderFactory));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task<Order> CancelOrder(Guid orderId)
        {
            var order = await _orderRepository.GetOrder(orderId);

            if (order.Status != OrderStatus.Created && order.Status != OrderStatus.WaitingDeparture)
            {
                throw new Exception("Order cannot be cancelled because already is in transit or delivered.");
            }

            order.Status = OrderStatus.Canceled;
            order.LastUpdatedDate = DateTimeOffset.UtcNow;
            order.CanceledDate = DateTimeOffset.UtcNow;

            await _orderRepository.UpdateOrder(order);

            OrderCanceledEvent?.Invoke(this, new OrderCanceledEventArgs(order));

            return order;
        }

        public async Task<Order> CreateOrder(CreateOrderModel orderModel)
        {
            var newOrder = _orderFactory
                .AddBillingAddress(orderModel.BillingAddress)
                .AddDeliveryAddress(orderModel.DeliveryAddress)
                .BillingAddressSameAsDelivery(orderModel.BillingAddressSameAsDelivery)
                .Build();

            var order = await _orderRepository.CreateOrder(newOrder);

            OrderCreatedEvent?.Invoke(this, new OrderCreatedEventArgs(order));

            return order;
        }

        public async Task<Order> GetOrder(Guid id)
        {
            return await _orderRepository.GetOrder(id);
        }

        public async Task<IEnumerable<Order>> GetOrders(int take = 20, int skip = 0)
        {
            return await _orderRepository.GetOrders(take, skip);
        }

        public async Task<Order> UpdateAddress(Guid orderId, UpdateAddressModel addressModel)
        {
            var order = await _orderRepository.GetOrder(orderId);

            if (order.Status != OrderStatus.Created && order.Status != OrderStatus.WaitingDeparture)
                throw new Exception("Cannot change order address as has already in delivery");

            _orderFactory
                .BasedOnOrder(order)
                .AddDeliveryAddress(addressModel.DeliveryAddress)
                .AddBillingAddress(addressModel.BillingAddress)
                .BillingAddressSameAsDelivery(addressModel.BillingAddressSameAsDelivery)
                .Build();
            order.LastUpdatedDate = DateTimeOffset.UtcNow;

            return await _orderRepository.UpdateOrder(order);
        }

        public Task<Order> UpdateOrderProducts(Guid orderId, IEnumerable<Product> products)
        {
            throw new NotImplementedException();
        }
    }
}
