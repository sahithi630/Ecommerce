
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore; // for [Precision]

namespace ProductService.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required, StringLength(200)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue)]
        [Precision(18, 2)]
        public decimal Price { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [StringLength(100)]
        public string Category { get; set; } = string.Empty;

        public bool IsAvailable { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}