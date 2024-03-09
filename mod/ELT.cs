using System;
using System.Reflection;
using System.IO;
using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using ExtraDetailingTools.Patches;
using Game;
using Game.Modding;
using Game.Prefabs;
using Game.Rendering;
using Game.SceneFlow;
using Game.Tools;
using Game.UI.InGame;
using Unity.Entities;
using UnityEngine;
using HarmonyLib;
using Colossal.PSI.Common;
using System.Drawing.Drawing2D;
using System.Linq;
using Colossal.PSI.Environment;
using System.IO.Compression;

namespace ExtraDetailingTools
{
    public class ELT : IMod
    {

        static internal readonly string ELTGameDataPath = $"{EnvPath.kStreamingDataPath}\\Mods\\ELT"; //: $"{EnvPath.kUserDataPath}\\Mods\\ELT"; Settings.settings.UseGameFolderForCache ? 
        static internal readonly string ELTUserDataPath = $"{EnvPath.kUserDataPath}\\Mods\\ELT"; //: $"{EnvPath.kUserDataPath}\\Mods\\ELT"; Settings.settings.UseGameFolderForCache ? 
        static private string PathToParent;// = Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).FullName;
        public static string PathToMods;
        public static string PathToCustomBrushes;
        // public static readonly string PathToCustomSurface = Path.Combine(PathToMods,"CustomSurfaces");
        // public static readonly string PathToCustomDecal = Path.Combine(PathToMods, "CustomDecals");

        // static readonly string pathToZip = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\resources.zip";

        static internal string resources; //= Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "resources");
        // static internal readonly string resourcesIcons = Path.Combine(resources, "Icons");
        static internal string resourcesBrushes;
        // static internal readonly string resourcesCache = Path.Combine(resources, "Cache");


        public static PrefabSystem m_PrefabSystem;
        public static RenderingSystem m_RenderingSystem;
        public static EntityManager m_EntityManager;
        public static ToolSystem m_ToolSystem;
        public static ToolUISystem m_ToolUISystem;
        public static ToolbarUISystem m_ToolbarUISystem;
        public static SurfaceReplacerTool m_SurfaceReplacerTool;

        internal delegate string OnGetIcon(PrefabBase prefabBase);
        internal static OnGetIcon onGetIcon;

        public static ILog Logger = LogManager.GetLogger($"{nameof(ExtraDetailingTools)}").SetShowsErrorsInUI(false); //.{nameof(ELT)}
        // private GameSetting m_Setting;

        private Harmony harmony;

        public void OnLoad(UpdateSystem updateSystem)
        {
            Logger.Info(nameof(OnLoad));

            // if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
            //    Logger.Info($"Current ELT asset at {asset.path}");

            // m_Setting = new GameSetting(this);
            // m_Setting.RegisterInOptionsUI();
            // GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));

            // AssetDatabase.global.LoadSettings(nameof(ExtraDetailingTools), m_Setting, new GameSetting(this));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
            {
                PathToParent = Directory.GetParent(Path.GetDirectoryName(asset.path)).FullName;
                PathToMods = Path.Combine(PathToParent, "ExtraLandscapingTools_mods");
                PathToCustomBrushes = Path.Combine(PathToMods, "CustomBrushes");
                resources = Path.Combine(Path.GetDirectoryName(asset.path), "resources");
                resourcesBrushes = Path.Combine(resources, "Brushes");
            } else {
                Logger.Error("Failed to get the Asset");
                return;
            }

            // if (File.Exists(pathToZip))
            // {
            //     if (Directory.Exists(resources)) Directory.Delete(resources, true);
            //     ZipFile.ExtractToDirectory(pathToZip, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            //     File.Delete(pathToZip);
            // }

            Settings.settings = Settings.LoadSettings("ELT", new ELTSettings());
            ClearData();

            CustomSurfaces.SearchForCustomSurfacesFolder(PathToParent);
            CustomDecals.SearchForCustomDecalsFolder(PathToParent);

            if (Directory.Exists(resourcesBrushes)) PrefabSystem_OnCreate.FolderToLoadBrush.Add(resourcesBrushes);
            if (Directory.Exists(PathToCustomBrushes)) PrefabSystem_OnCreate.FolderToLoadBrush.Add(PathToCustomBrushes);


            harmony = new($"{nameof(ExtraDetailingTools)}.{nameof(ELT)}");
            harmony.PatchAll(typeof(ELT).Assembly);
            var patchedMethods = harmony.GetPatchedMethods().ToArray();
            Logger.Info($"Plugin ExtraDetailingTools made patches! Patched methods: " + patchedMethods.Length);
            foreach (var patchedMethod in patchedMethods)
            {
                Logger.Info($"Patched method: {patchedMethod.Module.Name}:{patchedMethod.Name}");
            }

        }

        public void OnDispose()
        {
            Logger.Info(nameof(OnDispose));
            harmony.UnpatchAll($"{nameof(ExtraDetailingTools)}.{nameof(ELT)}");
            //if (m_Setting != null)
            //{
            //    m_Setting.UnregisterInOptionsUI();
            //    m_Setting = null;
            //}
        }

        internal static Stream GetEmbedded(string embeddedPath)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream("ExtraDetailingTools.embedded." + embeddedPath);
        }

        internal static void ClearData()
        {
            if (Directory.Exists(ELT.ELTGameDataPath))
            {
                Directory.Delete(ELT.ELTGameDataPath, true);
            }
            if (Directory.Exists(ELT.ELTUserDataPath))
            {
                Directory.Delete(ELT.ELTUserDataPath, true);
            }
        }

        internal static void ExtractZip(string pathToZip)
        {

        }

        public static long MapUlongToLong(ulong ulongValue)
        {
            return unchecked((long)ulongValue + long.MinValue);
        }

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

        public static string GetIcon(PrefabBase prefab)
        {

            return "Media/Game/Icons/LotTool.svg";

            if (prefab is null) return $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/Misc/placeholder.svg";

            if (onGetIcon is not null) foreach (Delegate @delegate in onGetIcon.GetInvocationList())
                {

                    object result = @delegate.DynamicInvoke(prefab);

                    if (result is string s && s is not null) return s;
                }

            // if (File.Exists($"{ELT.resourcesIcons}/{prefab.GetType().Name}/{prefab.name}.svg")) return $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/{prefab.GetType().Name}/{prefab.name}.svg";

            if (prefab is SurfacePrefab)
            {

                // return prefab.name switch
                // {   
                // 	"Agriculture Surface 01" => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/SurfacePrefab/{prefab.name}.svg",
                // 	"Concrete Surface 01" => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/SurfacePrefab/{prefab.name}.svg",
                // 	"Concrete Surface 02" => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/SurfacePrefab/{prefab.name}.svg",
                // 	"Grass Surface 01" => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/SurfacePrefab/{prefab.name}.svg",
                // 	"Grass Surface 02" => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/SurfacePrefab/{prefab.name}.svg",
                // 	"Pavement Surface 01" => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/SurfacePrefab/{prefab.name}.svg",
                // 	"Pavement Surface 02" => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/SurfacePrefab/{prefab.name}.svg",
                // 	"Sand Surface 01" => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/SurfacePrefab/{prefab.name}.svg",
                // 	"Sand Surface 02" => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/SurfacePrefab/{prefab.name}.svg",
                // 	"Tiles Surface 01" => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/SurfacePrefab/{prefab.name}.svg",
                // 	"Tiles Surface 02" => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/SurfacePrefab/{prefab.name}.svg",
                // 	"Tiles Surface 03" => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/SurfacePrefab/{prefab.name}.svg",
                // 	"Forestry Surface 01" => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/SurfacePrefab/{prefab.name}.svg",
                // 	"Landfill Surface 01" => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/SurfacePrefab/{prefab.name}.svg",
                // 	"Oil Surface 01" => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/SurfacePrefab/{prefab.name}.svg",
                // 	"Ore Surface 01" => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/SurfacePrefab/{prefab.name}.svg",
                // 	_ => "Media/Game/Icons/LotTool.svg",
                // };

                return "Media/Game/Icons/LotTool.svg";

            }
            else if (prefab is UIAssetCategoryPrefab)
            {
                // return prefab.name switch
                // {   
                // 	"Surfaces" => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/UIAssetMenuPrefab/Custom Surfaces.svg",
                // 	"Misc Surfaces" => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/UIAssetCategoryPrefab/Misc Surfaces.svg",
                // 	"Concrete Surfaces" => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/UIAssetCategoryPrefab/Concrete Surfaces.svg",
                // 	"Rock Surfaces" => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/UIAssetCategoryPrefab/Rock Surfaces.svg",
                // 	"Wood Surfaces" => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/UIAssetCategoryPrefab/Wood Surfaces.svg",
                // 	"Ground Surfaces" => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/UIAssetCategoryPrefab/Ground Surfaces.svg",
                // 	"Tiles Surfaces" => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/UIAssetCategoryPrefab/Tiles Surfaces.svg",
                // 	"Decals" => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/UIAssetCategoryPrefab/Decals.svg",
                // 	_ => "Media/Game/Icons/LotTool.svg"
                // };
                return $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/Misc/placeholder.svg";
            }
            else if (prefab is UIAssetMenuPrefab)
            {

                // return prefab.name switch
                // {   
                // 	"Custom Surfaces" => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/UIAssetMenuPrefab/Custom Surfaces.svg",
                // 	_ => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/Misc/placeholder.svg"
                // };
                return $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/Misc/placeholder.svg";
            }
            else if (prefab.name.ToLower().Contains("decal") || prefab.name.ToLower().Contains("roadarrow"))
            {



                // return prefab.name switch
                // {   

                // 	_ => $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/Misc/placeholder.svg"
                // };
                try
                {
                    if (prefab is StaticObjectPrefab staticObjectPrefab)
                    {

                        // SpawnableObject spawnableObject = staticObjectPrefab.GetComponent<SpawnableObject>();
                        // if(spawnableObject is not null) {
                        // 	foreach(ObjectPrefab objectPrefab in spawnableObject.m_Placeholders) {
                        // 		if(objectPrefab is StaticObjectPrefab staticObjectPrefab1) {
                        // 			foreach(ObjectMeshInfo objectMeshInfo in staticObjectPrefab1.m_Meshes) {
                        // 				if(objectMeshInfo.m_Mesh is RenderPrefab renderPrefab) {
                        // 					foreach(Material material in renderPrefab.ObtainMaterials()) {
                        // 						foreach(string s in material.GetPropertyNames(MaterialPropertyType.Texture)) {ResizeTexture(material.GetTexture(s), 64, $"{ELT.resourcesIcons}\\Decals\\{prefab.name}\\{s}.png");} //\\{staticObjectPrefab1.name}\\{objectMeshInfo}
                        // 					}
                        // 				}
                        // 			}
                        // 		}
                        // 	}
                        // }

                        // foreach(ObjectMeshInfo objectMeshInfo in staticObjectPrefab.m_Meshes) {
                        // 	if(objectMeshInfo.m_Mesh is RenderPrefab renderPrefab) {
                        // 		foreach(Material material in renderPrefab.ObtainMaterials()) {
                        // 			foreach(string s in material.GetPropertyNames(MaterialPropertyType.Texture)) {ResizeTexture(material.GetTexture(s), 64, $"{ELT.resourcesIcons}\\Decals\\{prefab.name}\\{s}.png");}
                        // 		}
                        // 	}
                        // }

                        // if(staticObjectPrefab.m_Meshes[0].m_Mesh is RenderPrefab) {




                        // ResizeTexture(renderPrefab.ObtainMaterial(0).GetTexture("_BaseColorMap"), 64, $"{ELT.resourcesIcons}\\Decals\\{prefab.name}.png");
                        // return $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/Decals/{prefab.name}.png";
                        // }
                    }
                }
                catch (Exception e) { Logger.Error(e); }

                return $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/Misc/placeholder.svg";
            }

            return $"{GameManager_InitializeThumbnails.COUIBaseLocation}/resources/Icons/Misc/placeholder.svg";
        }
    }
}