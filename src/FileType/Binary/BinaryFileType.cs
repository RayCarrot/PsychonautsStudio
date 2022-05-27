using PsychoPortal;

namespace PsychonautsTools;

public abstract class BinaryFileType<T> : IFileType
    where T : IBinarySerializable, new()
{
    public abstract string[] FileExtensions { get; }
    public virtual bool LeaveFileStreamOpen => false;
    public abstract string ID { get; }
    public abstract string DisplayName { get; }

    protected abstract DataNode CreateDataNode(T obj, FileContext fileContext);

    protected virtual void OnPreSerialize(FileContext fileContext, T obj) { }

    public DataNode CreateDataNode(FileContext fileContext)
    {
        // Serialize the object
        T obj = Binary.ReadFromStream<T>(fileContext.FileStream, fileContext.Settings, 
            logger: fileContext.Logger, onPreSerializing: (_, o) => OnPreSerialize(fileContext, o));

        return CreateDataNode(obj, fileContext);
    }
}