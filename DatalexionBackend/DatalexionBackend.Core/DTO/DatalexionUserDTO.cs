using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DatalexionBackend.Core.DTO;

public class DatalexionUserDTO : IdentityUser
{
    public required string Name { get; set; }

    public DateTime Creation { get; set; } = DateTime.Now;

    public DateTime Update { get; set; } = DateTime.Now;

    // -- Vueltas --

    [Required(ErrorMessage = "El campo {0} es requerido")] // n..0 (0=no existe este sin el padre)
    public int ClientId { get; set; }
    public ClientDTO Client { get; set; }

}