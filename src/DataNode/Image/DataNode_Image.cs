using System.IO;
using System.Windows.Media;
using Microsoft.Extensions.DependencyInjection;

namespace PsychonautsTools;

public class DataNode_Image : DataNode
{
    public DataNode_Image(Stream imgStream, string displayName)
    {
        DisplayName = displayName;
        ViewModel = new DataNode_ImageViewModel(imgStream);
    }

    public DataNode_Image(byte[] imgData, string displayName)
    {
        DisplayName = displayName;
        ViewModel = new DataNode_ImageViewModel(imgData);
    }

    private DataNode_ImageViewModel ViewModel { get; }

    public override string TypeDisplayName => "Image";
    public override string DisplayName { get; }
    public override ImageSource IconImageSource => ViewModel.ImageSource;

    public override object GetUI()
    {
        // Perhaps have a better way of getting the singleton service without a static reference?
        DataNodeUI_Image ui = App.Current.ServiceProvider.GetRequiredService<DataNodeUI_Image>();
        ui.ViewModel = ViewModel;
        return ui;
    }

    public override void Dispose()
    {
        base.Dispose();
        ViewModel.Dispose();
    }
}