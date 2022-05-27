using System;
using System.IO;

namespace PsychonautsTools;

public class DataNode_TextViewModel : BaseViewModel
{
    public DataNode_TextViewModel(AppUIManager appUI, DataNode_Text node)
    {
        AppUI = appUI;
        Node = node;
        Text = node.Text;
    }

    public AppUIManager AppUI { get; }

    public DataNode_Text Node { get; }
    public string Text { get; set; }

    public void Export(string? defaultName = null)
    {
        string? nativeExt = defaultName != null ? Path.GetExtension(defaultName) : null;
        string[] ext = nativeExt == null ? new[] { "TXT" } : new[] { nativeExt[1..], "TXT" };

        string? dest = AppUI.SaveFile("Export Text", defaultName, ext, false);

        if (dest == null)
            return;

        try
        {
            File.WriteAllText(dest, Text);
        }
        catch (Exception ex)
        {
            AppUI.ShowErrorMessage("An error occurred when exporting the text", ex);
        }
    }
}