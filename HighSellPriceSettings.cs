using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;

namespace HighSellPrice
{
    public class HighSellPriceSettings : AttributeGlobalSettings<HighSellPriceSettings>
    {
        public override string Id => "HighSellPrice";

        public override string DisplayName => "High Sell Price Alert";

        public override string FolderName => "HighSellPrice";

        public override string FormatType => "json2";

        [SettingPropertyBool("Toggle Food", Order = 0, RequireRestart = false, HintText = "Count food items. Enabled by default.", IsToggle = true)]
        [SettingPropertyGroup("Food", GroupOrder = 0)]
        public bool ShouldCountFood { get; set; } = true;

        [SettingPropertyInteger("Minimum Food Count", 1, 100, "0", Order = 1, RequireRestart = false, HintText = "Minimum number of food items to trigger the alert. Default is 2.")]
        [SettingPropertyGroup("Food", GroupOrder = 0)]
        public int MinFoodCount { get; set; } = 2;

        [SettingPropertyBool("Toggle Craftables", Order = 0, RequireRestart = false, HintText = "Count craftable items. Enabled by default.", IsToggle = true)]
        [SettingPropertyGroup("Craftables", GroupOrder = 1)]
        public bool ShouldCountCraftables { get; set; } = true;

        [SettingPropertyInteger("Minimum Craftable Count", 1, 100, "0", Order = 1, RequireRestart = false, HintText = "Minimum number of craftable items to trigger the alert. Default is 2.")]
        [SettingPropertyGroup("Craftables", GroupOrder = 1)]
        public int MinCraftableCount { get; set; } = 2;

        [SettingPropertyBool("Toggle Others", Order = 0, RequireRestart = false, HintText = "Count other items. Enabled by default.", IsToggle = true)]
        [SettingPropertyGroup("Others", GroupOrder = 2)]
        public bool ShouldCountOthers { get; set; } = true;

        [SettingPropertyInteger("Minimum Other Count", 1, 100, "0", Order = 1, RequireRestart = false, HintText = "Minimum number of other items to trigger the alert. Default is 1.")]
        [SettingPropertyGroup("Others", GroupOrder = 2)]
        public int MinOtherCount { get; set; } = 1;
    }
}
