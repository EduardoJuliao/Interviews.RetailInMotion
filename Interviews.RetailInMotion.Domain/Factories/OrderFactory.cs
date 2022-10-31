using Interviews.RetailInMotion.Domain.Entities;
using Interviews.RetailInMotion.Domain.Enums;
using Interviews.RetailInMotion.Domain.Interfaces.Factories;
using Interviews.RetailInMotion.Domain.Models;

namespace Interviews.RetailInMotion.Domain.Factories
{
    public class OrderFactory : IOrderFactory
    {
        private Order _newOrder = new Order();

        public IOrderFactory BasedOnOrder(Order order)
        {
            _newOrder = order;
            return this;
        }

        public IOrderFactory AddBillingAddress(Address address)
        {
            if (address == null)
                return this;

            if(_newOrder.OrderAddresses == null)
                _newOrder.OrderAddresses = new List<OrderAddress>();

            var existingAddress = _newOrder.OrderAddresses
                .SingleOrDefault(x => x.Address.AddressType == AddressType.Billing);

            if (existingAddress == null)
            {
                address.AddressType = Enums.AddressType.Billing;
                _newOrder.OrderAddresses.Add(new OrderAddress
                {
                    Order = _newOrder,
                    Address = address
                });
            }
            else
            {
                existingAddress.Address.Street = address.Street;
                existingAddress.Address.PostalCode = address.PostalCode;
            }

            return this;
        }

        public IOrderFactory AddDeliveryAddress(Address address)
        {
            if (address == null)
                return this;

            if (_newOrder.OrderAddresses == null)
                _newOrder.OrderAddresses = new List<OrderAddress>();

            var existingAddress = _newOrder.OrderAddresses
                .SingleOrDefault(x => x.Address.AddressType == AddressType.Delivery);

            if (existingAddress == null)
            {
                address.AddressType = Enums.AddressType.Delivery;
                _newOrder.OrderAddresses.Add(new OrderAddress
                {
                    Order = _newOrder,
                    Address = address
                });
            }
            else
            {
                existingAddress.Address.Street = address.Street;
                existingAddress.Address.PostalCode = address.PostalCode;
            }


            return this;
        }

        public IOrderFactory AddProducts(List<CreateOrderProductModel> products)
        {
            if (products == null)
                return this;

            if (_newOrder.OrderProducts == null)
                _newOrder.OrderProducts = new List<OrderProduct>();

            foreach (var product in products)
            {
                var existingProduct = _newOrder.OrderProducts
                    .SingleOrDefault(x => x.ProductId == product.ProductId);

                if (existingProduct == null)
                {
                    _newOrder.OrderProducts.Add(new OrderProduct
                    {
                        Order = _newOrder, 
                        ProductId = product.ProductId,
                        Quantity = product.Quantity,
                    });
                }
                else
                {
                    existingProduct.Quantity = product.Quantity;
                }
            }

            return this;
        }

        public IOrderFactory BillingAddressSameAsDelivery(bool isSame)
        {
            _newOrder.BillingAddressSameAsDelivery = isSame;
            return this;
        }

        public Order Build()
        {
            if (_newOrder.BillingAddressSameAsDelivery)
            {
                var deliveryAddress = _newOrder.OrderAddresses.Single(x => x.Address.AddressType == Enums.AddressType.Delivery);
                _newOrder.OrderAddresses.Clear();
                deliveryAddress.Address.AddressType = AddressType.Same;
                _newOrder.OrderAddresses.Add(deliveryAddress);
            }
            if (_newOrder.Status == null)
                _newOrder.Status = OrderStatus.Created;

            return _newOrder;
        }
    }
}
