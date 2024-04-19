using DatalexionBackend.CoreBackend.Core.Domain.Validations;
using Microsoft.AspNetCore.Http;

namespace DatalexionBackend.Core.DTO
{
    public class CircuitPartyCreateDTO
    {
        #region Internal

        public int CircuitId { get; set; }
        public int PartyId { get; set; }

        // Uniques
        public int? TotalPartyVotes { get; set; }

        public int BlankVotes { get; set; }
        public int NullVotes { get; set; }
        public int ObservedVotes { get; set; }
        public int RecurredVotes { get; set; }
        public bool Step1completed { get; set; } = false;
        public bool Step2completed { get; set; } = false;
        public bool Step3completed { get; set; } = false;
        public int? LastUpdateDelegadoId { get; set; }

        [FileSizeValidation(maxSizeMB: 5)]
        [FileTypeValidation(fileTypeGroup: FileTypeGroup.Image)]
        public List<IFormFile>? ListPhotos { get; set; } // Clase: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/19983788#notes

        #endregion

    }
}