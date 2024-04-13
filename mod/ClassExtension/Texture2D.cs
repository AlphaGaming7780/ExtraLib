using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using UnityEngine;

namespace Extra.Lib;

public static class Texture2DExtension
{
    public static Texture2D Format(this Texture2D texture2D, TextureFormat newTextureFormat)
    {
        Texture2D newTexture2D = new(texture2D.width, texture2D.height, newTextureFormat, true);

        for (int i = 0; i < texture2D.mipmapCount; i++)
        {
            newTexture2D.SetPixels(texture2D.GetPixels(i), i);
        }

        newTexture2D.Apply();
        return newTexture2D;
    }

    public static Texture2D ResizeTexture(this Texture2D texture, int newSize)
    {
        if (texture is null) throw new NullReferenceException("@ Texture2D ResizeTexture(this Texture2D texture, int newSize), texture is null.");
        RenderTexture scaledRT = RenderTexture.GetTemporary(newSize, newSize);
        Graphics.Blit(texture, scaledRT);

        Texture2D outputTexture = new(newSize, newSize, texture.format, true);

        RenderTexture.active = scaledRT;
        outputTexture.ReadPixels(new Rect(0, 0, newSize, newSize), 0, 0);

        outputTexture.Apply();

        // Clean up
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(scaledRT);
        return outputTexture;
    }

    public static Texture2D GetTextureFromNonReadable(this Texture2D texture2D)
    {
        RenderTexture scaledRT = RenderTexture.GetTemporary(texture2D.width, texture2D.height);
        Graphics.Blit(texture2D, scaledRT);

        Texture2D outputTexture = new(texture2D.width, texture2D.height, TextureFormat.RGBA32, true);

        RenderTexture.active = scaledRT;
        outputTexture.ReadPixels(new Rect(0, 0, texture2D.width, texture2D.height), 0, 0);

        outputTexture.Apply();

        // Clean up
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(scaledRT);

        return outputTexture;
    }

    public static void SaveTextureAsPNG(this Texture2D texture2D, string path)
    {
        Directory.CreateDirectory(new FileInfo(path).DirectoryName);
        File.WriteAllBytes(path, texture2D.EncodeToPNG());
    }

}
