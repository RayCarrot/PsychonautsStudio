using ImageMagick;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace PsychonautsTools;

public class DataNode_ImageViewModel : BaseViewModel, IDisposable
{
    public DataNode_ImageViewModel(AppUIManager appUI, Stream imgStream)
    {
        AppUI = appUI;
        Image = new MagickImage(imgStream);
        ImageSource = Image.ToBitmapSource();
    }

    public DataNode_ImageViewModel(AppUIManager appUI, byte[] imgData)
    {
        AppUI = appUI;
        Image = new MagickImage(imgData);
        ImageSource = Image.ToBitmapSource();
    }

    public AppUIManager AppUI { get; }

    public MagickImage Image { get; }
    public BitmapSource ImageSource { get; }

    public void Export(string? defaultName = null)
    {
        string? dest = AppUI.SaveFile("Export Image", Path.ChangeExtension(defaultName, ".png"), new[]
        {
            "PNG", "JPG", "DDS", "TGA"
        }, false);

        if (dest == null)
            return;

        try
        {
            Image.Write(dest);
        }
        catch (Exception ex)
        {
            AppUI.ShowErrorMessage("An error occurred when exporting the image", ex);
        }
    }

    public void Dispose()
    {
        Image.Dispose();
    }
}