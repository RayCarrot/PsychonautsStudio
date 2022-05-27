using System;
using System.IO;
using ImageMagick;
using System.Windows.Media;

namespace PsychonautsTools;

public class DataNode_ImageViewModel : BaseViewModel, IDisposable
{
    public DataNode_ImageViewModel(Stream imgStream)
    {
        Image = new MagickImage(imgStream);
        ImageSource = Image.ToBitmapSource();
    }

    public DataNode_ImageViewModel(byte[] imgData)
    {
        Image = new MagickImage(imgData);
        ImageSource = Image.ToBitmapSource();
    }

    public MagickImage Image { get; }
    public ImageSource ImageSource { get; }

    public void Dispose()
    {
        Image.Dispose();
    }
}