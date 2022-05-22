using PsychoPortal;

namespace PsychonautsTools;

public class DataNode_SharedSkelAnim : BinaryDataNode<SharedSkelAnim>
{
    public DataNode_SharedSkelAnim(SharedSkelAnim sharedSkelAnim, string displayName) : base(sharedSkelAnim)
    {
        DisplayName = displayName;
    }

    public override string TypeDisplayName => "Skeleton Animation";
    public override string DisplayName { get; }
}