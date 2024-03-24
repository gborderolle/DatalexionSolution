using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.DTO
{
    public class PartyDTO
    {
        #region Internal

        public int Id { get; set; }

        public DateTime Creation { get; set; } = DateTime.Now;

        public DateTime Update { get; set; } = DateTime.Now;

        public string Comments { get; set; }

        // Uniques

        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Color { get; set; }
        public int? Votes { get; set; }

        #endregion

        #region External

        /// <summary>
        /// 0-N
        /// </summary>
        public List<Wing> ListWings { get; set; } = new();

        /// <summary>
        /// N-N
        /// </summary>
        public List<CircuitParty> ListCircuitParties { get; set; }

        public string PhotoLongURL { get; set; }
        public string PhotoShortURL { get; set; }

        // -- Vueltas --

        #endregion

    }
}
