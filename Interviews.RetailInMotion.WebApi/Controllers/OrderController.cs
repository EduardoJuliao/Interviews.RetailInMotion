using Interviews.RetailInMotion.Domain.Entities;
using Interviews.RetailInMotion.Domain.Interfaces.Services;
using Interviews.RetailInMotion.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Interviews.RetailInMotion.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{take:int}/{skip:int}")]
        public async Task<IEnumerable<Order>> GetOrders(int take, int skip)
        {
            return await _orderService.GetOrders(take, skip);
        }

        [HttpGet("{orderId:guid}")]
        public async Task<Order> GetOrder(Guid orderId)
        {
            return await _orderService.GetOrder(orderId);
        }

        [HttpPost]
        public async Task<Order> CreateOrder([FromBody]CreateOrderModel model)
        {
            return await _orderService.CreateOrder(model);
        }

        [HttpPut("address/{orderId:guid}")]
        public async Task<Order> UpdateOrderAddress(Guid orderId, [FromBody] UpdateAddressModel model)
        {
            return await _orderService.UpdateAddress(orderId, model);
        }

        [HttpPut("product/{orderId:guid}")]
        public async Task<Order> UpdateProducts(Guid orderId, [FromBody] IEnumerable<CreateOrderProductModel> products)
        {
            return await _orderService.UpdateOrderProducts(orderId, products);
        }

        [HttpDelete("{orderId:guid}")]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            await _orderService.CancelOrder(orderId);
            return Ok();
        }
    }
}
