using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.UI;
using Game.UI.Widgets;
using System.Collections.Generic;

namespace ExtraDetailingTools
{
    [FileLocation(nameof(ExtraDetailingTools))]
    [SettingsUIGroupOrder(kButtonGroup, kToggleGroup, kSliderGroup, kDropdownGroup)]
    [SettingsUIShowGroupName(kButtonGroup, kToggleGroup, kSliderGroup, kDropdownGroup)]
    public class GameSetting : ModSetting
    {
        public const string kSection = "Main";

        public const string kButtonGroup = "Button";
        public const string kToggleGroup = "Toggle";
        public const string kSliderGroup = "Slider";
        public const string kDropdownGroup = "Dropdown";

        public GameSetting(IMod mod) : base(mod)
        {

        }

        [SettingsUISection(kSection, kButtonGroup)]
        public bool Button { set { ELT.Logger.Info("Button clicked"); } }

        [SettingsUIButton]
        [SettingsUIConfirmation]
        [SettingsUISection(kSection, kButtonGroup)]
        public bool ButtonWithConfirmation { set { ELT.Logger.Info("ButtonWithConfirmation clicked"); } }

        [SettingsUISection(kSection, kToggleGroup)]
        public bool Toggle { get; set; }

        [SettingsUISlider(min = 0, max = 100, step = 1, scalarMultiplier = 1, unit = Unit.kDataMegabytes)]
        [SettingsUISection(kSection, kSliderGroup)]
        public int IntSlider { get; set; }

        [SettingsUIDropdown(typeof(GameSetting), nameof(GetIntDropdownItems))]
        [SettingsUISection(kSection, kDropdownGroup)]
        public int IntDropdown { get; set; }

        [SettingsUISection(kSection, kDropdownGroup)]
        public SomeEnum EnumDropdown { get; set; } = SomeEnum.Value1;

        public DropdownItem<int>[] GetIntDropdownItems()
        {
            var items = new List<DropdownItem<int>>();

            for (var i = 0; i < 3; i += 1)
            {
                items.Add(new DropdownItem<int>()
                {
                    value = i,
                    displayName = i.ToString(),
                });
            }

            return items.ToArray();
        }

        public override void SetDefaults()
        {
            throw new System.NotImplementedException();
        }

        public enum SomeEnum
        {
            Value1,
            Value2,
            Value3,
        }
    }

    public class LocaleEN : IDictionarySource
    {
        private readonly GameSetting m_Setting;
        public LocaleEN(GameSetting setting)
        {
            m_Setting = setting;
        }
        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "Mod settings sample" },
                { m_Setting.GetOptionTabLocaleID(GameSetting.kSection), "Main" },

                { m_Setting.GetOptionGroupLocaleID(GameSetting.kButtonGroup), "Buttons" },
                { m_Setting.GetOptionGroupLocaleID(GameSetting.kToggleGroup), "Toggle" },
                { m_Setting.GetOptionGroupLocaleID(GameSetting.kSliderGroup), "Sliders" },
                { m_Setting.GetOptionGroupLocaleID(GameSetting.kDropdownGroup), "Dropdowns" },

                { m_Setting.GetOptionLabelLocaleID(nameof(GameSetting.Button)), "Button" },
                { m_Setting.GetOptionDescLocaleID(nameof(GameSetting.Button)), $"Simple single button. It should be bool property with only setter or use [{nameof(SettingsUIButtonAttribute)}] to make button from bool property with setter and getter" },

                { m_Setting.GetOptionLabelLocaleID(nameof(GameSetting.ButtonWithConfirmation)), "Button with confirmation" },
                { m_Setting.GetOptionDescLocaleID(nameof(GameSetting.ButtonWithConfirmation)), $"Button can show confirmation message. Use [{nameof(SettingsUIConfirmationAttribute)}]" },
                { m_Setting.GetOptionWarningLocaleID(nameof(GameSetting.ButtonWithConfirmation)), "is it confirmation text which you want to show here?" },

                { m_Setting.GetOptionLabelLocaleID(nameof(GameSetting.Toggle)), "Toggle" },
                { m_Setting.GetOptionDescLocaleID(nameof(GameSetting.Toggle)), $"Use bool property with setter and getter to get toggable option" },

                { m_Setting.GetOptionLabelLocaleID(nameof(GameSetting.IntSlider)), "Int slider" },
                { m_Setting.GetOptionDescLocaleID(nameof(GameSetting.IntSlider)), $"Use int property with getter and setter and [{nameof(SettingsUISliderAttribute)}] to get int slider" },

                { m_Setting.GetOptionLabelLocaleID(nameof(GameSetting.IntDropdown)), "Int dropdown" },
                { m_Setting.GetOptionDescLocaleID(nameof(GameSetting.IntDropdown)), $"Use int property with getter and setter and [{nameof(SettingsUIDropdownAttribute)}(typeof(SomeType), nameof(SomeMethod))] to get int dropdown: Method must be static or instance of your setting class with 0 parameters and returns {typeof(DropdownItem<int>).Name}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(GameSetting.EnumDropdown)), "Simple enum dropdown" },
                { m_Setting.GetOptionDescLocaleID(nameof(GameSetting.EnumDropdown)), $"Use any enum property with getter and setter to get enum dropdown" },

                { m_Setting.GetEnumValueLocaleID(GameSetting.SomeEnum.Value1), "Value 1" },
                { m_Setting.GetEnumValueLocaleID(GameSetting.SomeEnum.Value2), "Value 2" },
                { m_Setting.GetEnumValueLocaleID(GameSetting.SomeEnum.Value3), "Value 3" },
            };
        }

        public void Unload()
        {

        }
    }
}
