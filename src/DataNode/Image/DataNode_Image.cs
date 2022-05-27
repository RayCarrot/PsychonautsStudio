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
}