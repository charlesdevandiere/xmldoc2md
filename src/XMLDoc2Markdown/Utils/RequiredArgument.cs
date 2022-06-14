using System;
using System.Collections.Generic;
using System.Linq;

namespace XMLDoc2Markdown.Utils;

internal static class RequiredArgument
{
    /// <summary>Verifies argument is not null.</summary>
    /// <param name="param">The parameter.</param>
    /// <param name="paramName">The parameter name.</param>
    /// <typeparam name="TArgument">The parameter type.</typeparam>
    internal static void NotNull<TArgument>(TArgument param, string paramName)
    {
        if (param is null)
        {
            throw new ArgumentNullException(paramName);
        }
    }

    /// <summary>Verifies argument is not null or empty.</summary>
    /// <param name="param">The parameter.</param>
    /// <param name="paramName">The parameter name.</param>
    internal static void NotNullOrEmpty(string param, string paramName)
    {
        RequiredArgument.NotNull(param, paramName);

        if (param.Length == 0)
        {
            throw new ArgumentException("Value cannot be empty.", paramName);
        }
    }

    /// <summary>Verifies argument is not null or empty.</summary>
    /// <param name="param">The parameter.</param>
    /// <param name="paramName">The parameter name.</param>
    internal static void NotNullOrEmpty<T>(IEnumerable<T> param, string paramName)
    {
        RequiredArgument.NotNull(param, paramName);

        if (!param.Any())
        {
            throw new ArgumentException("Value cannot be empty.", paramName);
        }
    }
}
