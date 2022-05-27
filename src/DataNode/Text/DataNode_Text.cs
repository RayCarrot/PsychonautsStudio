using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MahApps.Metro.IconPacks;
using Microsoft.Extensions.DependencyInjection;

namespace PsychonautsTools;

public class DataNode_Text : DataNode
{
    public DataNode_Text(string displayName, string typeDisplayName, string text)
    {
        DisplayName = displayName;
        TypeDisplayName = typeDisplayName;
        Text = text;
        ViewModel = new DataNode_TextViewModel(ServiceProvider.GetRequiredService<AppUIManager>(), this);
    }
    
    private DataNode_TextViewModel ViewModel { get; }

    public string Text { get; }

    public override string TypeDisplayName { get; }
    public override string DisplayName { get; }

    public override IEnumerable<UIItem> GetUIActions() => base.GetUIActions().Concat(new UIItem[]
    {
        new UIAction("Export", PackIconMaterialKind.Export, () => ViewModel.Export(DisplayName)),
        new UIAction("Copy to clipboard", PackIconMaterialKind.ContentCopy, () => Clipboard.SetText(Text)),
    });

    public override object GetUI()
    {
        DataNodeUI_Text ui = ServiceProvider.GetRequiredService<DataNodeUI_Text>();
        ui.ViewModel = ViewModel;
        return ui;
    }
}