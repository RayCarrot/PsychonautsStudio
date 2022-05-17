using System.Windows.Input;
using PsychoPortal;

namespace PsychonautsTools;

public class DataNodeSerializerLogViewModel : BaseViewModel
{
    public DataNodeSerializerLogViewModel(DataNode node, IBinarySerializable serializableObject, FileContext fileContext)
    {
        Node = node;
        SerializableObject = serializableObject;
        FileContext = fileContext;

        LoadLogCommand = new RelayCommand(LoadLog);
    }

    private const int MaxLogLength = 500000; // Allow this to be configured?

    public ICommand LoadLogCommand { get; }

    public DataNode Node { get; }
    public IBinarySerializable SerializableObject { get; }
    public FileContext FileContext { get; }
    public string? Log { get; set; }

    public void LoadLog()
    {
        BinarySerializerStringLogger logger = new() { MaxLength = MaxLogLength };
        BinaryDummySerializer s = new(FileContext.Settings, logger);
        SerializableObject.Serialize(s);
        Log = logger.StringBuilder.ToString();
    }
}