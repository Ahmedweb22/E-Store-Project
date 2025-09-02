using System.ComponentModel.DataAnnotations;

namespace APITest.DTO
{
    public class DtoOrders
    {
        public int orderId { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [MaxLength(50)]
        public string OrderName { get; set; }
        public ICollection<DtoOrderItems> OrderItems { get; set; } 
    }
    public class DtoOrderItems
    {
        [Required]
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        [Required]
        public decimal Price { get; set; }
        public int quantity { get; set; }
    }

}
