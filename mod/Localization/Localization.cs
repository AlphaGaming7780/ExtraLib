using Colossal.Json;
using Colossal.Localization;
using Extra;
using Extra.Lib.Debugger;
using Game.Prefabs;
using Game.SceneFlow;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Extra.Lib.Localization;

public class ExtraLocalization
{
    /// <summary>
    /// Load the Localization file/files.
    /// To use this function your embedded files need to be in embedded/Localization/... .json
    /// - logger => The logger of your mod, used in case something need to be printed.
    /// - assemnby => The assemby of your mod, used to get the embedded files
    /// - globalFile => true if you have one localizaton file for all the local id.
    /// </summary>
    /// <param name="logger">The logger of your mod, used in case something need to be printed.</param>
    /// <param name="assembly">The assemby of your mod, used to get the embedded files</param>
    /// <param name="singleFile">true if you have one localizaton file for all the local id.</param>
    public static void LoadLocalization(Logger logger, Assembly assembly, bool singleFile, string defaultLocalID = "en-US") 
	{
        logger.Info("Start loading the localization.");
		try
		{
            if (singleFile)
            {
                logger.Info("Loading Global Localization file");
                Dictionary<string, Dictionary<string, string>> localization = Decoder.Decode(new StreamReader(assembly.GetManifestResourceStream($"{assembly.GetName().Name}.embedded.Localization.Localization.json")).ReadToEnd()).Make<Dictionary<string, Dictionary<string, string>>>();
                foreach (string localeID in GameManager.instance.localizationManager.GetSupportedLocales())
                {
                    logger.Info($"Loading {localeID}");
                    string LoadingLocalID = localeID;
                    if (!localization.ContainsKey(localeID))
                    {
                        LoadingLocalID = defaultLocalID;
                        logger.Warn($"No {localeID} in the global file, using {defaultLocalID} instead.");
                    }
                    GameManager.instance.localizationManager.AddSource(localeID, new MemorySource(localization[LoadingLocalID]));
                }
            }
            else
            {
                logger.Info("Loading multiple Localization file");
                foreach (string localeID in GameManager.instance.localizationManager.GetSupportedLocales())
                {
                    logger.Info($"Loading {localeID}");
                    Dictionary<string, string> localization;

                    if (assembly.GetManifestResourceNames().Contains($"{assembly.GetName().Name}.embedded.Localization.{localeID}.json"))
                        localization = Decoder.Decode(new StreamReader(assembly.GetManifestResourceStream($"{assembly.GetName().Name}.embedded.Localization.{localeID}.json")).ReadToEnd()).Make<Dictionary<string, string>>();
                    else
                    {
                        localization = Decoder.Decode(new StreamReader(assembly.GetManifestResourceStream($"{assembly.GetName().Name}.embedded.Localization.{defaultLocalID}.json")).ReadToEnd()).Make<Dictionary<string, string>>();
                        logger.Warn($"No {localeID} in the files, using {defaultLocalID} instead.");
                    }

                    GameManager.instance.localizationManager.AddSource(localeID, new MemorySource(localization));
                }
            }
        } catch (Exception ex) { logger.Error(ex); }
	}
}

//[Serializable]
//internal class GlobaleLocalizationJS
//{
//	public Dictionary<string, Dictionary<string, string>> Localization = [];

//}

//[Serializable]
//internal class LocalLocalizationJS
//{
//	public Dictionary<string, string> Localization = [];
//}