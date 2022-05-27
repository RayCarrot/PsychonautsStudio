using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ImageMagick;
using PsychoPortal;

namespace PsychonautsTools;

public static class TextureHelpers
{
    #region General

    public const double DpiX = 96;
    public const double DpiY = 96;

    public static int GetStride(int width, PixelFormat format, int align = 0)
    {
        int stride = (int)(width / (8f / format.BitsPerPixel));

        if (align != 0)
        {
            if (stride % align != 0)
                stride += align - stride % align;
        }

        return stride;
    }

    #endregion

    #region Game Texture

    private static void SwapRedBlueChannels(byte[] imgData, TextureFormat format)
    {
        int bpp = format switch
        {
            TextureFormat.Format_8888 => 4,
            TextureFormat.Format_0888 => 3,
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, "Unsupported format")
        };

        for (int i = 0; i < imgData.Length; i += bpp)
            (imgData[i + 0], imgData[i + 2]) = (imgData[i + 2], imgData[i + 0]);
    }

    private static BitmapSource CreateBitmapImage(MipSurface surface, TextureFormat format, int width, int height)
    {
        byte[] imgData = surface.ImageData;

        // Decompress if compressed
        if (format is TextureFormat.Format_DXT1 or TextureFormat.Format_DXT3 or TextureFormat.Format_DXT5)
        {
            using MemoryStream stream = new(imgData);

            imgData = format switch
            {
                TextureFormat.Format_DXT1 => DecompressDXT1(stream, width, height),
                TextureFormat.Format_DXT3 => DecompressDXT3(stream, width, height),
                TextureFormat.Format_DXT5 => DecompressDXT5(stream, width, height),
                _ => throw new ArgumentOutOfRangeException(nameof(format), format, "Unsupported compressed format")
            };

            format = TextureFormat.Format_8888;
        }

        SwapRedBlueChannels(imgData, format);

        PixelFormat pixelFormat = format switch
        {
            TextureFormat.Format_8888 => PixelFormats.Bgra32,
            TextureFormat.Format_0888 => PixelFormats.Bgr24,
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, "Unsupported format")
        };

        return BitmapSource.Create(width, height, DpiX, DpiY, pixelFormat, null, imgData, GetStride(width, pixelFormat));
    }

    public static BitmapSource? ToImageSource(this TextureFrame frame)
    {
        // TODO: Handle PS2 textures

        BitmapSource? imgSource = null;

        if (frame.Type == TextureType.Bitmap)
        {
            MipSurfaces face = frame.Faces[0];
            imgSource = CreateBitmapImage(face.Surfaces[0], frame.Format, (int)frame.Width, (int)frame.Height);
        }
        else
        {
            // TODO: Other texture types
        }

        imgSource?.Freeze();

        return imgSource;
    }

    #endregion

    #region Bitmap

    public static void SaveAsPNG(this BitmapSource image, string filePath)
    {
        PngBitmapEncoder encoder = new();
        encoder.Frames.Add(BitmapFrame.Create(image));

        using FileStream fileStream = new(filePath, FileMode.Create);
        encoder.Save(fileStream);
    }

    #endregion

    #region Decompress DXT

    public static byte[] DecompressDXT1(Stream stream, int width, int height, int depth = 1)
    {
        using var reader = new Reader(stream, leaveOpen: true);

        // allocate bitmap
        const int bpp = 4;
        const int bpc = 1;
        int bps = width * bpp * bpc; // 1024
        int sizeofplane = bps * height; // 1024

        // DXT1 decompressor
        byte[] rawData = new byte[depth * sizeofplane + height * bps + width * bpp];

        Color8888[] colors = new Color8888[4];
        colors[0].Alpha = 0xFF;
        colors[1].Alpha = 0xFF;
        colors[2].Alpha = 0xFF;

        for (int z = 0; z < depth; z++)
        {
            for (int y = 0; y < height; y += 4)
            {
                for (int x = 0; x < width; x += 4)
                {
                    ushort colour0 = reader.ReadUInt16();
                    ushort colour1 = reader.ReadUInt16();
                    DxtcReadColor(colour0, ref colors[0]);
                    DxtcReadColor(colour1, ref colors[1]);

                    uint bitmask = reader.ReadUInt32();

                    if (colour0 > colour1)
                    {
                        // Four-color block: derive the other two colors.
                        // 00 = color_0, 01 = color_1, 10 = color_2, 11 = color_3
                        // These 2-bit codes correspond to the 2-bit fields
                        // stored in the 64-bit block.
                        colors[2].Blue = (byte)((2 * colors[0].Blue + colors[1].Blue + 1) / 3);
                        colors[2].Green = (byte)((2 * colors[0].Green + colors[1].Green + 1) / 3);
                        colors[2].Red = (byte)((2 * colors[0].Red + colors[1].Red + 1) / 3);
                        //colours[2].alpha = 0xFF;

                        colors[3].Blue = (byte)((colors[0].Blue + 2 * colors[1].Blue + 1) / 3);
                        colors[3].Green = (byte)((colors[0].Green + 2 * colors[1].Green + 1) / 3);
                        colors[3].Red = (byte)((colors[0].Red + 2 * colors[1].Red + 1) / 3);
                        colors[3].Alpha = 0xFF;
                    }
                    else
                    {
                        // Three-color block: derive the other color.
                        // 00 = color_0,  01 = color_1,  10 = color_2,
                        // 11 = transparent.
                        // These 2-bit codes correspond to the 2-bit fields 
                        // stored in the 64-bit block. 
                        colors[2].Blue = (byte)((colors[0].Blue + colors[1].Blue) / 2);
                        colors[2].Green = (byte)((colors[0].Green + colors[1].Green) / 2);
                        colors[2].Red = (byte)((colors[0].Red + colors[1].Red) / 2);
                        //colours[2].alpha = 0xFF;

                        colors[3].Blue = (byte)((colors[0].Blue + 2 * colors[1].Blue + 1) / 3);
                        colors[3].Green = (byte)((colors[0].Green + 2 * colors[1].Green + 1) / 3);
                        colors[3].Red = (byte)((colors[0].Red + 2 * colors[1].Red + 1) / 3);
                        colors[3].Alpha = 0x00;
                    }

                    for (int j = 0, k = 0; j < 4; j++)
                    {
                        for (int i = 0; i < 4; i++, k++)
                        {
                            int select = (int)((bitmask & (0x03 << k * 2)) >> k * 2);
                            Color8888 col = colors[select];
                            if (((x + i) < width) && ((y + j) < height))
                            {
                                uint offset = (uint)(z * sizeofplane + (y + j) * bps + (x + i) * bpp);
                                rawData[offset + 0] = col.Red;
                                rawData[offset + 1] = col.Green;
                                rawData[offset + 2] = col.Blue;
                                rawData[offset + 3] = col.Alpha;
                            }
                        }
                    }
                }
            }
        }

        return rawData;
    }

    public static byte[] DecompressDXT3(Stream stream, int width, int height, int depth = 1)
    {
        using var reader = new Reader(stream, leaveOpen: true);

        // allocate bitmap
        const int bpp = 4;
        const int bpc = 1;
        int bps = width * bpp * bpc;
        int sizeofplane = bps * height;

        // DXT3 decompressor
        byte[] rawData = new byte[depth * sizeofplane + height * bps + width * bpp];
        Color8888[] colors = new Color8888[4];

        for (int z = 0; z < depth; z++)
        {
            for (int y = 0; y < height; y += 4)
            {
                for (int x = 0; x < width; x += 4)
                {
                    var alpha = reader.ReadBytes(8);

                    DxtcReadColors(reader, ref colors);

                    var bitmask = reader.ReadUInt32();

                    // Four-color block: derive the other two colors.
                    // 00 = color_0, 01 = color_1, 10 = color_2, 11	= color_3
                    // These 2-bit codes correspond to the 2-bit fields
                    // stored in the 64-bit block.
                    colors[2].Blue = (byte)((2 * colors[0].Blue + colors[1].Blue + 1) / 3);
                    colors[2].Green = (byte)((2 * colors[0].Green + colors[1].Green + 1) / 3);
                    colors[2].Red = (byte)((2 * colors[0].Red + colors[1].Red + 1) / 3);
                    //colours[2].alpha = 0xFF;

                    colors[3].Blue = (byte)((colors[0].Blue + 2 * colors[1].Blue + 1) / 3);
                    colors[3].Green = (byte)((colors[0].Green + 2 * colors[1].Green + 1) / 3);
                    colors[3].Red = (byte)((colors[0].Red + 2 * colors[1].Red + 1) / 3);
                    //colours[3].alpha = 0xFF;

                    for (int j = 0, k = 0; j < 4; j++)
                    {
                        for (int i = 0; i < 4; k++, i++)
                        {
                            int select = (int)((bitmask & (0x03 << k * 2)) >> k * 2);

                            if (((x + i) < width) && ((y + j) < height))
                            {
                                uint offset = (uint)(z * sizeofplane + (y + j) * bps + (x + i) * bpp);
                                rawData[offset + 0] = colors[@select].Red;
                                rawData[offset + 1] = colors[@select].Green;
                                rawData[offset + 2] = colors[@select].Blue;
                            }
                        }
                    }

                    for (int j = 0; j < 4; j++)
                    {
                        //ushort word = (ushort)(alpha[2 * j] + 256 * alpha[2 * j + 1]);
                        ushort word = (ushort)(alpha[2 * j] | (alpha[2 * j + 1] << 8));
                        for (int i = 0; i < 4; i++)
                        {
                            if (((x + i) < width) && ((y + j) < height))
                            {
                                uint offset = (uint)(z * sizeofplane + (y + j) * bps + (x + i) * bpp + 3);
                                rawData[offset] = (byte)(word & 0x0F);
                                rawData[offset] = (byte)(rawData[offset] | (rawData[offset] << 4));
                            }
                            word >>= 4;
                        }
                    }
                }
            }
        }
        return rawData;
    }

    public static byte[] DecompressDXT5(Stream stream, int width, int height, int depth = 1)
    {
        using var reader = new Reader(stream, leaveOpen: true);

        // allocate bitmap
        const int bpp = 4;
        const int bpc = 1;
        int bps = width * bpp * bpc;
        int sizeofplane = bps * height;

        byte[] rawData = new byte[depth * sizeofplane + height * bps + width * bpp];
        Color8888[] colors = new Color8888[4];
        ushort[] alphas = new ushort[8];

        //int temp = 0;
        for (int z = 0; z < depth; z++)
        {
            for (int y = 0; y < height; y += 4)
            {
                for (int x = 0; x < width; x += 4)
                {
                    if (y >= height || x >= width)
                        break;
                    alphas[0] = reader.ReadByte();
                    alphas[1] = reader.ReadByte();
                    var alphamask = reader.ReadBytes(6);

                    DxtcReadColors(reader, ref colors);
                    uint bitmask = reader.ReadUInt32();

                    // Four-color block: derive the other two colors.
                    // 00 = color_0, 01 = color_1, 10 = color_2, 11	= color_3
                    // These 2-bit codes correspond to the 2-bit fields
                    // stored in the 64-bit block.
                    colors[2].Blue = (byte)((2 * colors[0].Blue + colors[1].Blue + 1) / 3);
                    colors[2].Green = (byte)((2 * colors[0].Green + colors[1].Green + 1) / 3);
                    colors[2].Red = (byte)((2 * colors[0].Red + colors[1].Red + 1) / 3);
                    //colours[2].alpha = 0xFF;

                    colors[3].Blue = (byte)((colors[0].Blue + 2 * colors[1].Blue + 1) / 3);
                    colors[3].Green = (byte)((colors[0].Green + 2 * colors[1].Green + 1) / 3);
                    colors[3].Red = (byte)((colors[0].Red + 2 * colors[1].Red + 1) / 3);
                    //colours[3].alpha = 0xFF;

                    int k = 0;
                    for (int j = 0; j < 4; j++)
                    {
                        for (int i = 0; i < 4; k++, i++)
                        {
                            int select = (int)((bitmask & (0x03 << k * 2)) >> k * 2);
                            Color8888 col = colors[select];
                            // only put pixels out < width or height
                            if (((x + i) < width) && ((y + j) < height))
                            {
                                uint offset = (uint)(z * sizeofplane + (y + j) * bps + (x + i) * bpp);
                                rawData[offset] = col.Red;
                                rawData[offset + 1] = col.Green;
                                rawData[offset + 2] = col.Blue;
                            }
                        }
                    }

                    // 8-alpha or 6-alpha block?
                    if (alphas[0] > alphas[1])
                    {
                        // 8-alpha block:  derive the other six alphas.
                        // Bit code 000 = alpha_0, 001 = alpha_1, others are interpolated.
                        alphas[2] = (ushort)((6 * alphas[0] + 1 * alphas[1] + 3) / 7); // bit code 010
                        alphas[3] = (ushort)((5 * alphas[0] + 2 * alphas[1] + 3) / 7); // bit code 011
                        alphas[4] = (ushort)((4 * alphas[0] + 3 * alphas[1] + 3) / 7); // bit code 100
                        alphas[5] = (ushort)((3 * alphas[0] + 4 * alphas[1] + 3) / 7); // bit code 101
                        alphas[6] = (ushort)((2 * alphas[0] + 5 * alphas[1] + 3) / 7); // bit code 110
                        alphas[7] = (ushort)((1 * alphas[0] + 6 * alphas[1] + 3) / 7); // bit code 111
                    }
                    else
                    {
                        // 6-alpha block.
                        // Bit code 000 = alpha_0, 001 = alpha_1, others are interpolated.
                        alphas[2] = (ushort)((4 * alphas[0] + 1 * alphas[1] + 2) / 5); // Bit code 010
                        alphas[3] = (ushort)((3 * alphas[0] + 2 * alphas[1] + 2) / 5); // Bit code 011
                        alphas[4] = (ushort)((2 * alphas[0] + 3 * alphas[1] + 2) / 5); // Bit code 100
                        alphas[5] = (ushort)((1 * alphas[0] + 4 * alphas[1] + 2) / 5); // Bit code 101
                        alphas[6] = 0x00; // Bit code 110
                        alphas[7] = 0xFF; // Bit code 111
                    }

                    // Note: Have to separate the next two loops,
                    // it operates on a 6-byte system.

                    // First three bytes
                    //uint bits = (uint)(alphamask[0]);
                    uint bits = (uint)((alphamask[0]) | (alphamask[1] << 8) | (alphamask[2] << 16));
                    for (int j = 0; j < 2; j++)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            // only put pixels out < width or height
                            if (((x + i) < width) && ((y + j) < height))
                            {
                                uint offset = (uint)(z * sizeofplane + (y + j) * bps + (x + i) * bpp + 3);
                                rawData[offset] = (byte)alphas[bits & 0x07];
                            }
                            bits >>= 3;
                        }
                    }

                    // Last three bytes
                    //bits = (uint)(alphamask[3]);
                    bits = (uint)((alphamask[3]) | (alphamask[4] << 8) | (alphamask[5] << 16));
                    for (int j = 2; j < 4; j++)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            // only put pixels out < width or height
                            if (((x + i) < width) && ((y + j) < height))
                            {
                                uint offset = (uint)(z * sizeofplane + (y + j) * bps + (x + i) * bpp + 3);
                                rawData[offset] = (byte)alphas[bits & 0x07];
                            }
                            bits >>= 3;
                        }
                    }

                    //if (header?.PixelFormat?.Flags != null && header.PixelFormat.Flags.HasFlag(DDS_PixelFormat.DDS_PixelFormatFlags.DDPF_NORMAL))
                    //{

                    //    for (int j = 0; j < 4; j++)
                    //    {
                    //        for (int i = 0; i < 4; i++)
                    //        {
                    //            uint offset = (uint)(z * sizeofplane + (y + j) * bps + (x + i) * bpp);
                    //            uint normal = BuildNormal(rawData[offset + 3], rawData[offset + 1]);
                    //            rawData[offset + 0] = (byte)(normal & 0xFF);
                    //            rawData[offset + 1] = (byte)((normal >> 8) & 0xFF);
                    //            rawData[offset + 2] = (byte)((normal >> 16) & 0xFF);
                    //            rawData[offset + 3] = 0xFF;
                    //        }
                    //    }
                    //}
                }
            }
        }

        return rawData;
    }

    private static void DxtcReadColors(Reader reader, ref Color8888[] op)
    {
        byte byte_0 = reader.ReadByte();
        byte byte_1 = reader.ReadByte();
        byte byte_2 = reader.ReadByte();
        byte byte_3 = reader.ReadByte();

        byte b0 = (byte)(byte_0 & 0x1F);
        byte g0 = (byte)(((byte_0 & 0xE0) >> 5) | ((byte_1 & 0x7) << 3));
        byte r0 = (byte)((byte_1 & 0xF8) >> 3);

        byte b1 = (byte)(byte_2 & 0x1F);
        byte g1 = (byte)(((byte_2 & 0xE0) >> 5) | ((byte_3 & 0x7) << 3));
        byte r1 = (byte)((byte_3 & 0xF8) >> 3);

        op[0].Red = (byte)(r0 << 3 | r0 >> 2);
        op[0].Green = (byte)(g0 << 2 | g0 >> 3);
        op[0].Blue = (byte)(b0 << 3 | b0 >> 2);

        op[1].Red = (byte)(r1 << 3 | r1 >> 2);
        op[1].Green = (byte)(g1 << 2 | g1 >> 3);
        op[1].Blue = (byte)(b1 << 3 | b1 >> 2);
    }

    private static void DxtcReadColor(ushort data, ref Color8888 op)
    {
        byte b = (byte)(data & 0x1f);
        byte g = (byte)((data & 0x7E0) >> 5);
        byte r = (byte)((data & 0xF800) >> 11);

        op.Red = (byte)(r << 3 | r >> 2);
        op.Green = (byte)(g << 2 | g >> 3);
        op.Blue = (byte)(b << 3 | r >> 2);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct Color8888
    {
        public byte Red;
        public byte Green;
        public byte Blue;
        public byte Alpha;
    }

    #endregion
}