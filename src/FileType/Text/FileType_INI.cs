﻿namespace PsychonautsTools;

public class FileType_INI : TextFileType
{
    public override string[] FileExtensions => new[] { ".ini" };
    public override string ID => "INI";
    public override string DisplayName => "Initialization (.ini)";
}