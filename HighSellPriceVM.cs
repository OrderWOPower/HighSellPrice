using System;
using System.Linq;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using SandBox.ViewModelCollection.Nameplate;

namespace HighSellPrice
{
    [HarmonyPatch(typeof(SettlementNameplateVM), "RefreshDynamicProperties")]
    public class HighSellPriceVM
    {
        // If the player has non-food trade goods of any number or food of more than 1 each with high selling prices, add an icon to the city's nameplate. If not, remove the icon.
        public static void Postfix(SettlementNameplateVM __instance)
        {
            if (__instance.IsInRange && __instance.Settlement.IsTown)
            {
                ItemRoster itemRoster = MobileParty.MainParty.ItemRoster;
                int num = 0;
                for (int i = 0; i < itemRoster.Count; i++)
                {
                    ItemObject itemAtIndex = itemRoster.GetItemAtIndex(i);
                    if ((!itemAtIndex.IsFood && itemAtIndex.IsTradeGood) || (itemAtIndex.IsFood && itemRoster.GetElementNumber(i) > 1))
                    {
                        ItemCategory itemCategory = itemAtIndex.ItemCategory;
                        float num2 = 0f;
                        float num3 = 0f;
                        if (Town.AllTowns != null)
                        {
                            foreach (Town town in Town.AllTowns)
                            {
                                num2 += town.GetItemCategoryPriceIndex(itemCategory, false) + town.GetItemCategoryPriceIndex(itemCategory, false);
                                num3 += 1f;
                            }
                        }
                        num2 /= num3 * 2f;
                        if (__instance.Settlement.Town.GetItemPrice(itemAtIndex, MobileParty.MainParty, true) / (float)itemAtIndex.Value > num2 * 1.3f)
                        {
                            num++;
                        }
                    }
                }
                SettlementEventsVM settlementEventsVM = __instance.SettlementEvents;
                SettlementNameplateEventItemVM settlementNameplateEventItemVM = settlementEventsVM.EventsList.FirstOrDefault((SettlementNameplateEventItemVM e) => e.EventType == (SettlementNameplateEventItemVM.SettlementEventType)EventTypes);
                if (num > 0)
                {
                    if (!settlementEventsVM.EventsList.Contains(settlementNameplateEventItemVM))
                    {
                        settlementEventsVM.EventsList.Add(new SettlementNameplateEventItemVM((SettlementNameplateEventItemVM.SettlementEventType)EventTypes));
                    }
                }
                else
                {
                    settlementEventsVM.EventsList.Remove(settlementNameplateEventItemVM);
                }
            }
        }
        public static int EventTypes => Enum.GetNames(typeof(SettlementNameplateEventItemVM.SettlementEventType)).Length;
    }
}
