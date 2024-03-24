using System.ComponentModel.DataAnnotations;

namespace DatalexionBackend.Core.Helpers
{
    public class UserCredential
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
