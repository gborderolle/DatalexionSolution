using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.Domain.Entities;

public class Photo
{
    #region Internal

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public DateTime Creation { get; set; } = DateTime.Now;

    public DateTime Update { get; set; } = DateTime.Now;

    // Uniques
    [Required(ErrorMessage = "El campo {0} es requerido")]
    public string URL { get; set; }

    #endregion

    #region External

    public int? CircuitId { get; set; }
    public CircuitParty? Circuit { get; set; }

    public int? CandidateId { get; set; }
    public Candidate? Candidate { get; set; }

    public int? PartyLongId { get; set; }
    public Party? PartyLong { get; set; }

    public int? PartyShortId { get; set; }
    public Party? PartyShort { get; set; }

    public int? WingId { get; set; }
    public Wing? Wing { get; set; }

    public int? SlateId { get; set; }
    public Slate? Slate { get; set; }

    #endregion
}