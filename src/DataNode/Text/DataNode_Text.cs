using Microsoft.Extensions.DependencyInjection;

namespace PsychonautsTools;

public class DataNode_Text : DataNode
{
    public DataNode_Text(string displayName, string typeDisplayName, string text)
    {
        DisplayName = displayName;
        TypeDisplayName = typeDisplayName;
        Text = text;
        ViewModel = new TextEditorViewModel(ServiceProvider.GetRequiredService<AppUIManager>(), Text)
        {
            FileName = displayName
        };
    }
    
    private TextEditorViewModel ViewModel { get; }
    public override EditorViewModel EditorViewModel => ViewModel;

    public string Text { get; }

    public override string TypeDisplayName { get; }
    public override string DisplayName { get; }
}