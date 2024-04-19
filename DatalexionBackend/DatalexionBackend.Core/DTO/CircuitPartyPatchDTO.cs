using Microsoft.AspNetCore.Http;

namespace DatalexionBackend.Core.DTO
{
    public class CircuitPartyPatchDTO
    {
        #region Internal

        public int CircuitId { get; set; }
        public int PartyId { get; set; }

        // Uniques
        public int? TotalPartyVotes { get; set; }

        public int BlankVotes { get; set; }
        public int NullVotes { get; set; }
        public int ObservedVotes { get; set; }
        public int RecurredVotes { get; set; }
        public bool Step1completed { get; set; } = false;
        public bool Step2completed { get; set; } = false;
        public bool Step3completed { get; set; } = false;
        public int? LastUpdateDelegadoId { get; set; }

        #endregion

        #region External

        public List<IFormFile> Photos { get; set; } = new();

        #endregion

    }
}
