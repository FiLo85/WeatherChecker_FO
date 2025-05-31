using System.ComponentModel.DataAnnotations;

namespace WeatherChecker_FO.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}
