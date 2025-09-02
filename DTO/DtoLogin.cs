using System.ComponentModel.DataAnnotations;

namespace APITest.DTO
{
    public class DtoLogin
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
