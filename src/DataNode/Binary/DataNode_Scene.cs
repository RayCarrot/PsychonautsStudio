using PsychoPortal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PsychonautsTools;

public class DataNode_Scene : BinaryDataNode<Scene>
{
    public DataNode_Scene(Scene scene, string displayName) : base(scene)
    {
        DisplayName = displayName;
    }

    public override string TypeDisplayName => "Scene";
    public override string DisplayName { get; }
    public override bool HasChildren => true;

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        if (SerializableObject.ReferencedScenes.AnyAndNotNull())
        {
            yield return DataNode_Folder.FromTypedFiles(
                files: SerializableObject.ReferencedScenes.Select((x, i) => new
                {
                    Scene = x,
                    FilePath = SerializableObject.RootDomain.RuntimeReferences[i]
                }),
                getFilePathFunc: x => x.FilePath,
                createFileNodeFunc: (file, fileName) => new Lazy<DataNode>(() => new DataNode_Scene(file.Scene, fileName)));
        }

        yield return new DataNode_Domain(SerializableObject.RootDomain);
    }
}