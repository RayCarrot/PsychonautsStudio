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
        ViewModel = new ImageEditorViewModel(ServiceProvider.GetRequiredService<AppUIManager>(), imgStream);
    }

    public DataNode_Image(byte[] imgData, string displayName)
    {
        DisplayName = displayName;
        ViewModel = new ImageEditorViewModel(ServiceProvider.GetRequiredService<AppUIManager>(), imgData);
    }

    private ImageEditorViewModel ViewModel { get; }
    public override object EditorViewModel => ViewModel;

    public override string TypeDisplayName => "Image";
    public override string DisplayName { get; }
    public override ImageSource? IconImageSource => ViewModel.ImageSource;

    public override IEnumerable<UIItem> GetUIActions() => base.GetUIActions().Concat(new UIItem[]
    {
        //new UIAction("Extract", PackIconMaterialKind.ExportVariant, () => { }), // TODO: Implement?
        new UIAction("Export", PackIconMaterialKind.Export, 
            ViewModel.IsValid ? () => ViewModel.Export(DisplayName) : null),
        new UIAction("Copy to clipboard", PackIconMaterialKind.ContentCopy, 
            ViewModel.IsValid ? () => Clipboard.SetImage(ViewModel.ImageSource) : null),
    });
}