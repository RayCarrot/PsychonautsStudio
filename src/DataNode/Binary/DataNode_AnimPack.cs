using System;
using System.Collections.Generic;
using PsychoPortal;

namespace PsychonautsTools;

public class DataNode_AnimPack : BinaryDataNode<AnimPack>
{
    public DataNode_AnimPack(AnimPack animPack, string displayName) : base(animPack)
    {
        DisplayName = displayName;
    }

    public override string TypeDisplayName => "Animation Pack";
    public override string DisplayName { get; }
    public override bool HasChildren => SerializableObject.StubSharedAnims.AnyAndNotNull();

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        if (SerializableObject.StubSharedAnims != null)
            yield return DataNode_Folder.FromTypedFiles(
                files: SerializableObject.StubSharedAnims,
                getFilePathFunc: x => x.JANFileName,
                createFileNodeFunc: (file, fileName) => new Lazy<DataNode>(new DataNode_StubSharedSkelAnim(file, fileName)));
    }
}