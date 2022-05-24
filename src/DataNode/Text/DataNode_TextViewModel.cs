namespace PsychonautsTools;

public class DataNode_TextViewModel : BaseViewModel
{
    public DataNode_TextViewModel(DataNode_Text node)
    {
        Node = node;
        Text = node.Text;
    }

    public DataNode_Text Node { get; }
    public string Text { get; set; }
}