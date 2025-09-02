using APITest.DTO;
using APITest.Migrations;
using APITest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APITest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly Data.AppDbContext _context;
        public OrderController(Data.AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("{orederId:int}")]
        public async Task<IActionResult> GetOrder(int orederId)
        {
            var order = await _context.Orders.Where(o => o.Id == orederId).FirstOrDefaultAsync();
            if (order == null)
            {
                return NotFound($"Order.Id {orederId} not exists");
            }
            else
            {
                DtoOrders dtoOrders = new()
                {
                    orderId = order.Id, 
                    CreatedDate = order.CreatedDate
                };
                if (order.OrderItems != null && order.OrderItems.Any())
                {
                    dtoOrders.OrderItems = new List<DtoOrderItems>();
                    foreach (var item in order.OrderItems)
                    {
                         DtoOrderItems dtoItem = new()
                        {
                            ItemId = item.ItemId,
                            ItemName = item.items?.Name, 
                            quantity = 1,
                            Price = item.Price
                        };
                        dtoOrders.OrderItems.Add(dtoItem);
                    }
                }
                return Ok(dtoOrders); 
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _context.Orders.ToListAsync();
            return Ok(orders);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(DtoOrders orders )
        {
            if (ModelState.IsValid)
            {
              Order od = new()
                {
                    CreatedDate = orders.CreatedDate,
                    OrderItems = new List<OrderItem>()
                };
                if (orders.OrderItems != null && orders.OrderItems.Any())
                {
                    foreach (var item in orders.OrderItems)
                    {
                        var orderItem = new OrderItem
                        {
                            ItemId = item.ItemId,
                            Price = item.Price
                        };
                        od.OrderItems.Add(orderItem);
                    }
                }
                _context.Orders.Add(od);
                await _context.SaveChangesAsync();
                orders.orderId = od.Id;
                return Ok(od);
            }
            return BadRequest();

        }
    }
}
