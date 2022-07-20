using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ProductsStore.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Не указан Email")]
        [Remote("CheckEmail", "Validation", ErrorMessage = "Такая почта уже есть!")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес электронной почты")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Введите свое имя")]
        public string UserName { get; set; }
   
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[0-9]).{8,}$", ErrorMessage = "Пароль должен содержать 1 цифру 1 букву в верхнем регистре и не менее 8 символов")]
        public string Password { get; set; }
   
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}