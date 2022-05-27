using MahApps.Metro.IconPacks;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace PsychonautsTools;

public class DataNode_Image : DataNode
{
    public DataNode_Image(Stream imgStream, string displayName)
    {
        DisplayName = displayName;
        ViewModel = new DataNode_ImageViewModel(ServiceProvider.GetRequiredService<AppUIManager>(), imgStream);
    }

    public DataNode_Image(byte[] imgData, string displayName)
    {
        DisplayName = displayName;
        ViewModel = new DataNode_ImageViewModel(ServiceProvider.GetRequiredService<AppUIManager>(), imgData);
    }

    private DataNode_ImageViewModel ViewModel { get; }

    public override string TypeDisplayName => "Image";
    public override string DisplayName { get; }
    public override ImageSource IconImageSource => ViewModel.ImageSource;

    public override IEnumerable<UIItem> GetUIActions() => base.GetUIActions().Concat(new UIItem[]
    {
        //new UIAction("Extract", PackIconMaterialKind.ExportVariant, () => { }), // TODO: Implement?
        new UIAction("Export", PackIconMaterialKind.Export, () => ViewModel.Export(DisplayName)),
        new UIAction("Copy to clipboard", PackIconMaterialKind.ContentCopy, () => Clipboard.SetImage(ViewModel.ImageSource)),
    });

    public override object GetUI()
    {
        DataNodeUI_Image ui = ServiceProvider.GetRequiredService<DataNodeUI_Image>();
        ui.ViewModel = ViewModel;
        return ui;
    }

    public override void Dispose()
    {
        base.Dispose();
        ViewModel.Dispose();
    }
}