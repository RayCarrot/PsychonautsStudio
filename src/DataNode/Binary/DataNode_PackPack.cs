using PsychoPortal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PsychonautsStudio;

public class DataNode_PackPack : BinaryDataNode<PackPack>
{
    public DataNode_PackPack(PackPack packPack, string filePath) : base(packPack)
    {
        FilePath = filePath;
        DisplayName = Path.GetFileName(filePath);
    }

    public string FilePath { get; }

    public override string TypeDisplayName => "Pack Pack";
    public override string DisplayName { get; }
    public override bool HasChildren => true;

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        string name = Path.GetFileNameWithoutExtension(FilePath);

        yield return new DataNode_TexturePack(SerializableObject.TexturePack, $"{name}.tpf");
        yield return new DataNode_MeshPack(SerializableObject.MeshPack, $"{name}.mpf");
        yield return new DataNode_ScriptPack(SerializableObject.ScriptPack, $"{name}.lpf");

        if (SerializableObject.Scene != null)
        {
            yield return new DataNode_Scene(SerializableObject.Scene, $"{name}.plb");

            if (SerializableObject.ReferencedScenes.AnyAndNotNull())
            {
                yield return DataNode_Folder.FromTypedFiles(
                    files: SerializableObject.ReferencedScenes.Select((x, i) => new
                    {
                        Scene = x,
                        FilePath = SerializableObject.Scene.RootDomain.RuntimeReferences[i]
                    }),
                    getFilePathFunc: x => x.FilePath,
                    createFileNodeFunc: (file, fileName) => new Lazy<DataNode>(() => new DataNode_Scene(file.Scene, fileName)));
            }
        }
    }
}