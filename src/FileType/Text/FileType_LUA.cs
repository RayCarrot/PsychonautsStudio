namespace PsychonautsStudio;

public class FileType_LUA : TextFileType
{
    public override string[] FileExtensions => new[] { ".lua" };
    public override string ID => "LUA";
    public override string DisplayName => "Script (.vsh)";
    public override string TypeDisplayName => "Script";
}