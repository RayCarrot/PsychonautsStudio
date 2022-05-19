using PsychoPortal;
using System;
using System.Collections.Generic;
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
    public override IBinarySerializable SerializableObject => Scene;

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
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
                createFileNodeFunc: (file, fileName) => new Lazy<DataNode>(() => new DataNode_Scene(file.Scene, fileName)));
        }

        yield return new DataNode_Domain(Scene.RootDomain);
    }
}