using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;

namespace PsychonautsTools;

public class DataNode_Image : DataNode
{
    public DataNode_Image(Stream imgStream, string displayName)
    {
        DisplayName = displayName;
        EditorViewModel = new ImageEditorViewModel(ServiceProvider.GetRequiredService<AppUIManager>(), imgStream)
        {
            FileName = displayName
        };
    }

    public DataNode_Image(byte[] imgData, string displayName)
    {
        DisplayName = displayName;
        EditorViewModel = new ImageEditorViewModel(ServiceProvider.GetRequiredService<AppUIManager>(), imgData)
        {
            FileName = displayName
        };
    }

    public override string TypeDisplayName => "Image";
    public override string DisplayName { get; }
    public override ImageSource? IconImageSource => EditorViewModel.ImageSource;
    public override ImageEditorViewModel EditorViewModel { get; }

    public override IEnumerable<InfoItem> GetInfoItems()
    {
        foreach (InfoItem item in base.GetInfoItems())
            yield return item;

        if (EditorViewModel.IsValid)
        {
            yield return new InfoItem("Width", $"{EditorViewModel.ImageSource.Width}");
            yield return new InfoItem("Height", $"{EditorViewModel.ImageSource.Height}");
        }
    }
}