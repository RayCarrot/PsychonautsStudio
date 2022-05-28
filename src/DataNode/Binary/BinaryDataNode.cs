using System;
using System.Collections.Generic;
using System.Linq;
using MahApps.Metro.IconPacks;
using Microsoft.Extensions.DependencyInjection;
using PsychoPortal;

namespace PsychonautsTools;

public abstract class BinaryDataNode : DataNode
{
    protected BinaryDataNode(IBinarySerializable serializableObject)
    {
        SerializableObject = serializableObject;
    }

    public IBinarySerializable SerializableObject { get; protected set; }

    public override IEnumerable<UIItem> GetUIActions(FileContext fileContext) => base.GetUIActions(fileContext).AppendGroup(new UIItem[]
    {
        new UIAction("Log to file", PackIconMaterialKind.FileDocumentOutline, () => LogToFile(fileContext.Settings)),
    });

    public void LogToFile(PsychonautsSettings settings)
    {
        AppUIManager appUI = ServiceProvider.GetRequiredService<AppUIManager>();
        string? logFile = appUI.SaveFile("Select text file to log to", $"{DisplayName} - Log.txt", new[] { "txt" }, false);

        if (logFile == null)
            return;

        using BinarySerializerLogger logger = new(logFile);
        BinaryDummySerializer s = new(settings, logger);
        SerializableObject.Serialize(s);
    }
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