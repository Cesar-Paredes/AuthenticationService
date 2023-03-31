

using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Models
{
    public class CSRAgent
    {
        [Key]
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
