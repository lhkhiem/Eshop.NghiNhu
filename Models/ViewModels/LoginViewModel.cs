using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    public class LoginViewModel
    {
        [Key]
        [Display(Name = "Email(*)")]
        [Required(ErrorMessage = "Nhập email ")]
        public string Email { get; set; }

        [Display(Name = "Mật khẩu(*)")]
        [Required(ErrorMessage = "Nhập mật khẩu")]
        public string Password { get; set; }
    }
}