using System.IO;
using UnityEngine;

namespace ExtraLib.Helpers;

public static class TextureHelper
{
    public static void ResizeTexture(ref Texture2D texture2D, int newSize, string savePath = null)
    {

        //if (texture2D)
        //{
        //    return;
        //}

        RenderTexture scaledRT = RenderTexture.GetTemporary(newSize, newSize);
        Graphics.Blit(texture2D, scaledRT);

        Texture2D outputTexture = new(newSize, newSize, texture2D.format, true);

        RenderTexture.active = scaledRT;
        outputTexture.ReadPixels(new Rect(0, 0, newSize, newSize), 0, 0);

        outputTexture.Apply();

        // Clean up
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(scaledRT);

        UnityEngine.Object.Destroy(texture2D);
        texture2D = outputTexture;

        if (savePath != null)
        {
            SaveTextureAsPNG(texture2D, savePath);
        }
    }

    public static void GetTextureFromNonReadable(ref Texture2D texture2D)
    {

        //if (texture2D)
        //{
        //    return;
        //}

        RenderTexture scaledRT = RenderTexture.GetTemporary(texture2D.width, texture2D.height);
        Graphics.Blit(texture2D, scaledRT);

        Texture2D outputTexture = new(texture2D.width, texture2D.height, TextureFormat.RGBA32, true);

        RenderTexture.active = scaledRT;
        outputTexture.ReadPixels(new Rect(0, 0, texture2D.width, texture2D.height), 0, 0);

        outputTexture.Apply();

        // Clean up
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(scaledRT);

        UnityEngine.Object.Destroy(texture2D);
        texture2D = outputTexture;
    }

    public static void Format(ref Texture2D texture2D, TextureFormat newTextureFormat)
    {
        Texture2D newTexture2D = new(texture2D.width, texture2D.height, newTextureFormat, true);

        for (int i = 0; i < texture2D.mipmapCount; i++)
        {
            newTexture2D.SetPixels(texture2D.GetPixels(i), i);
        }

        newTexture2D.Apply();

        UnityEngine.Object.Destroy(texture2D);
        texture2D = newTexture2D;
    }

    public static void SaveTextureAsPNG(Texture2D texture2D, string path)
    {

        //if (texture2D || path is null)
        //{
        //    return;
        //}

        Directory.CreateDirectory(new FileInfo(path).DirectoryName);
        File.WriteAllBytes(path, texture2D.EncodeToPNG());
    }
}