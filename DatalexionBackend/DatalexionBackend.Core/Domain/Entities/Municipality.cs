﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatalexionBackend.Core.Domain.Entities;

public class Municipality : IId
{
    #region Internal

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public DateTime Creation { get; set; } = DateTime.Now;

    public DateTime Update { get; set; } = DateTime.Now;

    public string? Comments { get; set; }

    // Uniques

    [Required(ErrorMessage = "El campo {0} es requerido")] // n..0 (0=no existe este sin el padre)
    [StringLength(maximumLength: 100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
    public required string Name { get; set; }

    #endregion

    #region External

    /// <summary>
    /// 0-N
    /// </summary>
    public List<Circuit> ListCircuits { get; set; } = new();

    // -- Vueltas --

    [Required(ErrorMessage = "El campo {0} es requerido")] // n..0 (0=no existe este sin el padre)
    public int ProvinceId { get; set; }
    public Province Province { get; set; }

    // public int? DelegadoId { get; set; }
    // public Delegado? Delegado { get; set; }

    #endregion

}