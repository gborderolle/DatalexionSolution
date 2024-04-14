using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.DTO
{
    public class WingDTO
    {
        #region Internal

        public int Id { get; set; }

        public DateTime Creation { get; set; } = DateTime.Now;

        public DateTime Update { get; set; } = DateTime.Now;

        public string? Comments { get; set; }

        // Uniques

        public required string Name { get; set; }

        #endregion

        #region External

        /// <summary>
        /// 0-N
        /// </summary>
        public List<Slate> ListSlates { get; set; } = new();

        public string PhotoURL { get; set; }

        // -- Vueltas --

        public int PartyId { get; set; }
        public Party Party { get; set; }

        #endregion

    }
}
