using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace PsychonautsTools;

public class DataNodeToIconBrushConverter : BaseValueConverter<DataNodeToIconBrushConverter, DataNode, Brush>
{
    public override Brush ConvertValue(DataNode value, Type targetType, object parameter, CultureInfo culture)
    {
        return new SolidColorBrush(((GenericIcon)Application.Current.FindResource($"DataNodeIcon.{value.GetType().Name}")).IconColor);
    }
}