using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Core.Helpers;
using DatalexionBackend.CoreBackend.Core.Domain.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatalexionBackend.Core.DTO
{
    public class SlateCreateDTO
    {
        #region Internal

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Creation { get; set; } = DateTime.Now;

        public DateTime Update { get; set; } = DateTime.Now;

        public string Comments { get; set; }

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
        /// N-N Usar TypeBinder.cs, s: https://www.udemy.com/course/desarrollando-aplicaciones-en-react-y-aspnet-core/learn/lecture/26047194#overview
        /// </summary>
        // public List<CircuitSlate> ListCircuitSlates { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<List<CircuitSlateCreateDTO>>))]
        public List<CircuitSlateCreateDTO> ListCircuitSlates { get; set; }

        public int? CandidateId { get; set; }
        public Candidate Candidate { get; set; }

        [FileSizeValidation(maxSizeMB: 4)]
        [FileTypeValidation(fileTypeGroup: FileTypeGroup.Image)]
        public IFormFile? Photo { get; set; } // Clase: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/19983788#notes

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
