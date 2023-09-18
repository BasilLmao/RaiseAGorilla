using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RaiseAGorilla.Scripts
{
    internal class Main : MonoBehaviour
    {
        public static Main Instance { get; private set; }

        internal bool ModInitialized;

        internal int cash = 0;

        private GameObject RaiseAGorilla;

        // Upgrade Shop
        internal Text PlayerStatsText;
        private Text UpgradeShopStartupText;
        private Text UpgradeShopMonitorNameText;
        internal Text UpgradeShopPageText;
        internal Text UpgradeText;
        internal Upgrade currentUpgrade;
        internal List<Upgrade> upgrades;
        internal Text PurchaseButtonText;

        // Page
        internal bool onShopStartupPage = true;
        internal int currentPage = 0;
        private int pages = 3; // Starts at 0

        // Tab
        internal string currentTab;
        internal List<string> shopTabs;

        // AutoClick
        internal float AutoClickGorillaSpeed = 1f;
        internal float AutoClickGorillaTime;

        // Save
        internal float saveCooldown = 15f;
        internal float nextSave;

        // ???
        private GameObject s_01;

        // Cosmetic Shop / Cosmetics
        internal Cosmetic currentCosmetic;
        internal List<Cosmetic> cosmetics;

        private GameObject BirthdayCake_I;
        private GameObject BirthdayCake_II;

        #region Initialize
        private async void Start()
        {
            if (Instance == null)
                Instance = this;

            RaiseAGorilla = GameObject.Instantiate(await ModUtilities.LoadAsset<GameObject>(Plugin.Instance.RaiseAGorillaBundle, "RaiseAGorilla"));
            RaiseAGorilla.transform.position = new Vector3(-50.0253f, 27.0473f, -93.4337f);
            RaiseAGorilla.transform.localEulerAngles = new Vector3(0, 0, 0);
            RaiseAGorilla.transform.localScale = new Vector3(1, 1, 1);

            PlayerStatsText = GameObject.Find("RaiseAGorilla(Clone)/Monitor - Player Stats/MonitorCanvas/StatsText").GetComponent<Text>();
            UpgradeShopStartupText = GameObject.Find("RaiseAGorilla(Clone)/Monitor - Upgrade Shop/MonitorCanvas/StartupText").GetComponent<Text>();
            UpgradeShopMonitorNameText = GameObject.Find("RaiseAGorilla(Clone)/Monitor - Upgrade Shop/MonitorCanvas/MonitorNameText").GetComponent<Text>();
            UpgradeShopPageText = GameObject.Find("RaiseAGorilla(Clone)/Monitor - Upgrade Shop/MonitorCanvas/PageText").GetComponent<Text>();
            UpgradeText = GameObject.Find("RaiseAGorilla(Clone)/Monitor - Upgrade Shop/MonitorCanvas/UpgradeText").GetComponent<Text>();
            PurchaseButtonText = GameObject.Find("RaiseAGorilla(Clone)/Button Stand - Shop Navigator/StandCanvas/PurchaseButtonText").GetComponent<Text>();

            s_01 = GameObject.Find("RaiseAGorilla(Clone)/Secrets/???_01");
            BirthdayCake_I = GameObject.Find("RaiseAGorilla(Clone)/Gorilla/Cosmetics/Cake_I");
            BirthdayCake_II = GameObject.Find("RaiseAGorilla(Clone)/Gorilla/Cosmetics/Cake_II");

            GameObject leftArrowButton = GameObject.Find("RaiseAGorilla(Clone)/Button Stand - Shop Navigator/Buttons/LeftButton");
            leftArrowButton.GetComponent<BoxCollider>().isTrigger = true;
            leftArrowButton.layer = LayerMask.NameToLayer("GorillaInteractable");
            leftArrowButton.AddComponent<PressableButton>().buttonType = PressableButton.ButtonType.LeftArrowButton;

            GameObject rightArrowButton = GameObject.Find("RaiseAGorilla(Clone)/Button Stand - Shop Navigator/Buttons/RightButton");
            rightArrowButton.GetComponent<BoxCollider>().isTrigger = true;
            rightArrowButton.layer = LayerMask.NameToLayer("GorillaInteractable");
            rightArrowButton.AddComponent<PressableButton>().buttonType = PressableButton.ButtonType.RightArrowButton;

            GameObject purchaseButton = GameObject.Find("RaiseAGorilla(Clone)/Button Stand - Shop Navigator/Buttons/PurchaseButton");
            purchaseButton.GetComponent<BoxCollider>().isTrigger = true;
            purchaseButton.layer = LayerMask.NameToLayer("GorillaInteractable");
            purchaseButton.AddComponent<PressableButton>().buttonType = PressableButton.ButtonType.PurchaseButton;

            GameObject clickableGorillaCollider = GameObject.Find("RaiseAGorilla(Clone)/Gorilla/Click Collider");
            clickableGorillaCollider.GetComponent<CapsuleCollider>().isTrigger = true;
            clickableGorillaCollider.layer = LayerMask.NameToLayer("GorillaInteractable");
            clickableGorillaCollider.AddComponent<PressableButton>().buttonType = PressableButton.ButtonType.ClickGorillaButton;

            GameObject upgradesTabButton = GameObject.Find("RaiseAGorilla(Clone)/Button Stand - Tab Navigator/Buttons/UpgradeShopButton");
            upgradesTabButton.GetComponent<BoxCollider>().isTrigger = true;
            upgradesTabButton.layer = LayerMask.NameToLayer("GorillaInteractable");
            upgradesTabButton.AddComponent<PressableButton>().buttonType = PressableButton.ButtonType.UpgradesTabButton;

            GameObject cosmeticsTabButton = GameObject.Find("RaiseAGorilla(Clone)/Button Stand - Tab Navigator/Buttons/CosmeticShopButton");
            cosmeticsTabButton.GetComponent<BoxCollider>().isTrigger = true;
            cosmeticsTabButton.layer = LayerMask.NameToLayer("GorillaInteractable");
            cosmeticsTabButton.AddComponent<PressableButton>().buttonType = PressableButton.ButtonType.CosmeticsTabButton;

            upgrades = new List<Upgrade>
            {
                new Upgrade("CASH PER CLICK", 25, 0, 25, "GIVES YOU MORE CASH PER CLICK.", false, 1), // Upgrade Shop Page 0
                new Upgrade("CASH PER SECOND", 30, 0, 27, "GIVES YOU MORE CASH PER SECOND.", false, 0), // Upgrade Shop Page 1
                new Upgrade("MULTIPLIER", 100, 0, 19, "GIVES YOU MORE MULTIPLIER.", false, 1), // Upgrade Shop Page 2
                new Upgrade("LIL BILLY", 1250, 0, 1, "???", false, 0) // Upgrade Shop Page 3
            };

            cosmetics = new List<Cosmetic>
            {
                new Cosmetic("BIRTHDAY CAKE I", 1000, "GIVES YOUR GORILLA A BIRTHDAY CAKE HAT FOR THE ONE YEAR ANNIVERSARY OF GORILLA TAG.", false, 5, BirthdayCake_I, false), // Cosmetic Shop Page 0
                new Cosmetic("BIRTHDAY CAKE II", 1500, "PUTS A BIRTHDAY CAKE ON THE TABLE FOR THE TWO YEAR ANNIVERSARY OF GORILLA TAG.", false, 10, BirthdayCake_II, false) // Cosmetic Shop Page 1
            };

            shopTabs = new List<string>
            {
                "UPGRADES TAB",
                "COSMETICS TAB"
            };
            currentTab = shopTabs[0];

            PlayerData playerData = DataSystem.GetPlayerData();
            cash = playerData.cash;
            upgrades[0].value = playerData.cashPerClick;
            upgrades[1].value = playerData.cashPerSecond;
            upgrades[0].upgradeAmountBought = playerData.currentTimesBoughtCPC;
            if (playerData.currentCPCCost == 0)
            {
                upgrades[0].upgradeCost = 25;
            }
            else
            {
                upgrades[0].upgradeCost = playerData.currentCPCCost;
            }
            upgrades[0].boughtOnce = playerData.boughtCPCOnce;
            upgrades[1].upgradeAmountBought = playerData.currentTimesBoughtCPS;
            if (playerData.currentCPSCost == 0)
            {
                upgrades[1].upgradeCost = 30;
            }
            else
            {
                upgrades[1].upgradeCost = playerData.currentCPSCost;
            }
            upgrades[1].boughtOnce = playerData.boughtCPSOnce;
            currentUpgrade = upgrades[currentPage];
            if (playerData.currentMultiplierCost == 0)
            {
                upgrades[2].upgradeCost = 100;
            }
            else
            {
                upgrades[2].upgradeCost = playerData.currentMultiplierCost;
            }
            upgrades[2].upgradeAmountBought = playerData.currentTimesBoughtMultiplier;
            upgrades[2].boughtOnce = playerData.boughtMultiplierOnce;
            upgrades[2].value = playerData.multiplier;
            upgrades[3].upgradeAmountBought = playerData.currentTimesBoughtLilBilly;
            upgrades[3].boughtOnce = playerData.boughtLilBilly;
            s_01.SetActive(upgrades[3].boughtOnce);

            cosmetics[0].boughtOnce = playerData.boughtBDayCake_I;
            cosmetics[0].equipped = playerData.bDayCakeI_Equipped;
            BirthdayCake_I.SetActive(cosmetics[0].equipped);
            cosmetics[1].boughtOnce = playerData.boughtBDayCake_II;
            cosmetics[1].equipped = playerData.bDayCakeII_Equipped;
            BirthdayCake_II.SetActive(cosmetics[1].equipped);

            ModInitialized = true;
        }

        private void Update()
        {
            if (ModInitialized)
            {
                if (Time.time > nextSave)
                {
                    DataSystem.SaveData();
                    nextSave = Time.time + saveCooldown;
                }

                PlayerStatsText.text = $"CASH : {cash}\r\nCASH PER CLICK : {upgrades[0].value}\r\nCASH PER SECOND : {upgrades[1].value}\r\nMULTIPLIER {upgrades[2].value}\r\nREBIRTHS : ???";

                if (!onShopStartupPage)
                {
                    UpgradeShopPageText.text = $"PAGE : {currentPage}";

                    UpgradeShopMonitorNameText.text = currentTab;

                    if (currentTab == shopTabs[0])
                    {
                        UpgradeText.text = $"{currentUpgrade.upgradeName}\r\nCOST : {currentUpgrade.upgradeCost}\r\nAMOUNT BOUGHT : {currentUpgrade.upgradeAmountBought}\r\nINFO : {currentUpgrade.upgradeInfo}";
                    }
                    else
                    {
                        UpgradeText.text = $"{currentCosmetic.cosmeticName}\r\nCOST : {currentCosmetic.cosmeticCost}\r\nINFO : {currentCosmetic.cosmeticInfo}";
                    }

                    if (currentUpgrade != null && currentUpgrade.upgradeAmountBought < currentUpgrade.maxAmountCanBuy || currentCosmetic != null && !currentCosmetic.boughtOnce)
                    {
                        PurchaseButtonText.text = "PURCHASE";
                    }
                    else if (currentTab == shopTabs[1] && currentCosmetic != null)
                    {
                        if (currentCosmetic.boughtOnce && !currentCosmetic.equipped)
                        {
                            PurchaseButtonText.text = "EQUIP";
                        }
                        else
                        {
                            PurchaseButtonText.text = "UNEQUIP";
                        }
                    }
                    else
                    {
                        PurchaseButtonText.text = "BOUGHT";
                    }
                }

                if (upgrades[1].value > 0)
                {
                    if (Time.time > AutoClickGorillaTime)
                    {
                        AttemptAutoClickGorilla();
                        AutoClickGorillaTime = Time.time + AutoClickGorillaSpeed;
                    }
                }
            }
        }
        #endregion

        internal void AttemptClickArrowButton(PressableButton button)
        {
            if (onShopStartupPage)
            {
                UpgradeShopStartupText.gameObject.SetActive(false);
                UpgradeShopMonitorNameText.gameObject.SetActive(true);
                UpgradeShopPageText.gameObject.SetActive(true);
                UpgradeText.gameObject.SetActive(true);

                onShopStartupPage = !onShopStartupPage;
                currentUpgrade = upgrades[0];

                #if DEBUG
                Debug.Log("[RaiseAGorilla] Showing UpgradesTab ");
                #endif
            }
            else
            {
                if (button.buttonType == PressableButton.ButtonType.LeftArrowButton)
                {
                    if (currentPage > 0)
                    {
                        if (currentTab == shopTabs[0])
                        {
                            currentPage = currentPage - 1 + upgrades.Count % upgrades.Count;
                            currentUpgrade = upgrades[currentPage];
                        }
                        else
                        {
                            currentPage = currentPage - 1 + cosmetics.Count % cosmetics.Count;
                            currentCosmetic = cosmetics[currentPage];
                        }
                    }
                }
                else if (button.buttonType == PressableButton.ButtonType.RightArrowButton)
                {
                    if (currentPage < pages)
                    {
                        if (currentTab == shopTabs[0])
                        {
                            currentPage = currentPage + 1 % upgrades.Count;
                            currentUpgrade = upgrades[currentPage];
                        }
                        else
                        {
                            currentPage = currentPage + 1 % cosmetics.Count;
                            currentCosmetic = cosmetics[currentPage];
                        }
                    }
                }
            }
        }

        internal void AttemptClickPurchaseButton()
        {
            if (!onShopStartupPage)
            {
                if (currentTab == shopTabs[0])
                {
                    if (currentUpgrade.upgradeAmountBought < currentUpgrade.maxAmountCanBuy && cash >= currentUpgrade.upgradeCost)
                    {
                        #if DEBUG
                        Debug.Log($"[RaiseAGorilla] Bought : {currentUpgrade.upgradeName} for {currentUpgrade.upgradeCost}");
                        #endif

                        cash -= currentUpgrade.upgradeCost;
                        if (!currentUpgrade.boughtOnce || currentUpgrade.upgradeName == "MULTIPLIER")
                        {
                            if (currentUpgrade.upgradeName == "LIL BILLY")
                            {
                                s_01.SetActive(true);
                                upgrades[2].value += 10;
                            }
                            else
                            {
                                currentUpgrade.value++;
                            }
                        }
                        else
                        {
                            currentUpgrade.value *= 2;
                        }
                        currentUpgrade.boughtOnce = true;
                        currentUpgrade.upgradeCost *= 2;
                        currentUpgrade.upgradeAmountBought++;
                    }
                }
                else
                {
                    if (!currentCosmetic.boughtOnce && cash >= currentCosmetic.cosmeticCost)
                    {
                        cash -= currentCosmetic.cosmeticCost;
                        upgrades[2].value += currentCosmetic.addValue;
                        currentCosmetic.boughtOnce = true;
                    }
                }
            }
        }

        internal void AttemptClickGorilla()
        {
            cash += upgrades[0].value * upgrades[2].value;

            #if DEBUG
            Debug.Log($"Clicked Gorilla - Cash : {cash}");
            #endif
        }

        internal void AttemptAutoClickGorilla()
        {
            cash += upgrades[1].value * upgrades[2].value;
        }

        internal void AttemptClickTabButton(PressableButton button)
        {
            if (onShopStartupPage)
            {
                UpgradeShopStartupText.gameObject.SetActive(false);
                UpgradeShopMonitorNameText.gameObject.SetActive(true);
                UpgradeShopPageText.gameObject.SetActive(true);
                UpgradeText.gameObject.SetActive(true);

                onShopStartupPage = !onShopStartupPage;
                currentUpgrade = upgrades[0];

                #if DEBUG
                Debug.Log("[RaiseAGorilla] Showing Upgrades Tab");
                #endif
            }
            else
            {
                if (button.buttonType == PressableButton.ButtonType.UpgradesTabButton)
                {
                    currentPage = 0;
                    pages = upgrades.Count - 1;
                    currentTab = shopTabs[0];
                    currentUpgrade = upgrades[currentPage];
                    currentCosmetic = null;
                }
                else if (button.buttonType == PressableButton.ButtonType.CosmeticsTabButton)
                {
                    currentPage = 0;
                    pages = cosmetics.Count - 1;
                    currentTab = shopTabs[1];
                    currentCosmetic = cosmetics[currentPage];
                    currentUpgrade = null;
                }

                #if DEBUG
                Debug.Log($"[RaiseAGorilla] Clicked A Tab Button - Tab : {currentTab}");
                #endif
            }
        }

        internal void AttemptClickEquipOrUnEquipCosmeticButton()
        {
            if (currentTab == shopTabs[1] && currentCosmetic.boughtOnce)
            {
                currentCosmetic.equipped = !currentCosmetic.equipped;
                currentCosmetic.cosmetic.SetActive(currentCosmetic.equipped);
            }
        }
    }
}
