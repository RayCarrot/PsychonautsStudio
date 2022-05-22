using System.Collections.Generic;
using System.IO;
using PsychoPortal;

namespace PsychonautsTools;

public class DataNode_StubSharedSkelAnim : BinaryDataNode<StubSharedSkelAnim>
{
    public DataNode_StubSharedSkelAnim(StubSharedSkelAnim stubSharedSkelAnim, string displayName) : base(stubSharedSkelAnim)
    {
        DisplayName = displayName;
    }

    public override string TypeDisplayName => "Stub Skeleton Animation";
    public override string DisplayName { get; }
    public override bool HasChildren => SerializableObject.EventAnim != null ||
                                        SerializableObject.BlendAnim != null;

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        if (SerializableObject.EventAnim != null)
        {
            string name = Path.GetFileName(Path.ChangeExtension(SerializableObject.JANFileName, ".eve"));
            EventAnim eventAnim = Binary.ReadFromBuffer<EventAnim>(SerializableObject.EventAnim, fileContext.Settings,
                logger: fileContext.Logger, name: name);
            yield return new DataNode_EventAnim(eventAnim, name);
        }

        if (SerializableObject.BlendAnim != null)
        {
            string name = Path.GetFileName(Path.ChangeExtension(SerializableObject.JANFileName, ".pba"));
            SharedBlendAnim blendAnim = Binary.ReadFromBuffer<SharedBlendAnim>(SerializableObject.BlendAnim, fileContext.Settings,
                logger: fileContext.Logger, name: name);
            yield return new DataNode_SharedBlendAnim(blendAnim, name);
        }
    }
}