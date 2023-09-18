using UnityEngine;

namespace RaiseAGorilla.Scripts
{
    internal class Cosmetic
    {
        public string cosmeticName;
        public int cosmeticCost;
        public string cosmeticInfo;
        public bool boughtOnce;
        public int addValue;
        public GameObject cosmetic;
        public bool equipped;

        internal Cosmetic(string cosmeticName, int cosmeticCost, string cosmeticInfo, bool boughtOnce, int addValue, GameObject cosmetic, bool equipped)
        {
            this.cosmeticName = cosmeticName;
            this.cosmeticCost = cosmeticCost;
            this.cosmeticInfo = cosmeticInfo;
            this.boughtOnce = boughtOnce;
            this.addValue = addValue;
            this.cosmetic = cosmetic;
            this.equipped = equipped;
        }
    }
}
