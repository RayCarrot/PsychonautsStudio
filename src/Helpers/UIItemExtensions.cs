using System.Collections.Generic;
using System.Linq;

namespace PsychonautsStudio;

public static class UIItemExtensions
{
    public static IEnumerable<UIItem> AppendGroup(this IEnumerable<UIItem> items, IEnumerable<UIItem> group)
    {
        UIItem[] itemsArray = items.ToArray();

        if (itemsArray.Length > 0)
            return itemsArray.Append(new UISeparator()).Concat(group);
        else
            return itemsArray.Concat(group);
    }
}