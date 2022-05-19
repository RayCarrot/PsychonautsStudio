using System.IO;
using System.Linq;

namespace PsychonautsTools;

public static class FileTypes
{
    private static readonly IFileType[] _fileTypes =
    {
        new FileType_PLB(), // Scene
        new FileType_JAN(), // Skeleton Animation
        new FileType_PBA(), // Blend Animation
        new FileType_EVE(), // Event Animation
        new FileType_PPF(), // Pack Pack
        new FileType_LPF(), // Script Pack
        new FileType_APF(), // Animation Pack
        new FileType_PKG(), // Package
    };

    public static IFileType[] GetFileTypes() => _fileTypes;

    public static IFileType? FindFileType(string filePath)
    {
        string ext = Path.GetExtension(filePath).ToLowerInvariant();
        return _fileTypes.FirstOrDefault(type => type.FileExtensions.Any(x => x == ext));
    }

    public static IFileType? GetFromID(string id) => _fileTypes.FirstOrDefault(x => x.ID == id);
}