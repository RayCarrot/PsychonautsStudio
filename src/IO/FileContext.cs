using System;
using System.Collections.Generic;
using System.IO;
using PsychoPortal;

namespace PsychonautsTools;

public record FileContext(string FilePath, Stream FileStream, PsychonautsSettings Settings, IBinarySerializerLogger? Logger) : IDisposable
{
    private readonly Dictionary<string, FileContext> _dependencies = new();

    public bool HasDependency(string key) => _dependencies.ContainsKey(key);
    public void AddDependency(string key, string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File {filePath} does not exist", filePath);

        _dependencies.Add(key, new FileContext(filePath, File.OpenRead(filePath), Settings, Logger));
    }
    public FileContext GetDependency(string key) => _dependencies[key];

    public void Dispose()
    {
        FileStream.Dispose();

        foreach (FileContext dep in _dependencies.Values)
            dep.Dispose();
    }
}