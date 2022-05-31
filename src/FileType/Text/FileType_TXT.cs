namespace PsychonautsStudio;

// The game uses this for raw event animations. But since .txt is such a common extension it's better treating it as simply "text".
public class FileType_TXT : TextFileType
{
    public override string[] FileExtensions => new[] { ".txt" };
    public override string ID => "TXT";
    public override string DisplayName => "Text (.txt)";
    public override string TypeDisplayName => "Text";
}