using System;
using System.Collections.Generic;
using PsychoPortal;

namespace PsychonautsStudio;

public class DataNode_SharedSkelAnim : BinaryDataNode<SharedSkelAnim>
{
    public DataNode_SharedSkelAnim(SharedSkelAnim sharedSkelAnim, string displayName) : base(sharedSkelAnim)
    {
        DisplayName = displayName;
    }

    public override string TypeDisplayName => "Skeleton Animation";
    public override string DisplayName { get; }

    public override IEnumerable<InfoItem> GetInfoItems()
    {
        foreach (InfoItem item in base.GetInfoItems())
            yield return item;

        yield return new InfoItem("Version", $"{SerializableObject.Header.Version}");
        yield return new InfoItem("Flags", $"{SerializableObject.Header.Flags}");
        yield return new InfoItem("Joints", $"{SerializableObject.Header.JointsCount}");

        if (SerializableObject.Header.Version >= 201)
        {
            yield return new InfoItem("Length", $"{SerializableObject.Header.V201_AnimLength}");
            yield return new InfoItem("Translation Mode", $"{SerializableObject.Header.V201_TranslateMode}");
        }

        yield return new InfoItem("Root Joint", $"{SerializableObject.RootJoint.Name}");
    }
}