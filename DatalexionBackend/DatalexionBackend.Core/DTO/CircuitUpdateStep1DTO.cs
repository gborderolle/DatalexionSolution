using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using DatalexionBackend.CoreBackend.Core.Domain.Validations;
using DatalexionBackend.Core.Helpers;
using Microsoft.AspNetCore.Http;
using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.DTO
{
    public class CircuitUpdateStep1DTO
    {
        #region Specials

        public int ClientId { get; set; }

        #endregion 

        public int Id { get; set; }

        public DateTime Creation { get; set; } = DateTime.Now;

        public DateTime Update { get; set; } = DateTime.Now;

        // Uniques

        public int Number { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")] // n..0 (0=no existe este sin el padre)
        [StringLength(maximumLength: 100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")] // n..0 (0=no existe este sin el padre)
        public int? LastUpdateDelegadoId { get; set; }

        /// <summary>
        /// N-N Usar TypeBinder.cs, s: https://www.udemy.com/course/desarrollando-aplicaciones-en-react-y-aspnet-core/learn/lecture/26047194#overview
        /// </summary>
        [ModelBinder(BinderType = typeof(TypeBinder<List<CircuitSlateCreateDTO>>))]
        public List<CircuitSlateCreateDTO> ListCircuitSlates { get; set; }


    }
}
