using System;
using System.Windows.Input;
using MahApps.Metro.IconPacks;

namespace PsychonautsTools;

public class UIAction : UIItem
{
    public UIAction(string displayName, PackIconMaterialKind iconKind, Action? action, bool isEnabled = true)
    {
        DisplayName = displayName;
        IconKind = iconKind;
        Command = action == null ? new RelayCommand(() => { }, false) : new RelayCommand(action, isEnabled);
    }

    public string DisplayName { get; }
    public PackIconMaterialKind IconKind { get; }
    public ICommand Command { get; }
}