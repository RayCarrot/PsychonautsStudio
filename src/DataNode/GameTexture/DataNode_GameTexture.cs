using PsychoPortal;
using System.Linq;
using System.Windows.Media;
using Microsoft.Extensions.DependencyInjection;

namespace PsychonautsTools;

public class DataNode_GameTexture : DataNode
{
    public DataNode_GameTexture(GameTexture gameTexture, string displayName)
    {
        GameTexture = gameTexture;
        DisplayName = displayName;
        ViewModel = new DataNode_GameTextureViewModel(this);
    }

    private DataNode_GameTextureViewModel ViewModel { get; }

    public GameTexture GameTexture { get; }

    public override string TypeDisplayName => "Texture";
    public override string DisplayName { get; }
    public override GenericIconKind IconKind => GenericIconKind.DataNode_GameTexture;
    public override ImageSource? IconImageSource => ViewModel.Frames.FirstOrDefault()?.ImageSource.Value;
    public override IBinarySerializable SerializableObject => GameTexture;

    public override object GetUI()
    {
        // Perhaps have a better way of getting the singleton service without a static reference?
        DataNodeUI_GameTexture ui = App.Current.ServiceProvider.GetRequiredService<DataNodeUI_GameTexture>();
        ui.ViewModel = ViewModel;
        return ui;
    }
}