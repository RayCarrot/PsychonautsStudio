using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using PsychoPortal;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro.IconPacks;

namespace PsychonautsTools;

public class DataNode_TextureFrame : BinaryDataNode<TextureFrame>
{
    public DataNode_TextureFrame(TextureFrame frame, string displayName) : base(frame)
    {
        DisplayName = displayName;

        BitmapSource? img = null;

        try
        {
            img = frame.ToImageSource();
        }
        catch (Exception ex)
        {
            ServiceProvider.GetRequiredService<AppUIManager>().ShowErrorMessage($"An error occurred when reading the image {displayName}", ex);
        }

        ViewModel = new ImageEditorViewModel(ServiceProvider.GetRequiredService<AppUIManager>(), img);
    }

    private ImageEditorViewModel ViewModel { get; }
    public override object EditorViewModel => ViewModel;

    public override string TypeDisplayName => "Texture";
    public override string DisplayName { get; }
    public override ImageSource? IconImageSource => ViewModel.ImageSource;

    public override IEnumerable<UIItem> GetUIActions() => base.GetUIActions().Concat(new UIItem[]
    {
        new UIAction("Export", PackIconMaterialKind.Export,
            ViewModel.IsValid ? () => ViewModel.Export(DisplayName) : null),
        new UIAction("Copy to clipboard", PackIconMaterialKind.ContentCopy,
            ViewModel.IsValid ? () => Clipboard.SetImage(ViewModel.ImageSource) : null),
    });
}