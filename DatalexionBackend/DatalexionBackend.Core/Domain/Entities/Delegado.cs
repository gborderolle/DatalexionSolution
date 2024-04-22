using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatalexionBackend.Core.Domain.Entities
{
    public class Delegado : IId
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
        public string CI { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")] // n..0 (0=no existe este sin el padre)
        public required string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        #endregion

        #region External

        /// <summary>
        /// N-N
        /// </summary>
        public List<CircuitDelegado> ListCircuitDelegados { get; set; } = new();

        /// <summary>
        /// 1-N
        /// </summary>
        public List<Municipality> ListMunicipalities { get; set; } = new();

        // -- Vueltas --

        [Required(ErrorMessage = "El campo {0} es requerido")] // n..0 (0=no existe este sin el padre)
        public int ClientId { get; set; }
        public Client Client { get; set; }

        #endregion

    }
}
