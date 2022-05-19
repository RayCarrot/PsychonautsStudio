using PsychoPortal;

namespace PsychonautsTools;

public class DataNode_SharedBlendAnim : DataNode
{
    public DataNode_SharedBlendAnim(SharedBlendAnim blendAnim, string displayName)
    {
        BlendAnim = blendAnim;
        DisplayName = displayName;
    }

    public SharedBlendAnim BlendAnim { get; }

    public override string TypeDisplayName => "Blend Animation";
    public override string DisplayName { get; }
    public override IBinarySerializable SerializableObject => BlendAnim;
}