using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows.Media;

namespace PsychonautsTools;

public class DataNode_Image : DataNode
{
    public DataNode_Image(Stream imgStream, string displayName)
    {
        DisplayName = displayName;
        ViewModel = new ImageEditorViewModel(ServiceProvider.GetRequiredService<AppUIManager>(), imgStream)
        {
            FileName = displayName
        };
    }

    public DataNode_Image(byte[] imgData, string displayName)
    {
        DisplayName = displayName;
        ViewModel = new ImageEditorViewModel(ServiceProvider.GetRequiredService<AppUIManager>(), imgData)
        {
            FileName = displayName
        };
    }

    private ImageEditorViewModel ViewModel { get; }
    public override EditorViewModel EditorViewModel => ViewModel;

    public override string TypeDisplayName => "Image";
    public override string DisplayName { get; }
    public override ImageSource? IconImageSource => ViewModel.ImageSource;

    public override IEnumerable<InfoItem> GetInfoItems()
    {
        foreach (InfoItem item in base.GetInfoItems())
            yield return item;

        if (ViewModel.IsValid)
        {
            yield return new InfoItem("Width", $"{ViewModel.ImageSource.Width}");
            yield return new InfoItem("Height", $"{ViewModel.ImageSource.Height}");
        }
    }
}