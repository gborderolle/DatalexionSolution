using System.ComponentModel.DataAnnotations;

namespace DatalexionBackend.Core.DTO;
public class DelegadoLoginDTO
{
    [Required]
    public string CI { get; set; }
}
