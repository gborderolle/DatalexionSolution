using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.DTO
{
    public class MunicipalityDTO
    {
        #region Internal

        public int Id { get; set; }

        public DateTime Creation { get; set; } = DateTime.Now;

        public DateTime Update { get; set; } = DateTime.Now;

        public string Comments { get; set; }

        // Uniques

        public string Name { get; set; }

        #endregion

        #region External

        /// <summary>
        /// 0-N
        /// </summary>
        public List<Circuit> ListCircuits { get; set; }

        // -- Vueltas --

        public int ProvinceId { get; set; }
        public Province Province { get; set; }

        public int? DelegadoId { get; set; }
        public Delegado? Delegado { get; set; }

        #endregion

    }
}
