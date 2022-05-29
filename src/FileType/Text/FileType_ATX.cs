namespace PsychonautsStudio;

public class FileType_ATX : TextFileType
{
    public override string[] FileExtensions => new[] { ".atx" };
    public override string ID => "ATX";
    public override string DisplayName => "Texture Animation (.atx)";
    public override string TypeDisplayName => "Texture Animation";
}