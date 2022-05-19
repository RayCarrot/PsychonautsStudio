using System;
using System.Collections.Generic;
using PsychoPortal;

namespace PsychonautsTools;

public class DataNode_AnimPack : DataNode
{
    public DataNode_AnimPack(AnimPack animPack, string fileName)
    {
        AnimPack = animPack;
        FileName = fileName;
    }

    public AnimPack AnimPack { get; }
    public string FileName { get; }

    public override string TypeDisplayName => "Animation Pack";
    public override string DisplayName => FileName;
    public override bool HasChildren => AnimPack.StubSharedAnims.AnyAndNotNull();
    public override IBinarySerializable SerializableObject => AnimPack;

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        if (AnimPack.StubSharedAnims != null)
            yield return DataNode_Folder.FromTypedFiles(
                files: AnimPack.StubSharedAnims,
                getFilePathFunc: x => x.JANFileName,
                createFileNodeFunc: (file, fileName) => new Lazy<DataNode>(new DataNode_StubSharedSkelAnim(file, fileName)));
    }
}