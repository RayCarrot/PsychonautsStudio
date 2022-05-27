using System;
using System.Collections.Generic;

namespace PsychonautsTools;

public abstract class EditorViewModel : BaseViewModel, IDisposable
{
    public virtual IEnumerable<UIItem> GetUIActions() => Array.Empty<UIItem>();
    public virtual void Dispose() { }
}