using PsychoPortal;

namespace PsychonautsTools;

public abstract class BinaryFileType<T> : IFileType
    where T : IBinarySerializable, new()
{
    public abstract string[] FileExtensions { get; }
    public virtual bool LeaveFileStreamOpen => true;
    public abstract string ID { get; }
    public abstract string DisplayName { get; }

    protected abstract DataNode CreateDataNode(T obj, FileContext fileContext);

    protected virtual void OnPreSerialize(FileContext fileContext, T obj) { }

    public DataNode CreateDataNode(FileContext fileContext)
    {
        // Create the deserializer
        BinaryDeserializer s = new(fileContext.FileStream, fileContext.Settings, logger: fileContext.Logger);

        // Create the serializable object
        T obj = new();

        OnPreSerialize(fileContext, obj);

        // Serialize the object
        obj.Serialize(s);

        return CreateDataNode(obj, fileContext);
    }
}