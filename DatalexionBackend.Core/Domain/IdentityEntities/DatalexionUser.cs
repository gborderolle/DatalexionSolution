using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DatalexionBackend.Core.Domain.Entities;

public class DatalexionUser : IdentityUser
{
    public string Name { get; set; }

    public DateTime Creation { get; set; } = DateTime.Now;

    public DateTime Update { get; set; } = DateTime.Now;

    // -- Vueltas --

    [Required(ErrorMessage = "El campo {0} es requerido")] // n..0 (0=no existe este sin el padre)
    public int ClientId { get; set; }
    public Client Client { get; set; }

}