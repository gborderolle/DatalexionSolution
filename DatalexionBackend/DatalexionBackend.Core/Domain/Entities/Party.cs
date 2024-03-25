using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatalexionBackend.Core.Domain.Entities
{
    public class Party : IId
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
        [StringLength(maximumLength: 6, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
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

        public int? PhotoLongId { get; set; }
        public Photo PhotoLong { get; set; }

        public int? PhotoShortId { get; set; }
        public Photo PhotoShort { get; set; }

        // -- Vueltas --

        #endregion

    }
}
