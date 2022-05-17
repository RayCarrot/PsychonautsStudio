using System.Collections.Generic;
using System.Linq;

namespace PsychonautsTools;

public static class EnumerableExtensions
{
    public static bool AnyAndNotNull<T>(this IEnumerable<T>? col) => col?.Any() == true;
}