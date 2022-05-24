using PsychoPortal;
using System.Threading.Tasks;

namespace PsychonautsTools;

public class SerializerLogViewModel : BaseViewModel
{
    public SerializerLogViewModel(string displayName, IBinarySerializable serializableObject, FileContext fileContext)
    {
        DisplayName = displayName;
        SerializableObject = serializableObject;
        FileContext = fileContext;
        Log = new BindableAsyncLazy<string>(() => Task.Run(() =>
        {
            BinarySerializerStringLogger logger = new() { MaxLength = MaxLogLength };
            BinaryDummySerializer s = new(FileContext.Settings, logger);
            SerializableObject.Serialize(s);
            return logger.StringBuilder.ToString();
        }));
    }

    private const int MaxLogLength = 500000; // Allow this to be configured?

    public string DisplayName { get; }
    public IBinarySerializable SerializableObject { get; }
    public FileContext FileContext { get; }
    public BindableAsyncLazy<string> Log { get; }
}