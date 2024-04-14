using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using DatalexionBackend.CoreBackend.Core.Domain.Validations;
using DatalexionBackend.Core.Helpers;
using Microsoft.AspNetCore.Http;
using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.DTO
{
    public class CircuitCreateDTO
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
        public required string Address { get; set; }
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
        /// N-N Usar TypeBinder.cs, s: https://www.udemy.com/course/desarrollando-aplicaciones-en-react-y-aspnet-core/learn/lecture/26047194#overview
        /// </summary>
        // public List<CircuitDelegado> ListCircuitDelegados { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<List<CircuitDelegadoCreateDTO>>))]
        public List<CircuitDelegadoCreateDTO> ListCircuitDelegados { get; set; }

        /// <summary>
        /// N-N Usar TypeBinder.cs, s: https://www.udemy.com/course/desarrollando-aplicaciones-en-react-y-aspnet-core/learn/lecture/26047194#overview
        /// </summary>
        [ModelBinder(BinderType = typeof(TypeBinder<List<CircuitSlateCreateDTO>>))]
        public List<CircuitSlateCreateDTO> ListCircuitSlates { get; set; }

        /// <summary>
        /// N-N Usar TypeBinder.cs, s: https://www.udemy.com/course/desarrollando-aplicaciones-en-react-y-aspnet-core/learn/lecture/26047194#overview
        /// </summary>
        public List<CircuitPartyCreateDTO> ListCircuitParties { get; set; }

        [FileSizeValidation(maxSizeMB: 5)]
        [FileTypeValidation(fileTypeGroup: FileTypeGroup.Image)]
        public List<IFormFile>? ListPhotos { get; set; } // Clase: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/19983788#notes

        // -- Vueltas --

        [Required(ErrorMessage = "El campo {0} es requerido")] // n..0 (0=no existe este sin el padre)
        public int MunicipalityId { get; set; }
        public Municipality Municipality { get; set; }

        #endregion

    }
}
