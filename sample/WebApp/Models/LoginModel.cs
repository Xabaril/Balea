using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
