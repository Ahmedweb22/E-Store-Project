using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APITest.Models
{
    public class Item
    {
        [Key]   
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public double Price { get; set; }
        public string? Notes { get; set; }
        public byte[]? Image { get; set; }

        [ForeignKey(nameof(category))]
        public int CategoryId { get; set; }
        public virtual Category category { get; set; }
        public bool IsActive { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }
    }
}
