using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KnitterNotebook.Helpers.Extensions;

public static class ValidationFailureExtension
{
    /// <summary>
    /// Returns all errors' messages from <paramref name="validationFailures"/> as string.
    /// </summary>
    /// <param name="validationFailures">Errors of validation</param>
    /// <returns>Errors' messages as string</returns>
    public static string GetMessagesAsString(this IEnumerable<ValidationFailure> validationFailures) 
        => string.Join(Environment.NewLine, validationFailures.Select(x => x.ErrorMessage));
}