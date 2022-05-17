using System;
using PsychoPortal;
using System.Collections.Generic;
using System.IO;

namespace PsychonautsTools;

public class DataNode_MeshPack : DataNode
{
    public DataNode_MeshPack(MeshPack meshPack, string fileName)
    {
        MeshPack = meshPack;
        FileName = fileName;
    }

    public MeshPack MeshPack { get; }
    public string FileName { get; }

    public override string TypeDisplayName => "Mesh Pack";
    public override string DisplayName => FileName;
    public override bool HasChildren => MeshPack.MeshFiles.AnyAndNotNull();
    public override GenericIconKind IconKind => GenericIconKind.DataNode_MeshPack;
    public override IBinarySerializable SerializableObject => MeshPack;

    public override IEnumerable<DataNode> CreateChildren()
    {
        if (MeshPack.MeshFiles != null)
            yield return DataNode_Folder.FromTypedFiles(
                files: MeshPack.MeshFiles, 
                getFilePathFunc: x => x.FileName, 
                createFileNodeFunc: x => new Lazy<DataNode>(() => new DataNode_Scene(x.Scene, Path.GetFileName(x.FileName))));
    }
}