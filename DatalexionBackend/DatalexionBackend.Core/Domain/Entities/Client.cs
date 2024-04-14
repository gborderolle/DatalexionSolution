using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DatalexionBackend.Core.Domain.IdentityEntities;

namespace DatalexionBackend.Core.Domain.Entities
{
    public class Client : IId
    {
        #region Internal

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Creation { get; set; } = DateTime.Now;

        public DateTime Update { get; set; } = DateTime.Now;

        public string? Comments { get; set; }

        // Uniques

        public required string Name { get; set; }

        #endregion

        #region External

        public int PartyId { get; set; }
        public Party Party { get; set; }

        /// <summary>
        /// 0-N
        /// </summary>
        public List<DatalexionUser> ListUsers { get; set; } = new();

        /// <summary>
        /// 0-N
        /// </summary>
        public List<Delegado> ListDelegados { get; set; } = new();

        // -- Vueltas --

        #endregion

    }
}
