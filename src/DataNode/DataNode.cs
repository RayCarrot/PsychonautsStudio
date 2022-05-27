using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace PsychonautsTools;

public abstract class DataNode : IDisposable
{
    public abstract string TypeDisplayName { get; }
    public abstract string DisplayName { get; }
    public virtual bool HasChildren => false;
    public virtual ImageSource? IconImageSource => null;

    public virtual object? GetUI() => null;

    public virtual IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        yield break;
    }

    public virtual void Dispose() { }
}