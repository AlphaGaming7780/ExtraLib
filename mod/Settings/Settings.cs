using System;
using System.IO;
using Colossal.Json;
using ExtraDetailingTools.Patches;

namespace ExtraDetailingTools
{
    public class Settings
    {
        internal static ELTSettings settings;

        internal static T LoadSettings<T>(string id, T ExtensionSettings)
        {
            if (Directory.Exists($"{ELT.PathToMods}\\Settings"))
            {
                if (File.Exists($"{ELT.PathToMods}\\Settings\\{id}.json"))
                {
                    ExtensionSettings = Decoder.Decode(File.ReadAllText($"{ELT.PathToMods}\\Settings\\{id}.json")).Make<T>();
                }
            }
            return ExtensionSettings;
        }

        internal static void SaveSettings<T>(string id, T ExtensionSettings)
        {
            if (!Directory.Exists($"{ELT.PathToMods}\\Settings")) Directory.CreateDirectory($"{ELT.PathToMods}\\Settings");
            File.WriteAllText($"{ELT.PathToMods}\\Settings\\{id}.json", Encoder.Encode(ExtensionSettings, EncodeOptions.None));
        }
    }

    [Serializable]
    internal class ELTSettings
    {
        public bool LoadCustomSurfaces = true;
        public bool LoadCustomDecals = true;
        public bool EnableTransformSection = true;
        public bool EnableSnowSurfaces = false;
        // public bool UseGameFolderForCache = false;
    }

}