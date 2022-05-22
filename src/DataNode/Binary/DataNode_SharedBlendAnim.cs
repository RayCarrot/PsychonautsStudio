using PsychoPortal;

namespace PsychonautsTools;

public class DataNode_SharedBlendAnim : BinaryDataNode<SharedBlendAnim>
{
    public DataNode_SharedBlendAnim(SharedBlendAnim blendAnim, string displayName) : base(blendAnim)
    {
        DisplayName = displayName;
    }

    public override string TypeDisplayName => "Blend Animation";
    public override string DisplayName { get; }
}