using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.Domain.Entities;

public class Log : IId
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Entity { get; set; }
    public string Action { get; set; }
    public string Data { get; set; }
    public string Username { get; set; }
    public DateTime Creation { get; set; } = DateTime.Now;
    public int? ClientId { get; set; }
}