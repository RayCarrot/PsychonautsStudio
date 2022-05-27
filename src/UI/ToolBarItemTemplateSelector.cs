using System;
using System.Windows;
using System.Windows.Controls;

namespace PsychonautsTools;

public class ToolBarItemTemplateSelector : DataTemplateSelector
{
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        return item switch
        {
            UISeparator => (DataTemplate)App.Current.FindResource("DataTemplate.ToolBar.Separator"),
            UIAction => (DataTemplate)App.Current.FindResource("DataTemplate.ToolBar.Button"),
            _ => throw new Exception($"Invalid UI item type {item.GetType()} for a tool bar item")
        };
    }
}