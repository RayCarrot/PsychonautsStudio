using System;
using PsychoPortal;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PsychonautsTools;

public class DataNode_Scene : DataNode
{
    public DataNode_Scene(Scene scene, string displayName)
    {
        Scene = scene;
        DisplayName = displayName;
    }

    public Scene Scene { get; }

    public override string TypeDisplayName => "Scene";
    public override string DisplayName { get; }
    public override bool HasChildren => true;
    public override GenericIconKind IconKind => GenericIconKind.DataNode_Scene;
    public override IBinarySerializable SerializableObject => Scene;

    public override IEnumerable<DataNode> CreateChildren()
    {
        if (Scene.ReferencedScenes.AnyAndNotNull())
        {
            yield return DataNode_Folder.FromTypedFiles(
                files: Scene.ReferencedScenes.Select((x, i) => new
                {
                    Scene = x,
                    FilePath = Scene.RootDomain.RuntimeReferences[i]
                }), 
                getFilePathFunc: x => x.FilePath, 
                createFileNodeFunc: x => new Lazy<DataNode>(() => new DataNode_Scene(x.Scene, Path.GetFileName(x.FilePath))));
        }

        yield return new DataNode_Domain(Scene.RootDomain);
    }
}