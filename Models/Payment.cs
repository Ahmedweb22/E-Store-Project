using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APITest.Models
{
    public class Payment
{
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public string PaymentStatus { get; set; } 
        public string StripeSessionId { get; set; }
        [ForeignKey(nameof(orders))]
        public int OrderId { get; set; } 
        public virtual Order orders { get; set; }
    }
}
