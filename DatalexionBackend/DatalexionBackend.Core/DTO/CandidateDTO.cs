namespace DatalexionBackend.Core.DTO;

public class CandidateDTO
{
    #region Internal

    public int Id { get; set; }

    public DateTime Creation { get; set; } = DateTime.Now;

    public DateTime Update { get; set; } = DateTime.Now;

    public string? Comments { get; set; }

    // Uniques

    public string Name { get; set; }

    #endregion

    public string PhotoURL { get; set; }

    #region External

    #endregion

}