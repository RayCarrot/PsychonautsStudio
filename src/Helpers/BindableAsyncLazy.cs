using System;
using System.Threading.Tasks;

namespace PsychonautsTools;

public class BindableAsyncLazy<T> : BaseViewModel
    where T : class
{
    public BindableAsyncLazy(Func<Task<T>> valueFactory)
    {
        _valueFactory = valueFactory;
    }

    private readonly Func<Task<T>> _valueFactory;
    private T? _value;
    private Task<T>? _runningValueTask;

    public T? Value
    {
        get
        {
            if (_value != null)
                return _value;

            if (_runningValueTask != null)
                return null; // In progress
            
            RetrieveValue();
            
            return null; // In progress
        }
        set => _value = value;
    }

    private async void RetrieveValue()
    {
        _runningValueTask = _valueFactory();
        Value = await _runningValueTask;
        _runningValueTask = null;
    }
}