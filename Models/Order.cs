using System.ComponentModel.DataAnnotations;

namespace APITest.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
    }
}
