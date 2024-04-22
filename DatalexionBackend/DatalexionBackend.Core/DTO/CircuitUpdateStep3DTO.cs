using System.ComponentModel.DataAnnotations;
using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.DTO
{
    public class CircuitUpdateStep3DTO
    {
        #region Specials

        public int ClientId { get; set; }

        #endregion 

        #region Generics

        public int Id { get; set; }

        public DateTime Creation { get; set; } = DateTime.Now;

        public DateTime Update { get; set; } = DateTime.Now;

        public int Number { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")] // n..0 (0=no existe este sin el padre)
        [StringLength(maximumLength: 100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")] // n..0 (0=no existe este sin el padre)
        public int? LastUpdateDelegadoId { get; set; }

        #endregion 

        #region Uniques

        public int BlankVotes { get; set; } = 0;
        public int NullVotes { get; set; } = 0;
        public int ObservedVotes { get; set; } = 0;
        public int RecurredVotes { get; set; } = 0;
        public int ImagesUploadedCount { get; set; } = 0;

        /// <summary>
        /// 1-N
        /// </summary>
        // public List<Photo> ListPhotos { get; set; }

        #endregion 

    }
}
