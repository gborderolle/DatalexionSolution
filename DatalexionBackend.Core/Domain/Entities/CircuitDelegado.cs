// s: https://www.udemy.com/course/desarrollando-aplicaciones-en-react-y-aspnet-core/learn/lecture/26047194#overview

namespace DatalexionBackend.Core.Domain.Entities
{
    public class CircuitDelegado
    {
        #region Internal

        public int CircuitId { get; set; }
        public Circuit Circuit { get; set; }

        public int DelegadoId { get; set; }
        public Delegado Delegado { get; set; }

        #endregion

    }
}