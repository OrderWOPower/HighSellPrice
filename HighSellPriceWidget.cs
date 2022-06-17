using HarmonyLib;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.Nameplate;

namespace HighSellPrice
{
    [HarmonyPatch(typeof(SettlementNameplateEventVisualBrushWidget), "UpdateVisual")]
    public class HighSellPriceWidget
    {
        // Set the icon widget to a red coin.
        private static void Prefix(SettlementNameplateEventVisualBrushWidget __instance, int type)
        {
            if (type == HighSellPriceVM.EventTypesLength)
            {
                __instance.SetState("HighSellingPrice");
                return;
            }
        }
    }
}
