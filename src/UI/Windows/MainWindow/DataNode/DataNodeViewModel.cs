using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PsychonautsStudio;

public class DataNodeViewModel : BaseViewModel, IDisposable
{
    public DataNodeViewModel(DataNode node, DataNodeViewModel? parent, RootDataNodeViewModel? root, FileContext fileContext)
    {
        Node = node;
        Parent = parent;
        Root = root ?? this as RootDataNodeViewModel ?? throw new Exception("The root can not be null on a child node");

        if (Parent == null && this != Root)
            throw new Exception("The parent can not be null on a child node");

        // Create a dummy node if the node should be able to be expanded in the UI
        if (node.HasChildren)
            Children.Add(new DataNodeViewModel(new DataNode_Dummy(), this, Root, fileContext));

        if (Node is BinaryDataNode bin)
        {
            IsBinary = true;
            SerializerLogViewModel = new Lazy<SerializerLogViewModel>(() => 
                new SerializerLogViewModel(bin.SerializableObject, fileContext.Settings));
            RawDataViewModel = new Lazy<RawDataViewModel>(() => 
                new RawDataViewModel(bin.SerializableObject, fileContext.Settings));
        }

        IEnumerable<UIItem> uiActions = node.GetUIActions(fileContext);

        if (node.EditorViewModel != null)
            uiActions = uiActions.AppendGroup(node.EditorViewModel.GetUIActions());

        UIItems = new ObservableCollection<UIItem>(uiActions);
        InfoItems = new ObservableCollection<InfoItem>(node.GetInfoItems());
    }

    private bool _createdChildren;

    public DataNodeViewModel? Parent { get; }
    public RootDataNodeViewModel Root { get; }
    public DataNode Node { get; }
    public object? EditorViewModel => Node.EditorViewModel;
    public ObservableCollection<DataNodeViewModel> Children { get; } = new();
    public ObservableCollection<UIItem> UIItems { get; }
    public ObservableCollection<InfoItem> InfoItems { get; }
    public bool HasInfoItems => InfoItems.Any();
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
            Children.Add(new DataNodeViewModel(childNode, this, Root, Root.FileContext));

        _createdChildren = true;
    }

    public virtual void Dispose()
    {
        Node.Dispose();

        foreach (DataNodeViewModel child in Children)
            child.Dispose();
    }
}