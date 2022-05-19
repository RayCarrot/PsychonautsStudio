using PsychoPortal;

namespace PsychonautsTools;

public class DataNode_EventAnim : DataNode
{
    public DataNode_EventAnim(EventAnim eventAnim, string displayName)
    {
        EventAnim = eventAnim;
        DisplayName = displayName;
    }

    public EventAnim EventAnim { get; }

    public override string TypeDisplayName => "Event Animation";
    public override string DisplayName { get; }
    public override IBinarySerializable SerializableObject => EventAnim;
}