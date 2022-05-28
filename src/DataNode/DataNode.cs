using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace PsychonautsTools;

public abstract class DataNode : IDisposable
{
    protected IServiceProvider ServiceProvider => App.Current.ServiceProvider; // Find better solution for accessing it?

    public abstract string TypeDisplayName { get; }
    public abstract string DisplayName { get; }
    public virtual bool HasChildren => false;
    public virtual ImageSource? IconImageSource => null;
    public virtual EditorViewModel? EditorViewModel => null;

    public virtual IEnumerable<UIItem> GetUIActions() => Array.Empty<UIItem>();
    public virtual IEnumerable<InfoItem> GetInfoItems() => Array.Empty<InfoItem>();

    public virtual IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        yield break;
    }

    public virtual void Dispose()
    {
        EditorViewModel?.Dispose();
    }
}