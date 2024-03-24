using System.ComponentModel.DataAnnotations;

namespace DatalexionBackend.Core.Helpers;

public class ValidationHelper
{
    internal static void ModelValidation(object obj)
    {
        // Model validations
        ValidationContext validationContext = new ValidationContext(obj);
        List<ValidationResult> validationResults = new List<ValidationResult>();
        if (!Validator.TryValidateObject(obj, validationContext, validationResults, true))
        {
            throw new ArgumentException(string.Join(", ", validationResults.FirstOrDefault()?.ErrorMessage));
        }

    }

}
