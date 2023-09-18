using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KnitterNotebook.Helpers.Extensions;

public static class ValidationFailureExtension
{
    public static string GetMessagesAsString(this IEnumerable<ValidationFailure> validationFailures) => string.Join(Environment.NewLine, validationFailures.Select(x => x.ErrorMessage));
}