//using Colossal.IO.AssetDatabase;
//using Colossal.Logging;
//using ExtraDetailingTools.Settings;
//using Game;
//using Game.ELTding;
//using Game.SceneFlow;
//using Game.SceneFlow;

//namespace ExtraLandscapingTools
//{
//    public class ExtraLandscapingTools : IELT
//    {
//        public static ILog Logger = LogManager.GetLogger($"{nameof(ExtraLandscapingTools)}.{nameof(Plugin)}").SetShowsErrorsInUI(false);
//        private GameSetting m_Setting;

//        public void OnLoad(UpdateSystem updateSystem)
//        {
//            log.Info(nameof(OnLoad));

//            if (GameManager.instance.ELTManager.TryGetExecutableAsset(this, out var asset))
//                log.Info($"Current ELT asset at {asset.path}");

//            m_Setting = new GameSetting(this);
//            m_Setting.RegisterInOptionsUI();
//            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));

//            AssetDatabase.global.LoadSettings(nameof(ExtraLandscapingTools), m_Setting, new GameSetting(this));
//        }

//        public void OnDispose()
//        {
//            log.Info(nameof(OnDispose));
//            if (m_Setting != null)
//            {
//                m_Setting.UnregisterInOptionsUI();
//                m_Setting = null;
//            }
//        }
//    }
//}
