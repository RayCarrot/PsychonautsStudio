using System.Windows.Media;
using MahApps.Metro.IconPacks;

namespace PsychonautsStudio;

public struct GenericIcon
{
    public GenericIcon(PackIconMaterialKind iconKind, Color iconColor)
    {
        IconKind = iconKind;
        IconColor = iconColor;
    }

    public PackIconMaterialKind IconKind { get; set; }
    public Color IconColor { get; set; }
}