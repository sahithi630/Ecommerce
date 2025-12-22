using OrderService.DTOs;
using OrderService.Models;
using OrderService.Repositories;

namespace OrderService.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync()
        {
            var orders = await _repository.GetAllAsync();
            return orders.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<OrderResponseDto>> GetUserOrdersAsync(string userId)
        {
            var orders = await _repository.GetByUserIdAsync(userId);
            return orders.Select(MapToResponseDto);
        }

        public async Task<OrderResponseDto?> GetOrderByIdAsync(int id)
        {
            var order = await _repository.GetByIdAsync(id);
            return order == null ? null : MapToResponseDto(order);
        }

        public async Task<OrderResponseDto> CreateOrderAsync(string userId, CreateOrderDto createOrderDto)
        {
            var order = new Order
            {
                UserId = userId,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                OrderItems = createOrderDto.OrderItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity
                }).ToList()
            };

            order.TotalAmount = order.OrderItems.Sum(item => item.Subtotal);

            var createdOrder = await _repository.CreateAsync(order);
            return MapToResponseDto(createdOrder);
        }

        public async Task<OrderResponseDto?> UpdateOrderStatusAsync(int id, OrderStatus status)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null)
                return null;

            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;

            var updatedOrder = await _repository.UpdateAsync(order);
            return updatedOrder == null ? null : MapToResponseDto(updatedOrder);
        }

        public async Task<bool> CancelOrderAsync(int id, string userId)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null || order.UserId != userId)
                return false;

            if (order.Status != OrderStatus.Pending)
                return false; // Can only cancel pending orders

            order.Status = OrderStatus.Cancelled;
            order.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(order);
            return true;
        }

        private static OrderResponseDto MapToResponseDto(Order order)
        {
            return new OrderResponseDto
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                OrderItems = order.OrderItems.Select(item => new OrderItemResponseDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Subtotal = item.Subtotal
                }).ToList()
            };
        }
    }
}