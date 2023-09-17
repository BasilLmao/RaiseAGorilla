using UnityEngine;
using UnityEngine.Assertions.Must;

namespace RaiseAGorilla.Scripts
{
    internal class PressableButton : GorillaPressableButton
    {
        internal enum ButtonType
        {
            LeftArrowButton,
            RightArrowButton,
            PurchaseButton,
            ClickGorillaButton,
            UpgradesTabButton,
            CosmeticsTabButton
        }

        internal ButtonType buttonType;
        private readonly float cooldown = 0.07f;
        private float cooldownTime = 0.014f;

        private void LateUpdate()
            => cooldownTime -= Time.deltaTime;

        private void OnTriggerEnter(Collider collider)
        {
            GorillaTriggerColliderHandIndicator colliderHandIndicator;
            if (cooldownTime > 0.0 || !collider.TryGetComponent<GorillaTriggerColliderHandIndicator>(out colliderHandIndicator))
                return;

            cooldownTime = cooldown;
            GorillaTagger.Instance.StartVibration(colliderHandIndicator.isLeftHand, 0.3f, 0.05f);
            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, colliderHandIndicator.isLeftHand, 0.07f);

            if (buttonType == ButtonType.LeftArrowButton || buttonType == ButtonType.RightArrowButton)
                Main.Instance.AttemptClickArrowButton(this);
            if (buttonType == ButtonType.PurchaseButton)
            {
                if (Main.Instance.currentTab == Main.Instance.shopTabs[0])
                {
                    Main.Instance.AttemptClickPurchaseButton();
                }
                else
                {
                    if (!Main.Instance.currentCosmetic.boughtOnce)
                    {
                        Main.Instance.AttemptClickPurchaseButton();
                    }
                    else
                    {
                        Main.Instance.AttemptClickEquipOrUnEquipCosmeticButton();
                    }
                }
            }
            if (buttonType == ButtonType.ClickGorillaButton)
                Main.Instance.AttemptClickGorilla();
            if (buttonType == ButtonType.UpgradesTabButton || buttonType == ButtonType.CosmeticsTabButton)
                Main.Instance.AttemptClickTabButton(this);
        }
    }
}
