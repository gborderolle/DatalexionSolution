﻿using DatalexionBackend.CoreBackend.Core.Domain.Validations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatalexionBackend.Core.DTO;

public class CandidateCreateDTO
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

    [FileSizeValidation(maxSizeMB: 4)]
    [FileTypeValidation(fileTypeGroup: FileTypeGroup.Image)]
    public IFormFile? Photo { get; set; } // Clase: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/19983788#notes

    #region External

    #endregion

}