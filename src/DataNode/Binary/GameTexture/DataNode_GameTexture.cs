using PsychoPortal;
using System.Linq;
using System.Windows.Media;
using Microsoft.Extensions.DependencyInjection;

namespace PsychonautsTools;

public class DataNode_GameTexture : BinaryDataNode<GameTexture>
{
    public DataNode_GameTexture(GameTexture gameTexture, string displayName) : base(gameTexture)
    {
        DisplayName = displayName;
        ViewModel = new DataNode_GameTextureViewModel(this);
    }

    private DataNode_GameTextureViewModel ViewModel { get; }

    public override string TypeDisplayName => "Texture";
    public override string DisplayName { get; }
    public override ImageSource? IconImageSource => ViewModel.Frames.FirstOrDefault()?.ImageSource.Value;

    public override object GetUI()
    {
        // Perhaps have a better way of getting the singleton service without a static reference?
        DataNodeUI_GameTexture ui = App.Current.ServiceProvider.GetRequiredService<DataNodeUI_GameTexture>();
        ui.ViewModel = ViewModel;
        return ui;
    }
}