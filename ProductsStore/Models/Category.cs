using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ProductsStore.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Заполните поле!")]
        [Remote("CheckCategory", "Validation", ErrorMessage = "Такая категория уже есть")]
        public string Name { get; set; }
    }
}