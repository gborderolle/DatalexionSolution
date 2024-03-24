using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.DTO
{
    public class ProvinceDTO
    {
        #region Internal

        public int Id { get; set; }

        public DateTime Creation { get; set; } = DateTime.Now;

        public DateTime Update { get; set; } = DateTime.Now;

        public string Comments { get; set; }

        // Uniques

        public string Name { get; set; }
        public string? Center { get; set; }
        public int? Zoom { get; set; }

        #endregion

        #region External

        /// <summary>
        /// 0-N
        /// </summary>
        public List<Slate> ListSlates { get; set; }

        /// <summary>
        /// 0-N
        /// </summary>
        public List<Municipality> ListMunicipalities { get; set; }

        #endregion

    }
}