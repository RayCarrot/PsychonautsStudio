using System;
using System.Collections.Generic;
using System.IO;
using PsychoPortal;

namespace PsychonautsStudio;

public class FileContext : IDisposable
{
    public FileContext(string filePath, Stream fileStream, PsychonautsSettings settings, IBinarySerializerLogger? logger)
    {
        FilePath = filePath;
        _fileStream = fileStream;
        Settings = settings;
        Logger = logger;
    }

    private readonly Dictionary<string, FileContext> _dependencies = new();
    private readonly HashSet<FileContext> _children = new();
    private Stream? _fileStream;

    public string FilePath { get; }
    public Stream FileStream => _fileStream ?? throw new ObjectDisposedException(nameof(FileStream));
    public PsychonautsSettings Settings { get; }
    public IBinarySerializerLogger? Logger { get; }

    public bool HasDependency(string key) => _dependencies.ContainsKey(key);
    public void AddDependency(string key, string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File {filePath} does not exist", filePath);

        _dependencies.Add(key, new FileContext(filePath, File.OpenRead(filePath), Settings, Logger));
    }
    public FileContext GetDependency(string key) => _dependencies[key];

    public void AddChild(FileContext child)
    {
        _children.Add(child);
    }

    public void Dispose()
    {
        _fileStream?.Dispose();
        _fileStream = null;

        foreach (FileContext dep in _dependencies.Values)
            dep.Dispose();
        
        foreach (FileContext child in _children)
            child.Dispose();
    }
}