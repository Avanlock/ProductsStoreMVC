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
        [Required(ErrorMessage = "Заполните поле!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Cлишком короткое имя!")]

        public string Name { get; set; }
        [Required(ErrorMessage = "Заполните поле!")]
        [Range(50, int.MaxValue, ErrorMessage = "Товар не может стоить меньше 50 тг")]
        public decimal Price { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
        public string ImgUrl { get; set; }
        [Required(ErrorMessage = "Выберите категорию!")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        [Required(ErrorMessage = "Выберите бренд!")]
        public int BrandId { get; set; }
        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }
    }
}