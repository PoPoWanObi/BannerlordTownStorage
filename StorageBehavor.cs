using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace Storage
{
    internal class StorageBehavior : CampaignBehaviorBase
    {
        public ItemRoster Roster = new ItemRoster();

        public override void RegisterEvents()
        {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
        }

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData<ItemRoster>("StorageRoster", ref this.Roster);
        }

        public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
        {
            AddGameMenus(campaignGameStarter);
        }

        protected void AddGameMenus(CampaignGameStarter campaignGameStarter)
        {
            campaignGameStarter.AddGameMenuOption("town", "player_town_storage", "Enter Storage", delegate (MenuCallbackArgs args)
            {
                Settlement currentSettlement = Settlement.CurrentSettlement;
                bool flag = currentSettlement != null && currentSettlement.OwnerClan.IsAtWarWith(Hero.MainHero.MapFaction);
                args.Tooltip = (flag ? new TextObject("You are currently in war with this faction and can't access the storage.", null) : new TextObject("", null));
                args.IsEnabled = !flag;
                args.optionLeaveType = GameMenuOption.LeaveType.Trade;
                return true;
            }, delegate (MenuCallbackArgs args)
            {
                InventoryManager.OpenScreenAsStash(this.Roster);
            }, false, 2, true, null);
        }
    }
}
