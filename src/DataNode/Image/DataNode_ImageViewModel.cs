using ImageMagick;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows.Media.Imaging;

namespace PsychonautsTools;

public class DataNode_ImageViewModel : BaseViewModel, IDisposable
{
    public DataNode_ImageViewModel(AppUIManager appUI, BitmapSource? imgSource)
    {
        AppUI = appUI;
        MagickImage = null;
        ImageSource = imgSource;
    }

    public DataNode_ImageViewModel(AppUIManager appUI, MagickImage img)
    {
        AppUI = appUI;
        MagickImage = img;
        ImageSource = MagickImage.ToBitmapSource();
    }

    public DataNode_ImageViewModel(AppUIManager appUI, Stream imgStream) : this(appUI, new MagickImage(imgStream)) { }

    public DataNode_ImageViewModel(AppUIManager appUI, byte[] imgData) : this(appUI, new MagickImage(imgData)) { }

    public AppUIManager AppUI { get; }

    [MemberNotNullWhen(true, nameof(ImageSource))]
    public bool IsValid => ImageSource != null;

    public MagickImage? MagickImage { get; }
    public BitmapSource? ImageSource { get; }

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

    public void Dispose()
    {
        MagickImage?.Dispose();
    }
}