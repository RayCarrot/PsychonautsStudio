using System.Collections.Generic;
using System.IO;
using PsychoPortal;

namespace PsychonautsTools;

public class DataNode_StubSharedSkelAnim : DataNode
{
    public DataNode_StubSharedSkelAnim(StubSharedSkelAnim stubSharedSkelAnim, string displayName)
    {
        StubSharedSkelAnim = stubSharedSkelAnim;
        DisplayName = displayName;
    }

    public StubSharedSkelAnim StubSharedSkelAnim { get; }

    public override string TypeDisplayName => "Stub Skeleton Animation";
    public override string DisplayName { get; }
    public override bool HasChildren => StubSharedSkelAnim.EventAnim != null ||
                                        StubSharedSkelAnim.BlendAnim != null;
    public override IBinarySerializable SerializableObject => StubSharedSkelAnim;

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        if (StubSharedSkelAnim.EventAnim != null)
        {
            string name = Path.GetFileName(Path.ChangeExtension(StubSharedSkelAnim.JANFileName, ".eve"));
            EventAnim eventAnim = Binary.ReadFromBuffer<EventAnim>(StubSharedSkelAnim.EventAnim, fileContext.Settings,
                logger: fileContext.Logger, name: name);
            yield return new DataNode_EventAnim(eventAnim, name);
        }

        if (StubSharedSkelAnim.BlendAnim != null)
        {
            string name = Path.GetFileName(Path.ChangeExtension(StubSharedSkelAnim.JANFileName, ".pba"));
            SharedBlendAnim blendAnim = Binary.ReadFromBuffer<SharedBlendAnim>(StubSharedSkelAnim.BlendAnim, fileContext.Settings,
                logger: fileContext.Logger, name: name);
            yield return new DataNode_SharedBlendAnim(blendAnim, name);
        }
    }
}