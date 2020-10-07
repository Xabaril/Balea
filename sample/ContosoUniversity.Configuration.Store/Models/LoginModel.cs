using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Configuration.Store.Models
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
