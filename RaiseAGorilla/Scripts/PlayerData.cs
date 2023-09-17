namespace RaiseAGorilla.Scripts
{
    [System.Serializable]
    public class PlayerData
    {
        public float cash;
        public float cashPerClick;
        public float cashPerSecond;
        public int currentTimesBoughtCPC;
        public int currentCPCCost;
        public bool boughtCPCOnce;
        public int currentTimesBoughtCPS;
        public int currentCPSCost;
        public bool boughtCPSOnce;
        public float multiplier;
        public int currentTimesBoughtMultiplier;
        public int currentMultiplierCost;
        public bool boughtMultiplierOnce;
        public int currentTimesBoughtLilBilly;
        public bool boughtLilBilly;
        public bool boughtBDayCake_I;
        public bool bDayCakeI_Equipped;
        public bool boughtBDayCake_II;
        public bool bDayCakeII_Equipped;

        internal PlayerData()
        {
            cash = Main.Instance.cash;
            cashPerClick = Main.Instance.upgrades[0].value;
            cashPerSecond = Main.Instance.upgrades[1].value;
            currentTimesBoughtCPC = Main.Instance.upgrades[0].upgradeAmountBought;
            currentCPCCost = Main.Instance.upgrades[0].upgradeCost;
            boughtCPCOnce = Main.Instance.upgrades[0].boughtOnce;
            currentTimesBoughtCPS = Main.Instance.upgrades[1].upgradeAmountBought;
            currentCPSCost = Main.Instance.upgrades[1].upgradeCost;
            boughtCPSOnce = Main.Instance.upgrades[1].boughtOnce;
            multiplier = Main.Instance.upgrades[2].value;
            currentTimesBoughtMultiplier = Main.Instance.upgrades[2].upgradeAmountBought;
            currentMultiplierCost = Main.Instance.upgrades[2].upgradeCost;
            boughtMultiplierOnce = Main.Instance.upgrades[2].boughtOnce;
            currentTimesBoughtLilBilly = Main.Instance.upgrades[3].upgradeAmountBought;
            boughtLilBilly = Main.Instance.upgrades[3].boughtOnce;
            boughtBDayCake_I = Main.Instance.cosmetics[0].boughtOnce;
            bDayCakeI_Equipped = Main.Instance.cosmetics[0].equipped;
            boughtBDayCake_II = Main.Instance.cosmetics[1].boughtOnce;
            bDayCakeII_Equipped = Main.Instance.cosmetics[1].equipped;
        }
    }
}
