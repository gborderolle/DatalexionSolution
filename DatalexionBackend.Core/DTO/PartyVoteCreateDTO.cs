namespace DatalexionBackend.Core.DTO
{
    public class CircuitPartyCreateDTO
    {
        #region Internal

        public int CircuitId { get; set; }

        public int PartyId { get; set; }

        // Uniques
        public int? Votes { get; set; }

        #endregion

    }
}