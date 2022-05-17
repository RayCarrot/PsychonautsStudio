using System;
using System.IO;
using PsychoPortal;

namespace PsychonautsTools;

public record FileContext(string FilePath, Stream FileStream, PsychonautsSettings Settings, IBinarySerializerLogger? Logger) : IDisposable
{
    public void Dispose() => FileStream.Dispose();
}