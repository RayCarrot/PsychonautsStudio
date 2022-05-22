using PsychoPortal;

namespace PsychonautsTools;

public class DataNode_EventAnim : BinaryDataNode<EventAnim>
{
    public DataNode_EventAnim(EventAnim eventAnim, string displayName) : base(eventAnim)
    {
        DisplayName = displayName;
    }

    public override string TypeDisplayName => "Event Animation";
    public override string DisplayName { get; }
}