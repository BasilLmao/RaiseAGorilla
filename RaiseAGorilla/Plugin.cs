using BepInEx;
using HarmonyLib;
using RaiseAGorilla.Scripts;
using System.Reflection;
using UnityEngine;

namespace RaiseAGorilla
{
    [BepInDependency("dev.auros.bepinex.bepinject")]
    [BepInPlugin("decalfree.raiseagorilla", "RaiseAGorilla", "1.0.1")]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }

        public AssetBundle RaiseAGorillaBundle { get; private set; }

        private async void Start()
        {
            if (Instance == null)
                Instance = this;

            RaiseAGorillaBundle = await ModUtilities.LoadFromStream("RaiseAGorilla.Resources.raiseagorilla");

            GorillaTagger.Instance.gameObject.AddComponent<Main>();

            new Harmony("decalfree.raiseagorilla").PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
