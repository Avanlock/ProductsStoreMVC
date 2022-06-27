using System.ComponentModel.DataAnnotations;

namespace ProductsStore.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}