using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.DTO
{
    public class SlateDTO
    {
        #region Internal

        public int Id { get; set; }

        public DateTime Creation { get; set; } = DateTime.Now;

        public DateTime Update { get; set; } = DateTime.Now;

        public string? Comments { get; set; }

        // Uniques

        public string Name { get; set; }
        public string Color { get; set; }
        public int? Votes { get; set; }

        #endregion

        #region External

        /// <summary>
        /// 0-N
        /// </summary>
        public List<Participant> ListParticipants { get; set; } = new();

        /// <summary>
        /// N-N
        /// </summary>
        public List<CircuitSlate> ListCircuitSlates { get; set; }

        public int? CandidateId { get; set; }
        public Candidate Candidate { get; set; }

        public string PhotoURL { get; set; }

        // -- Vueltas --

        public int WingId { get; set; }
        public Wing Wing { get; set; }

        public int ProvinceId { get; set; }
        public Province Province { get; set; }

        #endregion

    }
}
