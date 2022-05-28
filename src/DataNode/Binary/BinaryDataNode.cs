using System;
using PsychoPortal;

namespace PsychonautsTools;

public abstract class BinaryDataNode : DataNode
{
    protected BinaryDataNode(IBinarySerializable serializableObject)
    {
        SerializableObject = serializableObject;
    }

    public IBinarySerializable SerializableObject { get; protected set; }
}

public abstract class BinaryDataNode<T> : BinaryDataNode
    where T : class, IBinarySerializable, new()
{
    protected BinaryDataNode(T serializableObject) : base(serializableObject) { }

    public new T SerializableObject
    {
        get => base.SerializableObject as T ?? throw new Exception($"Serializable object is not of correct type {typeof(T)}");
        protected set => base.SerializableObject = value;
    }
}