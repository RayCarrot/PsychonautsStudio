using Microsoft.Extensions.DependencyInjection;

namespace PsychonautsTools;

public class DataNode_Text : DataNode
{
    public DataNode_Text(string displayName, string typeDisplayName, string text)
    {
        DisplayName = displayName;
        TypeDisplayName = typeDisplayName;
        Text = text;
        ViewModel = new DataNode_TextViewModel(this);
    }
    
    private DataNode_TextViewModel ViewModel { get; }

    public string Text { get; }

    public override string TypeDisplayName { get; }
    public override string DisplayName { get; }

    public override object GetUI()
    {
        // Perhaps have a better way of getting the singleton service without a static reference?
        DataNodeUI_Text ui = App.Current.ServiceProvider.GetRequiredService<DataNodeUI_Text>();
        ui.ViewModel = ViewModel;
        return ui;
    }
}