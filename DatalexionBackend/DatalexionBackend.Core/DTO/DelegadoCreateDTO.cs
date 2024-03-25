using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Core.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DatalexionBackend.Core.DTO
{
    public class DelegadoCreateDTO
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
        [StringLength(maximumLength: 8, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        public string CI { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")] // n..0 (0=no existe este sin el padre)
        [StringLength(maximumLength: 100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        #endregion

        #region External

        /// <summary>
        /// N-N Usar TypeBinder.cs, s: https://www.udemy.com/course/desarrollando-aplicaciones-en-react-y-aspnet-core/learn/lecture/26047194#overview
        /// </summary>
        // public List<CircuitDelegado> ListCircuitDelegados { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<List<CircuitDelegadoCreateDTO>>))]
        public List<CircuitDelegadoCreateDTO> ListCircuitDelegados { get; set; }

        /// <summary>
        /// 1-N
        /// </summary>
        public List<MunicipalityDTO> ListMunicipalities { get; set; }

        // -- Vueltas --

        [Required(ErrorMessage = "El campo {0} es requerido")] // n..0 (0=no existe este sin el padre)
        public int ClientId { get; set; }
        public Client Client { get; set; }


        #endregion

    }
}
