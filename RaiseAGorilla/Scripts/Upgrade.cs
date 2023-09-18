namespace RaiseAGorilla.Scripts
{
    internal class Upgrade
    {
        public string upgradeName;
        public int upgradeCost;
        public int upgradeAmountBought;
        public int maxAmountCanBuy;
        public string upgradeInfo;
        public bool boughtOnce;
        public int value;

        internal Upgrade(string upgradeName, int upgradeCost, int upgradeAmountBought, int maxAmountCanBuy, string upgradeInfo, bool boughtOnce, int value)
        {
            this.upgradeName = upgradeName;
            this.upgradeCost = upgradeCost;
            this.upgradeAmountBought = upgradeAmountBought;
            this.maxAmountCanBuy = maxAmountCanBuy;
            this.upgradeInfo = upgradeInfo;
            this.boughtOnce = boughtOnce;
            this.value = value;
        }
    }
}
