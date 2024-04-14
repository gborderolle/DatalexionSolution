using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.DTO
{
    public class DelegadoDTO
    {
        #region Internal

        public int Id { get; set; }

        public DateTime Creation { get; set; } = DateTime.Now;

        public DateTime Update { get; set; } = DateTime.Now;

        public string? Comments { get; set; }

        // Uniques

        public string CI { get; set; }
        public required string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        #endregion

        #region External

        /// <summary>
        /// N-N
        /// </summary>
        public List<CircuitDelegado> ListCircuitDelegados { get; set; }

        /// <summary>
        /// 1-N
        /// </summary>
        public List<Municipality> ListMunicipalities { get; set; }

        // -- Vueltas --

        public int ClientId { get; set; }
        public Client Client { get; set; }

        #endregion

    }
}
