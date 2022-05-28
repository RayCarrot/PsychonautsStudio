using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Extensions.DependencyInjection;
using PsychoPortal;

namespace PsychonautsTools;

public class DataNode_PS2_Texture : BinaryDataNode<PS2_Texture>
{
    public DataNode_PS2_Texture(PS2_Texture texture, string displayName) : base(texture)
    {
        DisplayName = displayName;

        BitmapSource? img = null;

        try
        {
            img = texture.ToImageSource();
        }
        catch (Exception ex)
        {
            ServiceProvider.GetRequiredService<AppUIManager>().ShowErrorMessage($"An error occurred when reading the image {displayName}", ex);
        }

        ViewModel = new ImageEditorViewModel(ServiceProvider.GetRequiredService<AppUIManager>(), img)
        {
            FileName = displayName
        };
    }

    private ImageEditorViewModel ViewModel { get; }
    public override EditorViewModel EditorViewModel => ViewModel;

    public override string TypeDisplayName => "PS2 Texture";
    public override string DisplayName { get; }
    public override ImageSource? IconImageSource => ViewModel.ImageSource;

    public override IEnumerable<InfoItem> GetInfoItems()
    {
        foreach (InfoItem item in base.GetInfoItems())
            yield return item;

        yield return new InfoItem("Width", $"{SerializableObject.Width}");
        yield return new InfoItem("Height", $"{SerializableObject.Height}");
        yield return new InfoItem("Format", $"{SerializableObject.Format}");
        yield return new InfoItem("Is Swizzled", $"{SerializableObject.IsSwizzled}");
    }
}