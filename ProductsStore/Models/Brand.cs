using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ProductsStore.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Заполните поле!")]
        [Remote("CheckBrand", "Validation", ErrorMessage = "Такой бренд уже есть")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Заполните поле!")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес электронной почты")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Заполните поле!")]
        [Remote("CheckDate", "Validation", ErrorMessage = "Дата введена некорректно!")]
        public DateTime FoundationDate{ get; set; }
    }
}

