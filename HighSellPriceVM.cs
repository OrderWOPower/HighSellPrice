﻿using HarmonyLib;
using SandBox.ViewModelCollection.Nameplate;
using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace HighSellPrice
{
    [HarmonyPatch(typeof(SettlementNameplateVM), "RefreshDynamicProperties")]
    public class HighSellPriceVM
    {
        public static int NumOfEventTypes => Enum.GetNames(typeof(SettlementNameplateEventItemVM.SettlementEventType)).Length;

        public static void Postfix(SettlementNameplateVM __instance)
        {
            if (__instance.IsInRange && __instance.Settlement.IsTown)
            {
                ItemRoster itemRoster = MobileParty.MainParty.ItemRoster;
                MBBindingList<SettlementNameplateEventItemVM> eventsList = __instance.SettlementEvents.EventsList;
                SettlementNameplateEventItemVM highSellPriceEvent = eventsList.FirstOrDefault(e => e.EventType == (SettlementNameplateEventItemVM.SettlementEventType)NumOfEventTypes);
                HighSellPriceSettings settings = HighSellPriceSettings.Instance;
                int numOfHighSellingItems = 0;

                for (int i = 0; i < itemRoster.Count; i++)
                {
                    ItemObject item = itemRoster.GetItemAtIndex(i);
                    int elementNumber = itemRoster.GetElementNumber(i);
                    bool isFood = item.IsFood, isCraftable = item.ItemCategory == DefaultItemCategories.Iron || item.ItemCategory == DefaultItemCategories.Wood, isOther = item.IsTradeGood && !isFood && !isCraftable;

                    if (((isFood && settings.ShouldCountFood && elementNumber >= settings.MinFoodCount) || (isCraftable && settings.ShouldCountCraftables && elementNumber >= settings.MinCraftableCount) || (isOther && settings.ShouldCountOthers && elementNumber >= settings.MinOtherCount)) && !Campaign.Current.GetCampaignBehavior<IViewDataTracker>().GetInventoryLocks().Contains(CampaignUIHelper.GetItemLockStringID(itemRoster.GetElementCopyAtIndex(i).EquipmentElement)))
                    {
                        float num = 0f, num2 = 0f;

                        if (Town.AllTowns != null)
                        {
                            foreach (Town town in Town.AllTowns)
                            {
                                num += town.GetItemCategoryPriceIndex(item.ItemCategory);
                                num2++;
                            }
                        }

                        num /= num2;

                        if (__instance.Settlement.Town.GetItemPrice(item, MobileParty.MainParty, true) / (float)item.Value > num * 1.3f)
                        {
                            numOfHighSellingItems++;
                        }
                    }
                }

                if (numOfHighSellingItems > 0)
                {
                    if (!eventsList.Contains(highSellPriceEvent))
                    {
                        // If the player has trade goods of more than the configurable number with high selling prices, add an icon to the city's nameplate. 
                        eventsList.Add(new SettlementNameplateEventItemVM((SettlementNameplateEventItemVM.SettlementEventType)NumOfEventTypes));
                    }
                }
                else
                {
                    // If not, remove the icon.
                    eventsList.Remove(highSellPriceEvent);
                }
            }
        }
    }
}
