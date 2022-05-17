using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace PsychonautsTools;

public class GenericIconToBrushConverter : BaseValueConverter<GenericIconToBrushConverter, GenericIconKind, Brush>
{
    public override Brush ConvertValue(GenericIconKind value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((GenericIcon)Application.Current.FindResource($"GenericIcons.{value}")).IconColor;
    }
}