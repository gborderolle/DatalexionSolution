using System.ComponentModel.DataAnnotations;

namespace DatalexionBackend.Core.DTO
{
    public class DatalexionUserLoginDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
