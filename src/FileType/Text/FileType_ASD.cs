namespace PsychonautsTools;

public class FileType_ASD : TextFileType
{
    public override string[] FileExtensions => new[] { ".asd" };
    public override string ID => "ASD";
    public override string DisplayName => "Animation Actions (.asd)";
    public override string TypeDisplayName => "Animation Actions";
}