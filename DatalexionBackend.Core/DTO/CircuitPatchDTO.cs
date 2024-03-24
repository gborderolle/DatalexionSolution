using Microsoft.AspNetCore.Http;

namespace DatalexionBackend.Core.DTO
{
    public class CircuitPatchDTO
    {

        #region Internal

        public int BlankVotes { get; set; }
        public int NullVotes { get; set; }
        public int ObservedVotes { get; set; }
        public int RecurredVotes { get; set; }
        public bool Step3completed { get; set; } = false;

        #endregion

        #region External

        public List<IFormFile> Photos { get; set; } = new List<IFormFile>();

        #endregion

    }
}
