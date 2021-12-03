using HarmonyLib;
using TaleWorlds.GauntletUI;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.Nameplate;

namespace HighSellPrice
{
    [HarmonyPatch(typeof(SettlementNameplateEventVisualWidget), "UpdateVisual")]
    public class HighSellPriceWidget
    {
        // Set the icon widget to a red coin.
        private static void Prefix(BrushWidget __instance, int type)
        {
            if (type == HighSellPriceVM.EventTypesLength)
            {
                __instance.SetState("HighSellingPrice");
                return;
            }
        }
    }
}
