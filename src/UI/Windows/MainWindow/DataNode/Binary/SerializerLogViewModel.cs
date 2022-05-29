using PsychoPortal;
using System.Threading.Tasks;

namespace PsychonautsStudio;

public class SerializerLogViewModel : BaseViewModel
{
    public SerializerLogViewModel(IBinarySerializable serializableObject, PsychonautsSettings settings)
    {
        SerializableObject = serializableObject;
        Log = new BindableAsyncLazy<string>(() => Task.Run(() =>
        {
            BinarySerializerStringLogger logger = new() { MaxLength = MaxLogLength };
            BinaryDummySerializer s = new(settings, logger);
            SerializableObject.Serialize(s);
            return logger.StringBuilder.ToString();
        }));
    }

    private const int MaxLogLength = 500000; // Allow this to be configured?

    public IBinarySerializable SerializableObject { get; }
    public BindableAsyncLazy<string> Log { get; }
}