using System.ComponentModel.DataAnnotations;
using ProductsStore.Models;

namespace ProductsStore.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string ContactPhone { get; set; }
        [Required]
        public string Address { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}