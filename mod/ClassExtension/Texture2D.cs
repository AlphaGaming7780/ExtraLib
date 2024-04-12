using System;
using System.Collections.Generic;
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
}
