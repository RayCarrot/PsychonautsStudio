using System.Windows;
using System.Windows.Controls;

namespace PsychonautsStudio;

public class EditorTemplateSelector : DataTemplateSelector
{
    public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
    {
        if (item == null)
            return null;

        return App.Current.TryFindResource($"DataTemplate.{item.GetType().Name}") as DataTemplate;
    }
}