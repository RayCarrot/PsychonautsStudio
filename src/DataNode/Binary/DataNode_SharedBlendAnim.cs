using System;
using System.Collections.Generic;
using System.Linq;
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

    public override IEnumerable<InfoItem> GetInfoItems()
    {
        foreach (InfoItem item in base.GetInfoItems())
            yield return item;

        yield return new InfoItem("Version", $"{SerializableObject.Version}");
        yield return new InfoItem("Channels", $"{String.Join(", ", SerializableObject.Channels.Select(x => x.Name))}");
    }
}