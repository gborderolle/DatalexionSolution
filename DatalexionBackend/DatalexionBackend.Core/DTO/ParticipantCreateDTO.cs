using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DatalexionBackend.Core.DTO;
using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.DTO;
public class ParticipantCreateDTO
{
    #region Internal

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public DateTime Creation { get; set; } = DateTime.Now;

    public DateTime Update { get; set; } = DateTime.Now;

    public string? Comments { get; set; }

    // Uniques

    [Required(ErrorMessage = "El campo {0} es requerido")] // n..0 (0=no existe este sin el padre)
    [StringLength(maximumLength: 100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
    public string Name { get; set; }

    #endregion

    #region External

    // -- Vueltas --

    [Required(ErrorMessage = "El campo {0} es requerido")] // n..0 (0=no existe este sin el padre)
    public int SlateId { get; set; }
    public SlateDTO Slate { get; set; }

    #endregion

}
