using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Utilities;

public class GuidCollection : IParsable<GuidCollection>
{
    public IEnumerable<Guid> Values { get; init; } = Enumerable.Empty<Guid>();
    public static GuidCollection Parse(string value, IFormatProvider? provider)
    {
        if (!TryParse(value, provider, out var result))
        {
            throw new ArgumentException("Could not parse this value", nameof(value));
        }
        return result;
    }

    public static bool TryParse([NotNullWhen(true)] string? str, IFormatProvider? provider, [MaybeNullWhen(false)] out GuidCollection result)
    {
        var segments = str?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (segments is null || !segments.Any())
        {
            result = null;
            return false;
        }
        var values = new List<Guid>();
        foreach (var segment in segments)
        {
            if (Guid.TryParse(segment, out var value))
            {
                values.Add(value);
            }
        }

        result = new GuidCollection
        {
            Values = values
        };

        return true;
    }
}
