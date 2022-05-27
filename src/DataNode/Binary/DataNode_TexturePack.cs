using PsychoPortal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PsychonautsTools;

public class DataNode_TexturePack : BinaryDataNode<TexturePack>
{
    public DataNode_TexturePack(TexturePack texturePack, string displayName) : base(texturePack)
    {
        DisplayName = displayName;
    }

    public override string TypeDisplayName => "Texture Pack";
    public override string DisplayName { get; }
    public override bool HasChildren => SerializableObject.Textures.AnyAndNotNull() ||
                                        SerializableObject.LocalizedTextures.AnyAndNotNull();

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        var textures = SerializableObject.Textures.Select(x => new { Texture = x, FileName = x.FileName.Value });

        if (SerializableObject.LocalizedTextures != null)
            foreach (LocalizedTextures locTextures in SerializableObject.LocalizedTextures)
                textures = textures.Concat(locTextures.Textures.Select(x => new
                {
                    Texture = x,
                    FileName = x.FileName.Value.Insert(x.FileName.Value.LastIndexOf('.'), $"_{locTextures.Language.ToString().ToLower()}"),
                }));

        yield return DataNode_Folder.FromTypedFiles(
            files: textures,
            getFilePathFunc: x => x.FileName,
            createFileNodeFunc: (file, fileName) => new Lazy<DataNode>(() =>
            {
                if (file.Texture.Frames.Length > 1)
                {
                    // Animated, wrap everything in an animated texture node
                    return new DataNode_AnimatedGameTexture(file.Texture, fileName);
                }
                else
                {
                    // Not animated, so we use the texture frame as is
                    return new DataNode_TextureFrame(file.Texture.Frames[0], fileName);
                }
            }));
    }
}