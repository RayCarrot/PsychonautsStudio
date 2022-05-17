using System;
using System.Globalization;
using System.Windows;
using MahApps.Metro.IconPacks;

namespace PsychonautsTools;

public class GenericIconToIconKindConverter : BaseValueConverter<GenericIconToIconKindConverter, GenericIconKind, PackIconMaterialKind>
{
    public override PackIconMaterialKind ConvertValue(GenericIconKind value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((GenericIcon)Application.Current.FindResource($"GenericIcons.{value}")).IconKind;
    }
}