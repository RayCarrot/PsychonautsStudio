using PsychoPortal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PsychonautsTools;

public class DataNode_Scene : BinaryDataNode<Scene, SceneEditorViewModel>
{
    public DataNode_Scene(Scene scene, string displayName, PLBType? type = null) : base(scene, new SceneEditorViewModel(scene))
    {
        DisplayName = displayName;
        Type = type;
    }

    public PLBType? Type { get; }

    public override string TypeDisplayName => "Scene";
    public override string DisplayName { get; }
    public override bool HasChildren => true;

    public override IEnumerable<InfoItem> GetInfoItems()
    {
        foreach (InfoItem item in base.GetInfoItems())
            yield return item;

        yield return new InfoItem("Type", $"{Type}");
        yield return new InfoItem("Version", $"{SerializableObject.Version}");
        yield return new InfoItem("Flags", $"{SerializableObject.Flags}");
        yield return new InfoItem("Textures", $"{SerializableObject.TextureTranslationTable.Length}");
        yield return new InfoItem("Navigation Meshes", $"{SerializableObject.NavMeshes.Length}");
    }

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