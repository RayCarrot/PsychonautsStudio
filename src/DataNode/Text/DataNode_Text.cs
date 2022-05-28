using Microsoft.Extensions.DependencyInjection;

namespace PsychonautsTools;

public class DataNode_Text : DataNode
{
    public DataNode_Text(string displayName, string typeDisplayName, string text)
    {
        DisplayName = displayName;
        TypeDisplayName = typeDisplayName;
        Text = text;
        EditorViewModel = new TextEditorViewModel(ServiceProvider.GetRequiredService<AppUIManager>(), Text)
        {
            FileName = displayName
        };
    }
    
    public string Text { get; }

    public override string TypeDisplayName { get; }
    public override string DisplayName { get; }
    public override TextEditorViewModel EditorViewModel { get; }
}