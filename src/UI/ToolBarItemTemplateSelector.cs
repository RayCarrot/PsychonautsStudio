using System;
using System.Windows;
using System.Windows.Controls;

namespace PsychonautsStudio;

public class ToolBarItemTemplateSelector : DataTemplateSelector
{
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        return item switch
        {
            UISeparator => (DataTemplate)App.Current.FindResource("DataTemplate.UISeparator.ToolBar"),
            UIAction => (DataTemplate)App.Current.FindResource("DataTemplate.UIAction.ToolBar"),
            _ => throw new Exception($"Invalid UI item type {item.GetType()} for a tool bar item")
        };
    }
}