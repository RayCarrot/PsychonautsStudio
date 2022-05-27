using System;
using System.Collections.ObjectModel;

namespace PsychonautsTools;

public class DataNodeViewModel : BaseViewModel, IDisposable
{
    public DataNodeViewModel(DataNode node, DataNodeViewModel? parent, RootDataNodeViewModel? root)
    {
        Node = node;
        Parent = parent;
        Root = root ?? this as RootDataNodeViewModel ?? throw new Exception("The root can not be null on a child node");

        if (Parent == null && this != Root)
            throw new Exception("The parent can not be null on a child node");

        // Create a dummy node if the node should be able to be expanded in the UI
        if (node.HasChildren)
            Children.Add(new DataNodeViewModel(new DataNode_Dummy(), this, Root));

        if (Node is BinaryDataNode bin)
        {
            IsBinary = true;
            SerializerLogViewModel = new Lazy<SerializerLogViewModel>(() => 
                new SerializerLogViewModel(bin.SerializableObject, Root.FileContext.Settings));
            RawDataViewModel = new Lazy<RawDataViewModel>(() => 
                new RawDataViewModel(bin.SerializableObject, Root.FileContext.Settings));
        }

        UIItems = new ObservableCollection<UIItem>(node.GetUIActions());
    }

    private bool _createdChildren;

    public DataNodeViewModel? Parent { get; }
    public RootDataNodeViewModel Root { get; }
    public DataNode Node { get; }
    public ObservableCollection<DataNodeViewModel> Children { get; } = new();
    public ObservableCollection<UIItem> UIItems { get; }
    public object? UI => Node.GetUI();
    public bool IsBinary { get; }
    public Lazy<SerializerLogViewModel>? SerializerLogViewModel { get; }
    public Lazy<RawDataViewModel>? RawDataViewModel { get; }
    public bool IsSelected { get; set; }

    public void Expand()
    {
        if (_createdChildren)
            return;

        Children.Clear();

        foreach (DataNode childNode in Node.CreateChildren(Root.FileContext))
            Children.Add(new DataNodeViewModel(childNode, this, Root));

        _createdChildren = true;
    }

    public virtual void Dispose()
    {
        Node.Dispose();

        foreach (DataNodeViewModel child in Children)
            child.Dispose();
    }
}