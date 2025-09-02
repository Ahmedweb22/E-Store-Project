using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APITest.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(orders))]
        public int OrderId { get; set; }
        public virtual Order? orders { get; set; }
        [ForeignKey(nameof(items))]
        public int ItemId { get; set; }
        public virtual Item? items { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
