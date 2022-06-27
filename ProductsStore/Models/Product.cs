using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsStore.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Товар не может быть бесплатным")]
        public decimal Price { get; set; }
        public string CreateDate { get; set; } = DateTime.Now.ToString("dd/MM/yyyy/ HH:mm");
        public string UpdateDate { get; set; }
        [Required]
        public string ImgUrl { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public int BrandId { get; set; }
        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }
    }
}