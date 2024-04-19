using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatalexionBackend.Core.Domain.Entities
{
    public class Circuit : IId
    {
        #region Internal

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Creation { get; set; } = DateTime.Now;

        public DateTime Update { get; set; } = DateTime.Now;

        public string? Comments { get; set; }

        // Uniques

        public int Number { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")] // n..0 (0=no existe este sin el padre)
        [StringLength(maximumLength: 100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")] // n..0 (0=no existe este sin el padre)
        public string Address { get; set; }
        public string? LatLong { get; set; }        

        #endregion

        #region External

        /// <summary>
        /// N-N
        /// </summary>
        public List<CircuitDelegado> ListCircuitDelegados { get; set; }

        /// <summary>
        /// N-N
        /// </summary>
        public List<CircuitSlate> ListCircuitSlates { get; set; }

        /// <summary>
        /// N-N
        /// </summary>
        public List<CircuitParty> ListCircuitParties { get; set; }

        // -- Vueltas --

        [Required(ErrorMessage = "El campo {0} es requerido")] // n..0 (0=no existe este sin el padre)
        public int MunicipalityId { get; set; }
        public Municipality Municipality { get; set; }

        #endregion

    }
}
