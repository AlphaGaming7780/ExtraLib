using System.IO;
using UnityEngine;

namespace ExtraLib.Helper;

class TextureHelper
{
        public static Texture2D ResizeTexture(Texture2D texture, int newSize, string savePath = null)
        {

            if (texture is null)
            {
                // Plugin.Logger.LogWarning("The input texture2D is null @ ResizeTexture");
                return null;
            }

            // Plugin.Logger.LogMessage(savePath);

            // if(texture is not Texture2D texture2D) {
            // 	texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, true);
            // 	Graphics.CopyTexture(texture, texture2D);
            // }

            // int height = (int)((float)texture.height/texture.width * newSize);

            RenderTexture scaledRT = RenderTexture.GetTemporary(newSize, newSize);
            Graphics.Blit(texture, scaledRT);

            Texture2D outputTexture = new(newSize, newSize, texture.format, true);

            RenderTexture.active = scaledRT;
            outputTexture.ReadPixels(new Rect(0, 0, newSize, newSize), 0, 0);

            outputTexture.Apply();

            // Clean up
            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(scaledRT);

            if (savePath != null)
            {
                SaveTexture(outputTexture, savePath);
            }

            return outputTexture;

        }

        public static Texture2D GetTextureFromNonReadable(Texture2D texture2D)
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

        public static void SaveTexture(Texture2D texture2D, string path)
        {
            Directory.CreateDirectory(new FileInfo(path).DirectoryName);
            File.WriteAllBytes(path, texture2D.EncodeToPNG());
        }
}