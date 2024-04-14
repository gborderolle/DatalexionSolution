using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.DTO
{
    public class CircuitDTO
    {
        #region Internal

        public int Id { get; set; }

        public DateTime Creation { get; set; } = DateTime.Now;

        public DateTime Update { get; set; } = DateTime.Now;

        public string? Comments { get; set; }

        // Uniques

        public int Number { get; set; }
        public required string Name { get; set; }
        public string Address { get; set; }
        public string? LatLong { get; set; }
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

        /// <summary>
        /// N-N
        /// </summary>
        public List<CircuitDelegadoDTO> ListCircuitDelegados { get; set; }

        /// <summary>
        /// N-N
        /// </summary>
        public List<CircuitSlateDTO> ListCircuitSlates { get; set; }

        /// <summary>
        /// N-N
        /// </summary>
        public List<CircuitPartyDTO> ListCircuitParties { get; set; }

        public List<string> ListPhotosURL { get; set; }

        // -- Vueltas --

        public int MunicipalityId { get; set; }
        public MunicipalityDTO Municipality { get; set; }

        #endregion

    }
}
