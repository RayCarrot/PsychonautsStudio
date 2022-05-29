using System.Collections.Generic;
using System.Linq;

namespace PsychonautsStudio;

public static class EnumerableExtensions
{
    public static bool AnyAndNotNull<T>(this IEnumerable<T>? col) => col?.Any() == true;
}