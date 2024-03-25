using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatalexionBackend.Core.Domain.Entities
{
    public class Slate : IId
    {
        #region Internal

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Creation { get; set; } = DateTime.Now;

        public DateTime Update { get; set; } = DateTime.Now;

        public string? Comments { get; set; }

        // Uniques

        [Required(ErrorMessage = "El campo {0} es requerido")] // n..0 (0=no existe este sin el padre)
        [StringLength(maximumLength: 100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
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

        public int? PhotoId { get; set; }
        public Photo? Photo { get; set; }

        // -- Vueltas --

        [Required(ErrorMessage = "El campo {0} es requerido")] // n..0 (0=no existe este sin el padre)
        public int WingId { get; set; }
        public Wing Wing { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")] // n..0 (0=no existe este sin el padre)
        public int ProvinceId { get; set; }
        public Province Province { get; set; }

        #endregion

    }
}
