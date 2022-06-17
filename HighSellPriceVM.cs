using HarmonyLib;
using SandBox.ViewModelCollection.Nameplate;
using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace HighSellPrice
{
    [HarmonyPatch(typeof(SettlementNameplateVM), "RefreshDynamicProperties")]
    public class HighSellPriceVM
    {
        public static int EventTypesLength => Enum.GetNames(typeof(SettlementNameplateEventItemVM.SettlementEventType)).Length;

        // If the player has trade goods of more than the configurable number with high selling prices, add an icon to the city's nameplate. If not, remove the icon.
        public static void Postfix(SettlementNameplateVM __instance)
        {
            if (__instance.IsInRange && __instance.Settlement.IsTown)
            {
                ItemRoster itemRoster = MobileParty.MainParty.ItemRoster;
                HighSellPriceSettings settings = HighSellPriceSettings.Instance;
                int num = 0;
                for (int i = 0; i < itemRoster.Count; i++)
                {
                    ItemObject itemAtIndex = itemRoster.GetItemAtIndex(i);
                    int elementNumber = itemRoster.GetElementNumber(i);
                    bool isFood = itemAtIndex.IsFood;
                    bool isCraftable = itemAtIndex.ItemCategory == DefaultItemCategories.Iron || itemAtIndex.ItemCategory == DefaultItemCategories.Wood;
                    bool isOther = itemAtIndex.IsTradeGood && !isFood && !isCraftable;
                    if ((isFood && settings.ShouldCountFood && elementNumber >= settings.MinFoodCount) || (isCraftable && settings.ShouldCountCraftables && elementNumber >= settings.MinCraftableCount) || (isOther && settings.ShouldCountOthers && elementNumber >= settings.MinOtherCount))
                    {
                        ItemCategory itemCategory = itemAtIndex.ItemCategory;
                        float num2 = 0f;
                        float num3 = 0f;
                        if (Town.AllTowns != null)
                        {
                            foreach (Town town in Town.AllTowns)
                            {
                                num2 += town.GetItemCategoryPriceIndex(itemCategory) + town.GetItemCategoryPriceIndex(itemCategory);
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
                SettlementNameplateEventsVM settlementNameplateEventsVM = __instance.SettlementEvents;
                SettlementNameplateEventItemVM settlementNameplateEventItemVM = settlementNameplateEventsVM.EventsList.FirstOrDefault((SettlementNameplateEventItemVM e) => e.EventType == (SettlementNameplateEventItemVM.SettlementEventType)EventTypesLength);
                if (num > 0)
                {
                    if (!settlementNameplateEventsVM.EventsList.Contains(settlementNameplateEventItemVM))
                    {
                        settlementNameplateEventsVM.EventsList.Add(new SettlementNameplateEventItemVM((SettlementNameplateEventItemVM.SettlementEventType)EventTypesLength));
                    }
                }
                else
                {
                    settlementNameplateEventsVM.EventsList.Remove(settlementNameplateEventItemVM);
                }
            }
        }
    }
}
