namespace DatalexionBackend.Core.Domain.Entities
{
    public class CircuitParty
    {
        #region Internal

        public int CircuitId { get; set; }
        public Circuit Circuit { get; set; }

        public int PartyId { get; set; }
        public Party Party { get; set; }

        // Uniques
        public int? Votes { get; set; }

        #endregion

    }
}