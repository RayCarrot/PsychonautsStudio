using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using PsychoPortal;

namespace PsychonautsTools;

public class DataNode_GameTextureViewModel : BaseViewModel
{
    public DataNode_GameTextureViewModel(DataNode_GameTexture node)
    {
        Node = node;

        Frames = new ObservableCollection<TextureFrameViewModel>();

        foreach (TextureFrame frame in Node.GameTexture.Frames)
            Frames.Add(new TextureFrameViewModel(frame, new Lazy<ImageSource?>(() => frame.ToImageSource()), (int)frame.Width, (int)frame.Height));
    }

    public DataNode_GameTexture Node { get; }
    public ObservableCollection<TextureFrameViewModel> Frames { get; }
    public TextureFrameViewModel? SelectedFrame { get; set; }

    public class TextureFrameViewModel : BaseViewModel
    {
        public TextureFrameViewModel(TextureFrame frame, Lazy<ImageSource?> imageSource, int width, int height)
        {
            Frame = frame;
            ImageSource = imageSource;
            Width = width;
            Height = height;
        }

        public TextureFrame Frame { get; }
        public Lazy<ImageSource?> ImageSource { get; }
        public int Width { get; }
        public int Height { get; }

        // TODO: Export options etc.
    }
}