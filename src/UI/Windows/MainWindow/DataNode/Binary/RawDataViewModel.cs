using System.IO;
using System.Threading.Tasks;
using PsychoPortal;

namespace PsychonautsTools;

public class RawDataViewModel : BaseViewModel
{
    public RawDataViewModel(IBinarySerializable serializableObject, PsychonautsSettings settings)
    {
        SerializableObject = serializableObject;
        RawData = new BindableAsyncLazy<BinaryReader>(() => Task.Run(() =>
        {
            MemoryStream stream = new();
            BinarySerializer s = new(stream, settings);
            SerializableObject.Serialize(s);
            return new BinaryReader(stream);
        }));
    }

    public IBinarySerializable SerializableObject { get; }
    public BindableAsyncLazy<BinaryReader> RawData { get; }
}