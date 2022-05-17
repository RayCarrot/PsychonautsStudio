using System.IO;
using System.Linq;

namespace PsychonautsTools;

public static class FileTypes
{
    private static readonly IFileType[] _fileTypes =
    {
        new FileType_PLB(),
        new FileType_PPF(),
        new FileType_PKG(),
    };

    public static IFileType[] GetFileTypes() => _fileTypes;

    public static IFileType? FindFileType(string filePath)
    {
        string ext = Path.GetExtension(filePath).ToLowerInvariant();
        return _fileTypes.FirstOrDefault(type => type.FileExtensions.Any(x => x == ext));
    }

    public static IFileType? GetFromID(string id) => _fileTypes.FirstOrDefault(x => x.ID == id);
}