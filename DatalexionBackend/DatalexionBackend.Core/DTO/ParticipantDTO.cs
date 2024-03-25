using DatalexionBackend.Core.DTO;
using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.DTO;

public class ParticipantDTO
{
    #region Internal

    public int Id { get; set; }

    public DateTime Creation { get; set; } = DateTime.Now;

    public DateTime Update { get; set; } = DateTime.Now;

    public string? Comments { get; set; }

    // Uniques
    public string Name { get; set; }

    #endregion

    #region External

    // -- Vueltas --

    public int SlateId { get; set; }
    public SlateDTO Slate { get; set; }

    #endregion

}
