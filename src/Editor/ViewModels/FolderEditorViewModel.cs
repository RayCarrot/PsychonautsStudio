using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace PsychonautsTools;

public class FolderEditorViewModel : EditorViewModel
{
    public FolderEditorViewModel(Func<IEnumerable<DataNode>> getChildEntriesFunc)
    {
        Items = new Lazy<ObservableCollection<ItemViewModel>>(() => 
            new ObservableCollection<ItemViewModel>(getChildEntriesFunc().Select(x => new ItemViewModel(x))));

        DeselectCommand = new RelayCommand(() => SelectedItem = null);
    }

    public ICommand DeselectCommand { get; }

    public Lazy<ObservableCollection<ItemViewModel>> Items { get; }
    public ItemViewModel? SelectedItem { get; set; }

    public class ItemViewModel : BaseViewModel
    {
        public ItemViewModel(DataNode node)
        {
            Node = node;
        }

        public DataNode Node { get; }
        public bool IsSelected { get; set; }
    }
}