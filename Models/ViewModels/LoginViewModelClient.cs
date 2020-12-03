using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    public class LoginViewModelClient
    {
        [Key]
        [Display(Name = "Email đăng nhập(*)")]
        [Required(ErrorMessage = "Bạn phải nhập Email")]
        public string Email { get; set; }

        [Display(Name = "Mật khẩu(*)")]
        [Required(ErrorMessage = "Bạn phải nhập mật khẩu")]
        public string Password { get; set; }
    }
}