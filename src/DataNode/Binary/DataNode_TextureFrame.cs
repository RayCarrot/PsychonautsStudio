﻿using Microsoft.Extensions.DependencyInjection;
using PsychoPortal;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PsychonautsStudio;

public class DataNode_TextureFrame : BinaryDataNode<TextureFrame>
{
    public DataNode_TextureFrame(TextureFrame frame, string displayName) : base(frame)
    {
        DisplayName = displayName;

        BitmapSource? img = null;

        try
        {
            img = frame.ToImageSource();
        }
        catch (Exception ex)
        {
            ServiceProvider.GetRequiredService<AppUIManager>().ShowErrorMessage($"An error occurred when reading the image {displayName}", ex);
        }

        EditorViewModel = new ImageEditorViewModel(ServiceProvider.GetRequiredService<AppUIManager>(), img)
        {
            FileName = displayName
        };
    }

    public override ImageEditorViewModel EditorViewModel { get; }

    public override string TypeDisplayName => "Texture";
    public override string DisplayName { get; }
    public override ImageSource? IconImageSource => EditorViewModel.ImageSource;

    public override IEnumerable<InfoItem> GetInfoItems()
    {
        foreach (InfoItem item in base.GetInfoItems())
            yield return item;

        yield return new InfoItem("Width", $"{SerializableObject.Width}");
        yield return new InfoItem("Height", $"{SerializableObject.Height}");
        yield return new InfoItem("Type", $"{SerializableObject.Type}");
        yield return new InfoItem("Format", $"{SerializableObject.Format}");
        yield return new InfoItem("MipMaps Levels", $"{SerializableObject.MipMapLevels}");
    }
}