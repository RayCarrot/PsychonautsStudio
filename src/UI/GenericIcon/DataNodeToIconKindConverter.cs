using System;
using System.Globalization;
using System.Windows;
using MahApps.Metro.IconPacks;

namespace PsychonautsTools;

public class DataNodeToIconKindConverter : BaseValueConverter<DataNodeToIconKindConverter, DataNode, PackIconMaterialKind>
{
    public override PackIconMaterialKind ConvertValue(DataNode value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((GenericIcon)Application.Current.FindResource($"DataNodeIcon.{value.GetType().Name}")).IconKind;
    }
}