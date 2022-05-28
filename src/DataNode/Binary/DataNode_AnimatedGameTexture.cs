using System;
using System.Collections.Generic;
using System.Linq;
using PsychoPortal;

namespace PsychonautsTools;

public class DataNode_AnimatedGameTexture : BinaryDataNode<GameTexture>
{
    public DataNode_AnimatedGameTexture(GameTexture texture, string displayName) : base(texture)
    {
        DisplayName = displayName;
    }

    public override string TypeDisplayName => "Animated Game Texture";
    public override string DisplayName { get; }
    public override bool HasChildren => true;

    public override IEnumerable<InfoItem> GetInfoItems()
    {
        foreach (InfoItem item in base.GetInfoItems())
            yield return item;

        if (SerializableObject.AnimInfo != null)
        {
            yield return new InfoItem("Play Mode", $"{SerializableObject.AnimInfo.PlayMode}");
            yield return new InfoItem("Frames", $"{SerializableObject.AnimInfo.FramesCount}");
            yield return new InfoItem("Frames Per Second", $"{SerializableObject.AnimInfo.FramesPerSecond}");
        }
    }

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        for (var i = 0; i < SerializableObject.Frames.Length; i++)
        {
            TextureFrame frame = SerializableObject.Frames[i];
            yield return new DataNode_TextureFrame(frame, $"{i}");
        }
    }
}