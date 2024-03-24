// s: https://www.udemy.com/course/desarrollando-aplicaciones-en-react-y-aspnet-core/learn/lecture/26047194#overview
namespace DatalexionBackend.Core.DTO
{
    public class CircuitSlateDTO
    {
        #region Internal

        public int CircuitId { get; set; }

        public int SlateId { get; set; }

        // Uniques
        public int? Votes { get; set; }

        #endregion

    }
}