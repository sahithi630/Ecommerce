using OrderService.DTOs;
using OrderService.Models;

namespace OrderService.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync();
        Task<IEnumerable<OrderResponseDto>> GetUserOrdersAsync(string userId);
        Task<OrderResponseDto?> GetOrderByIdAsync(int id);
        Task<OrderResponseDto> CreateOrderAsync(string userId, CreateOrderDto createOrderDto);
        Task<OrderResponseDto?> UpdateOrderStatusAsync(int id, OrderStatus status);
        Task<bool> CancelOrderAsync(int id, string userId);
    }
}