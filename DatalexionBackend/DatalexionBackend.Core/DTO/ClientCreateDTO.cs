using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DatalexionBackend.Core.Domain.IdentityEntities;

namespace DatalexionBackend.Core.DTO
{
    public class ClientCreateDTO
    {
        #region Internal

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Creation { get; set; } = DateTime.Now;

        public DateTime Update { get; set; } = DateTime.Now;

        public string? Comments { get; set; }

        // Uniques

        public string Name { get; set; }

        #endregion

        #region External

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int PartyId { get; set; }

        /// <summary>
        /// 0-N
        /// </summary>
        public List<DatalexionUser> ListUsers { get; set; } = new();

        /// <summary>
        /// 0-N
        /// </summary>
        public List<DelegadoDTO> ListDelegados { get; set; } = new();

        #endregion

    }
}
