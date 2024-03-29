﻿using ImageMagick;
using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PsychonautsStudio;

public class ImageEditorViewModel : EditorViewModel
{
    public ImageEditorViewModel(AppUIManager appUI, BitmapSource? imgSource)
    {
        AppUI = appUI;
        MagickImage = null;
        ImageSource = imgSource;
    }

    public ImageEditorViewModel(AppUIManager appUI, MagickImage img)
    {
        AppUI = appUI;
        MagickImage = img;
        ImageSource = MagickImage.ToBitmapSource();
    }

    public ImageEditorViewModel(AppUIManager appUI, Stream imgStream) : this(appUI, new MagickImage(imgStream)) { }

    public ImageEditorViewModel(AppUIManager appUI, byte[] imgData) : this(appUI, new MagickImage(imgData)) { }

    public AppUIManager AppUI { get; }

    [MemberNotNullWhen(true, nameof(ImageSource))]
    public bool IsValid => ImageSource != null;

    public MagickImage? MagickImage { get; }
    public BitmapSource? ImageSource { get; }
    public string? FileName { get; init; }

    public override IEnumerable<UIItem> GetUIActions() => base.GetUIActions().AppendGroup(new UIItem[]
    {
        new UIAction("Export", PackIconMaterialKind.Export,
            IsValid ? () => Export(FileName) : null),
        new UIAction("Copy to clipboard", PackIconMaterialKind.ContentCopy,
            IsValid ? () => Clipboard.SetImage(ImageSource) : null),
    });

    public void Export(string? defaultName = null)
    {
        if (!IsValid)
            throw new Exception("Can't export a null image");

        string[] ext = MagickImage == null ? new[] { "PNG" } : new[] { "PNG", "JPG", "DDS", "TGA" };

        string ? dest = AppUI.SaveFile("Export Image", Path.ChangeExtension(defaultName, ".png"), ext, false);

        if (dest == null)
            return;

        try
        {
            if (MagickImage == null)
                ImageSource!.SaveAsPNG(dest);
            else
                MagickImage.Write(dest);
        }
        catch (Exception ex)
        {
            AppUI.ShowErrorMessage("An error occurred when exporting the image", ex);
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        MagickImage?.Dispose();
    }
}