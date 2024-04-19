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
        public int? TotalPartyVotes { get; set; } = 0;

        // Son los votos desagregados del circuito que fueron ingresados por algún delegado de su Partido
        // Cada partido tiene sus propios ingresos, por eso estos votos están acá (CircuitParty) y no en Circuit
        public int BlankVotes { get; set; } = 0;
        public int NullVotes { get; set; } = 0;
        public int ObservedVotes { get; set; } = 0;
        public int RecurredVotes { get; set; } = 0;
        public bool Step1completed { get; set; } = false;
        public bool Step2completed { get; set; } = false;
        public bool Step3completed { get; set; } = false;
        public int? LastUpdateDelegadoId { get; set; } = null;

        /// <summary>
        /// 1-N
        /// </summary>
        public List<Photo> ListPhotos { get; set; }

        #endregion

    }
}