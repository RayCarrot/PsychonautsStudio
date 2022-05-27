using System;
using System.IO;

namespace PsychonautsTools;

public class TextEditorViewModel : BaseViewModel
{
    public TextEditorViewModel(AppUIManager appUI, string text)
    {
        AppUI = appUI;
        Text = text;
    }

    public AppUIManager AppUI { get; }

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