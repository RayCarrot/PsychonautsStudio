using PsychoPortal;
using System;
using System.Collections.Generic;

namespace PsychonautsTools;

public class DataNode_MeshPack : BinaryDataNode<MeshPack>
{
    public DataNode_MeshPack(MeshPack meshPack, string displayName) : base(meshPack)
    {
        DisplayName = displayName;
    }

    public override string TypeDisplayName => "Mesh Pack";
    public override string DisplayName { get; }
    public override bool HasChildren => SerializableObject.MeshFiles.AnyAndNotNull();

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        if (SerializableObject.MeshFiles != null)
            yield return DataNode_Folder.FromTypedFiles(
                files: SerializableObject.MeshFiles,
                getFilePathFunc: x => x.FileName,
                createFileNodeFunc: (file, fileName) => new Lazy<DataNode>(() => new DataNode_Scene(file.Scene, fileName)));
    }
}