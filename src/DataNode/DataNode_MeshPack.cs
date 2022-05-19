using PsychoPortal;
using System;
using System.Collections.Generic;

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
    public override IBinarySerializable SerializableObject => MeshPack;

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        if (MeshPack.MeshFiles != null)
            yield return DataNode_Folder.FromTypedFiles(
                files: MeshPack.MeshFiles, 
                getFilePathFunc: x => x.FileName, 
                createFileNodeFunc: (file, fileName) => new Lazy<DataNode>(() => new DataNode_Scene(file.Scene, fileName)));
    }
}