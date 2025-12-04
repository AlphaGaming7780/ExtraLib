#if DEBUG
using Colossal.Logging;
using Game;
using Game.Modding;
using HarmonyLib;

namespace ExtraLib.Patches
{
    internal class ModLoadOrderTest
    {
        [HarmonyPatch(typeof(ModManager.ModInfo), "OnLoad")]
        class ModInfo
        {
            private static ILog moddingLog = LogManager.GetLogger("Modding").SetShowsErrorsInUI(showsErrorsInUI: false);
            static void Postfix(ModManager.ModInfo __instance, UpdateSystem updateSystem)
            {
                moddingLog.Info($"Calling mod OnLoad : {__instance.asset.id} | {__instance.asset.name}");
            }
        }
    }
}
#endif