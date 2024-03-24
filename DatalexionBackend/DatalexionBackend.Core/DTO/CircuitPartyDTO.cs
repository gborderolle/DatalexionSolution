namespace DatalexionBackend.Core.DTO
{
    public class CircuitPartyDTO
    {
        #region Internal

        public int CircuitId { get; set; }

        public int PartyId { get; set; }

        // Uniques
        public int? Votes { get; set; }

        #endregion

    }
}