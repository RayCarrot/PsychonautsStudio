using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using MahApps.Metro.IconPacks;

namespace PsychonautsTools;

public class TextEditorViewModel : EditorViewModel
{
    public TextEditorViewModel(AppUIManager appUI, string text)
    {
        AppUI = appUI;
        Text = text;
    }

    public AppUIManager AppUI { get; }
    public string? FileName { get; init; }

    public string Text { get; set; }

    public override IEnumerable<UIItem> GetUIActions() => base.GetUIActions().Concat(new UIItem[]
    {
        new UIAction("Export", PackIconMaterialKind.Export, () => Export(FileName)),
        new UIAction("Copy to clipboard", PackIconMaterialKind.ContentCopy, () => Clipboard.SetText(Text)),
    });

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