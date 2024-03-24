// s: https://www.udemy.com/course/desarrollando-aplicaciones-en-react-y-aspnet-core/learn/lecture/26047194#overview
namespace DatalexionBackend.Core.Domain.Entities
{
    public class CircuitSlate
    {
        #region Internal

        public int CircuitId { get; set; }
        public Circuit Circuit { get; set; }

        public int SlateId { get; set; }
        public Slate Slate { get; set; }

        // Uniques
        public int? Votes { get; set; }

        #endregion

    }
}