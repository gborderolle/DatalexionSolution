using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.DTO
{
    public class ClientDTO
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

        public Party Party { get; set; } = new();

        /// <summary>
        /// 0-N
        /// </summary>
        public List<DatalexionUser> ListUsers { get; set; } = new();

        /// <summary>
        /// 0-N
        /// </summary>
        public List<Delegado> ListDelegados { get; set; } = new();

        #endregion

    }
}
