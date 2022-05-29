using PsychoPortal;

namespace PsychonautsStudio;

public class AppUserData
{
    public LoadedFile[]? LoadedFiles { get; set; }

    public record LoadedFile(string FilePath, string FileTypeID, PsychonautsSettings Settings);
}