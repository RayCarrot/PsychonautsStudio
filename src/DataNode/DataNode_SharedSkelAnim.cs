using PsychoPortal;

namespace PsychonautsTools;

public class DataNode_SharedSkelAnim : DataNode
{
    public DataNode_SharedSkelAnim(SharedSkelAnim sharedSkelAnim, string displayName)
    {
        SharedSkelAnim = sharedSkelAnim;
        DisplayName = displayName;
    }

    public SharedSkelAnim SharedSkelAnim { get; }

    public override string TypeDisplayName => "Skeleton Animation";
    public override string DisplayName { get; }
    public override IBinarySerializable SerializableObject => SharedSkelAnim;
}