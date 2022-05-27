﻿using Microsoft.Extensions.DependencyInjection;
using PsychoPortal;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PsychonautsTools;

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

        ViewModel = new ImageEditorViewModel(ServiceProvider.GetRequiredService<AppUIManager>(), img)
        {
            FileName = displayName
        };
    }

    private ImageEditorViewModel ViewModel { get; }
    public override EditorViewModel EditorViewModel => ViewModel;

    public override string TypeDisplayName => "Texture";
    public override string DisplayName { get; }
    public override ImageSource? IconImageSource => ViewModel.ImageSource;
}