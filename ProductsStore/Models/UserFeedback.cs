using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsStore.Models
{
    public class UserFeedback
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Заполните поле!")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Заполните поле!")]
        public string Feedback { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [Required(ErrorMessage = "Заполните поле!")]
        public int ProductEvaluation { get; set; }
    }
}