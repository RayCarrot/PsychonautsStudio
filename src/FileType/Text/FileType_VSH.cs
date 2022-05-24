namespace PsychonautsTools;

public class FileType_VSH : TextFileType
{
    public override string[] FileExtensions => new[] { ".vsh" };
    public override string ID => "VSH";
    public override string DisplayName => "Vertex Shader (.vsh)";
}